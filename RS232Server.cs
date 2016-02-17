using System;
using System.Collections.Generic;
using System.Threading;
using System.IO.Ports;

namespace Capstone
{
    public class RS232Server : SerialPortServer
    {
        protected const int DEFAULT_READ_DELAY_MS = 50;
        protected const int DEFAULT_WRITE_DELAY_MS = 50;
        protected const byte STOP_BIT = 0xFF;

        public RS232Server(SerialPortSettings sps) : base(sps) { }

        protected ThreadSafeQueue<byte> Incoming { get; set; }
        protected ThreadSafeQueue<byte> Outgoing { get; set; }

        // ********************************************************************************************
        //  method  :   public override void Start()
        //  purpose :   allow client to enable writing/reading of the server to/from the current Port
        //  notes   :   calling the method will create new read/write threads and open the current Port
        //              as well as create new Incoming/Outgoing Queues
        // ********************************************************************************************
        public override void Start()
        {
            Stop();  // sets _isRunning to false, ends the read, and closes the port

            Incoming = new ThreadSafeQueue<byte>();
            Outgoing = new ThreadSafeQueue<byte>();

            Reader = new Thread(ReaderProcess);  // prep threads
            Reader.IsBackground = true;

            Writer = new Thread(WriterProcess);
            Writer.IsBackground = true;

            Port.Open();  // open port to begin read
            _isRunning = true;

            Reader.Start();
            Writer.Start();
        }

        // ************************************************************************************************
        //  method  :   publicI override void Stop()
        //  purpose :   allow client to stop the server from writing and reading from the current open Port
        //  notes   :   calling the method will kill the read/write threads and close the current Port
        // ************************************************************************************************
        public override void Stop()
        {
            _isRunning = false;     // toggle
            Reader.Join();          // wait for reading thread to finish up after _isRunning is toggled
            Writer.Join();          // wait for writing thread to finish up after _isRunning is toggled
            Port.Close();           // close the open Port
        }

        public override void Write(byte b)
        {
            Outgoing.Enqueue(b);
        }

        public override void Write(byte[] arr)
        {
            if (arr == null)  // if we've got an unknown value
                return;  // fail out

            for (var i = 0; i < arr.Length; ++i)  // otherwise
                Write(arr[i]);  // write the full length of the value out
        }

        public override byte Read()
        {
            return Incoming.Count > 0 ? Incoming.Dequeue() : STOP_BIT;
        }

        // *****************************************************************************************************
        //  method  :   private void ReaderProcess()
        //  purpose :   provide a continuous method to run in a parallel thread method which will read in data
        //              from the open Port and place it in the Incoming Queue
        //  notes   :   -1 (STOP_BIT) will be enqueued into Incoming if the end of stream is met
        // *****************************************************************************************************
        private void ReaderProcess()
        {
            while (_isRunning)
            {
                try
                {
                    Incoming.Enqueue((byte)Port.ReadChar());
                }
                catch (TimeoutException) { }

                Thread.Sleep(DEFAULT_READ_DELAY_MS);
            }
        }

        // ******************************************************************************************************
        //  method  :   private void WriterProcess()
        //  purpose :   provide a continuous method to run in a parallel thread method which will write and clear
        //              contents of Outgoing queue to the open Port
        //  notes   :   
        // ******************************************************************************************************
        private void WriterProcess()
        {
            while (_isRunning)
            {
                byte? transmission = Outgoing.Count > 0 ? Outgoing.Dequeue() : null;
                try
                {
                    if (transmission != null)
                        Port.Write(new byte[] { transmission });
                }
                catch (TimeoutException) { }

                Thread.Sleep(DEFAULT_WRITE_DELAY_MS);
            }
        }
    }
}
