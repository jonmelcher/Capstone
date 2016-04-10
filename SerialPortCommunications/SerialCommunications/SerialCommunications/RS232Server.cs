// ***************************************************************************************
//  filename    :   RS232Server.cs
//  purpose     :   provide a concrete SerialPortServer for the Motorola Microprocessor
//                  which runs through the built-in Serial Port (COM1) at 19200 baudrate.
//                  Server will scan incoming bytes and enqueue them for access higher up.
//
//  written by Jonathan Melcher and Brennan MacGregor
//  last updated 2016-03-30
// ***************************************************************************************


using System;
using System.Threading;
using System.IO.Ports;
using GenericContainers;


namespace SerialCommunications
{
    public sealed class RS232Server : SerialPortServer
    {
        private const int DEFAULT_READ_DELAY_MS = 50;
        private const int DEFAULT_WRITE_DELAY_MS = 50;
        private const byte STOP_BIT = 0xFF;

        private ThreadSafeQueue<byte> Incoming { get; set; }
        private ThreadSafeQueue<byte> Outgoing { get; set; }

        public RS232Server(SerialPort port) : base(port) { }

        public override void StartServer()
        {
            StopServer();

            Incoming = new ThreadSafeQueue<byte>();
            Outgoing = new ThreadSafeQueue<byte>();

            Reader = new Thread(ReaderProcess);
            Reader.IsBackground = true;

            Writer = new Thread(WriterProcess);
            Writer.IsBackground = true;

            Port.Open();
            _isRunning = true;

            Reader.Start();
            Writer.Start();
        }

        public override void StopServer()
        {
            _isRunning = false;
            Reader?.Join();
            Writer?.Join();
            Port.Close();
        }

        public void Write(byte b)
        {
            Outgoing.Enqueue(b);
        }

        public void Write(byte[] arr)
        {
            if (arr == null)
                return;

            for (var i = 0; i < arr.Length; ++i)
                Write(arr[i]);
        }

        public void ClearIncoming()
        {
            Incoming.Clear();
        }

        public byte Read()
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

        private void WriterProcess()
        {
            while (_isRunning)
            {
                byte? transmission = (Outgoing.Count > 0) ? (byte?)Outgoing.Dequeue() : null;
                try
                {
                    if (transmission.HasValue)
                        Port.Write(new byte[] { transmission.Value }, 0, 1);
                }
                catch (TimeoutException) { }

                Thread.Sleep(DEFAULT_WRITE_DELAY_MS);
            }
        }
    }
}
