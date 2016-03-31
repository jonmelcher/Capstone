// ***************************************************************************************
//  filename    :   Parallax28140Server.cs
//  purpose     :   provide a concrete SerialPortServer for the Parallax28140 RFID Reader
//                  which runs through the USB serial (COM3) at 2400 baudrate.  Server
//                  will scan incoming bytes and process them into 10 byte chunks
//                  representing scanned RFID IDs using the START and STOP bytes as guides
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-30
// ***************************************************************************************


using System;
using System.Linq;
using System.Threading;
using System.IO.Ports;
using GenericContainers;


namespace SerialPortCommunications
{
    public sealed class Parallax28140Server : SerialPortServer
    {
        private const byte START_TRANSMISSION = 0x0A;
        private const byte STOP_TRANSMISSION = 0x0D;
        private const int DEFAULT_DELAY_MS = 10;

        public RFIDToken CurrentScan { get; private set; }
        private ThreadSafeQueue<byte> Incoming { get; set; }

        public Parallax28140Server(SerialPort port) : base(port) { }

        // ***************************************************************************************
        //  method  :   public override void Start()
        //  purpose :   allow client to enable reading of the server from the current Port
        //  notes   :   calling the method will create a new read thread and open the current Port
        //              as well as create new Incoming Queue
        // ***************************************************************************************
        public override void StartServer()
        {
            StopServer();

            Incoming = new ThreadSafeQueue<byte>();

            Reader = new Thread(ReaderProcess);
            Reader.IsBackground = true;

            Port.Open();
            _isRunning = true;

            Reader.Start();
        }

        // ************************************************************************************
        //  method  :   public override void Stop()
        //  purpose :   allow client to stop the server from reading from the current open Port
        //  notes   :   calling the method will kill the read thread and close the current Port
        // ************************************************************************************
        public override void StopServer()
        {
            _isRunning = false;
            Reader?.Join();
            Port.Close();
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
                    byte read = (byte)Port.ReadChar();
                    if (CurrentScan != null)
                        continue;
                    ProcessCurrentRead(read);
                }
                catch (TimeoutException) { }

                Thread.Sleep(DEFAULT_DELAY_MS);
            }
        }

        private void ProcessCurrentRead(byte read)
        {
            switch (read)
            {
                case START_TRANSMISSION:
                    Incoming.Clear();
                    Incoming.Enqueue(read);
                    break;
                case STOP_TRANSMISSION:
                    Incoming.Enqueue(read);
                    byte[] transmission = Incoming.ToArray();
                    Incoming.Clear();
                    if (IsValidTransmission(transmission))
                    {
                        var buffer = new byte[RFIDToken.Length];
                        Array.Copy(transmission, 1, buffer, 0, buffer.Length);
                        CurrentScan = new RFIDToken(buffer);
                    }
                    break;
                default:
                    Incoming.Enqueue(read);
                    break;
            }
        }

        // testing for a valid key
        private bool IsValidTransmission(byte[] transmission)
        {
            if (transmission.Length != RFIDToken.Length + 2)  // if we get the wrong length
                return false;  // fail
            // if we're not bookended by start and stop bits
            if (transmission.First() != START_TRANSMISSION || transmission.Last() != STOP_TRANSMISSION)
                return false;  // fail
            // if we get a non-permissable value
            for (var i = 1; i < transmission.Length - 1; ++i)
                if (!char.IsLetterOrDigit((char)transmission[i]))
                    return false;

            return true;
        }

        public void ClearCurrent()
        {
            CurrentScan = null;
        }

        public void ClearIncoming()
        {
            Incoming.Clear();
        }
    }
}
