using System.Linq;
using GenericContainers;


namespace SerialCommunications
{
    public class RFIDTransmission
    {
        private const byte START_TRANSMISSION = 0x0A;
        private const byte STOP_TRANSMISSION = 0x0D;
        private const byte TRANSMISSION_LENGTH = 10;

        public string CurrentRFIDTag { get; set; }
        private ThreadSafeQueue<byte> Incoming { get; set; } = new ThreadSafeQueue<byte>();
        private RFIDTransmissionState State { get; set; } = RFIDTransmissionState.Waiting;

        public void Read(byte read)
        {
            if (State == RFIDTransmissionState.Collecting)
                ProcessRead(read);
            else if (read == START_TRANSMISSION)
                State = RFIDTransmissionState.Collecting;
        }

        public void ProcessRead(byte read)
        {
            switch (read)
            {
                case START_TRANSMISSION:
                    Incoming.Clear();
                    break;
                case STOP_TRANSMISSION:
                    ProcessTransmission();
                    break;
                default:
                    Incoming.Enqueue(read);
                    break;
            }
        }
        
        public void Clear()
        {
            CurrentRFIDTag = string.Empty;
            Incoming.Clear();
        }


        private void ProcessTransmission()
        {
            var transmission = Incoming.EmptyIntoArray();
            if (IsValidTransmission(transmission))
                CurrentRFIDTag = GetTransmissionString(transmission);
            State = RFIDTransmissionState.Waiting;
        }

        private static string GetTransmissionString(byte[] id)
        {
            return new string(id.Select(b => (char)b).ToArray());
        }

        private static bool IsValidTransmission(byte[] id)
        {
            if (id == null || id.Length != TRANSMISSION_LENGTH)
                return false;
            return id.All(b => char.IsLetterOrDigit((char)b));
        }

        private enum RFIDTransmissionState { Waiting, Collecting }
    }
}