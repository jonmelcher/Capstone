using System;
using System.Linq;

namespace Capstone
{
    public sealed class Parallax28140Server : SerialPortServer
    {
        private const int BAUD_RATE = 2400;  // RFID baud
        private const byte START_TRANSMISSION = 0x0A;  // RFID start bit
        private const byte STOP_TRANSMISSION = 0x0D;  // RFID end bit

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
            Stop();  // flags _isRunning as false, stops read and closes the open port

            Incoming = new ThreadSafeQueue<byte>();

            Reader = new Thread(ReaderProcess);
            Reader.IsBackground = true;

            Port.Open();  // reopens the port
            _isRunning = true;  // resets _isRunning

            Reader.Start();  // and begins the thread
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
                        case START_TRANSMISSION:  // 0x0A
                            Incoming.Clear();  // clear out the container
                            Incoming.Enqueue(read);  // and populate with the first read bit
                            break;
                        case STOP_TRANSMISSION:  // 0x0D
                            Incoming.Enqueue(read);  // add the final bit to the container
                            byte[] transmission = Incoming.ToArray();  // shift everything to a byte array
                            Incoming.Clear();  // clear the container
                            if (IsValidTransmission(transmission))  // if we have the correct number of bits
                            {
                                Array.Copy(transmission, 1, buffer, 0, buffer.Length);  // copy
                                Tokens.Enqueue(new RFIDToken(buffer));  // and enqueue as an RFID token
                            }
                            break;
                        default:
                            Incoming.Enqueue(read);  // if we're somewhere between start and stop, enqueue
                            break;          
                    }
                }
                catch (TimeoutException) { }

                Thread.Sleep(DEFAULT_READ_DELAY_MS);
            }
        }

        // testing for a valid key
        private bool IsValidTransmission(byte[] transmission)
        {
            if (transmission.Length != RFIDToken.Length + 2)  // if we get the wrong length
                return false;  // fail
            // if we're not bookended by start and stop bits
            if (transmission.First != START_TRANSMISSION || transmission.Last != STOP_TRANSMISSION)
                return false;  // fail
            // if we get a non-permissable value
            if (transmission.Any(t => !char.IsLetterOrDigit(t)
                || t != START_TRANSMISSION || t != STOP_TRANSMISSION))
                return false;  // fail
            return true;  // else, succeed
        }
    }
}
