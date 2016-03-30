using System.IO.Ports;
using System.Threading;


namespace Capstone
{
    public abstract class SerialPortServer : ISerialWritable, ISerialReadable
    {
        protected const int DEFAULT_DELAY_MS = 10;
        protected volatile bool _isRunning;

        protected SerialPortServer(SerialPortSettings sps)
        {
            Port = new SerialPort(sps.PortName, sps.BaudRate, sps.Parity, sps.DataBits, sps.StopBits);
            Port.ReadTimeout = DEFAULT_DELAY_MS;
            Port.WriteTimeout = DEFAULT_DELAY_MS;
        }

        protected SerialPortServer(string pN, int bR) : this(new SerialPortSettings(pN, bR)) { }

        protected SerialPort Port { get; set; }
        protected Thread Reader { get; set; }
        protected Thread Writer { get; set; }

        public abstract void Start();
        public abstract void Stop();

        public virtual void Write(byte b) { }
        public virtual void Write(byte[] arr) { }
        public virtual byte Read() { return 0; }
    }
}
