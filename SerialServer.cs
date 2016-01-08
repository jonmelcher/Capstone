using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Threading;


namespace Capstone
{
    public class SerialServer
    {
        private const int DEFAULT_TIMEOUT_MS = 1000;
        private const int DEFAULT_READ_DELAY_MS = 50;
        private const int DEFAULT_WRITE_DELAY_MS = 50;

        private volatile bool _isRunning;       // status as to whether the server is currently reading/writing
        private object _syncIncoming;           // key for Incoming Queue
        private object _syncOutgoing;           // key for Outgoing Queue

        // ********************************************************************************
        //  method  :   public SerialServer(SerialPortSettings sps)
        //  purpose :   sets up Incoming/Outgoing keys, and sets up Port without opening it
        // ********************************************************************************
        public SerialServer(SerialPortSettings sps)
        {
            _syncIncoming = new object();
            _syncOutgoing = new object();

            Port = new SerialPort(sps.PortName, sps.BaudRate, sps.Parity, sps.DataBits, sps.StopBits);
            Port.ReadTimeout = DEFAULT_TIMEOUT_MS;
            Port.WriteTimeout = DEFAULT_TIMEOUT_MS;
        }

        private SerialPort Port { get; set; }
        private Thread Reader { get; set; }
        private Thread Writer { get; set; }
        private Queue<byte> Incoming { get; set; }
        private Queue<byte> Outgoing { get; set; }

        // ********************************************************************************************
        //  method  :   public void Start()
        //  purpose :   allow client to enable writing/reading of the server to/from the current Port
        //  notes   :   calling the method will create new read/write threads and open the current Port
        //              as well as create new Incoming/Outgoing Queues
        // ********************************************************************************************
        public void Start()
        {
            if (_isRunning)
                return;

            Incoming = new Queue<byte>();
            Outgoing = new Queue<byte>();

            Reader = new Thread(Read);
            Reader.IsBackground = true;

            Writer = new Thread(Write);
            Writer.IsBackground = true;

            Port.Open();
            _isRunning = true;

            Reader.Start();
            Writer.Start();
        }

        // ************************************************************************************************
        //  method  :   public void Stop()
        //  purpose :   allow client to stop the server from writing and reading from the current open Port
        //  notes   :   calling the method will kill the read/write threads and close the current Port
        // ************************************************************************************************
        public void Stop()
        {
            if (!_isRunning)
                return;

            _isRunning = false;     // toggle
            Reader.Join();          // wait for reading thread to finish up after _isRunning is toggled
            Writer.Join();          // wait for writing thread to finish up after _isRunning is toggled
            Port.Close();           // close the open Port
        }

        // *****************************************************************************************************
        //  method  :   private void Read()
        //  purpose :   provide a continuous method to run in a parallel thread method which will read in data
        //              from the open Port and place it in the Incoming Queue
        //  notes   :   _syncIncoming should always be used to lock the Outgoing queue since it will be at least
        //              accessed by the main server thread and the reading thread
        // *****************************************************************************************************
        private void Read()
        {
            while (_isRunning)
            {
                lock (_syncIncoming)
                {
                    try
                    {
                        Incoming.Enqueue((byte)Port.ReadChar());
                    }
                    catch (TimeoutException) { }
                }

                Thread.Sleep(DEFAULT_READ_DELAY_MS);
            }
        }

        // ******************************************************************************************************
        //  method  :   private void Write()
        //  purpose :   provide a continuous method to run in a parallel thread method which will write and clear
        //              contents of Outgoing queue to the open Port
        //  notes   :   _syncOutgoing should always be used to lock the Outgoing queue since it will be at least
        //              accessed by the main server thread and the writing thread
        // ******************************************************************************************************
        private void Write()
        {
            while (_isRunning)
            {
                lock (_syncOutgoing)
                {
                    var buffer = Outgoing.ToArray();
                    Outgoing.Clear();

                    try
                    {
                        Port.Write(buffer, 0, buffer.Length);
                    }
                    catch (TimeoutException) { }
                }

                Thread.Sleep(DEFAULT_WRITE_DELAY_MS);
            }
        }
    }
}
