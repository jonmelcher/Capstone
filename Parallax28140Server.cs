using System;
using System.Linq;


namespace Capstone
{
    public sealed class Parallax28140Server : SerialPortServer
    {
        private const int BAUD_RATE = 2400;
        private const byte START_TRANSMISSION = 0x0A;
        private const byte STOP_TRANSMISSION = 0x0D;

        public Parallax28140Server(string pN) : base(pN, BAUD_RATE) { }

        private ThreadSafeQueue<RFIDToken> Tokens { get; set; }

        // ***************************************************************************************
        //  method  :   public override void Start()
        //  purpose :   allow client to enable reading of the server from the current Port
        //  notes   :   calling the method will create a new read thread and open the current Port
        //              as well as create new Incoming Queue
        // ***************************************************************************************
        public override void Start()
        {
            Stop();

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
        public override void Stop()
        {
            _isRunning = false;     // toggle
            Reader.Join();          // wait for reading thread to finish up after _isRunning is toggled
            Port.Close();           // close the open Port
        }

        // *****************************************************************************************************
        //  method  :   private void ReaderProcess()
        //  purpose :   provide a continuous method to run in a parallel thread method which will read in data
        //              from the open Port and place it in the Incoming Queue
        //  notes   :   -1 (STOP_BIT) will be enqueued into Incoming if the end of stream is met
        // *****************************************************************************************************
        private void ReaderProcess()
        {
            byte[] buffer = new byte[10];

            while (_isRunning)
            {
                try
                {
                    byte read = (byte)Port.ReadChar();
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
                                Array.Copy(transmission, 1, buffer, 0, buffer.Length);
                                Tokens.Enqueue(new RFIDToken(buffer));
                            }
                            break;
                        default:
                            Incoming.Enqueue(read);
                            break;          
                    }
                }
                catch (TimeoutException) { }

                Thread.Sleep(DEFAULT_READ_DELAY_MS);
            }
        }

        private bool IsValidTransmission(byte[] transmission)
        {
            if (transmission.Length != RFIDToken.Length + 2)
                return false;
            if (transmission.First != START_TRANSMISSION || transmission.Last != STOP_TRANSMISSION)
                return false;
            if (transmission.Any(t => !char.IsLetterOrDigit(t)
                || t != START_TRANSMISSION || t != STOP_TRANSMISSION))
                return false;
            return true;
        }
    }
}