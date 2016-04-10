using System.IO.Ports;

namespace SerialCommunications
{
    public sealed class Parallax28140ServerFactory : ISerialPortServerFactory
    {
        private const string PORT_NAME = "COM3";
        private const int BAUD_RATE = 2400;
        private const Parity PARITY = Parity.None;
        private const int DATA_BITS = 8;
        private const StopBits STOP_BITS = StopBits.One;
        private const Handshake HANDSHAKE = Handshake.None;
        private const int READ_TIMEOUT = 500;
        private const int WRITE_TIMEOUT = 500;
        private static ISerialPortServerFactory _instance;

        private Parallax28140ServerFactory() { }

        public static ISerialPortServerFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Parallax28140ServerFactory();
                return _instance;
            }
        }

        public SerialPort CreatePort()
        {
            var port = new SerialPort(PORT_NAME, BAUD_RATE, PARITY, DATA_BITS, STOP_BITS);
            port.Handshake = HANDSHAKE;
            port.ReadTimeout = READ_TIMEOUT;
            port.WriteTimeout = WRITE_TIMEOUT;
            return port;
        }

        public SerialPortServer CreateServer()
        {
            return new Parallax28140Server(CreatePort());
        }
    }
}