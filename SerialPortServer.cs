using System;
using System.IO.Ports;
using System.Threading;

namespace Capstone
{
    abstract class SerialPortServer : ISerialWritable, ISerialReadable
    {
        protected const int DEFAULT_TIMEOUT_MS = 1000;
        protected volatile bool _isRunning;

        protected SerialPortServer(SerialPortSettings sps)
        {
            Port = new SerialPort(sps.PortName, sps.BaudRate, sps.Parity, sps.DataBits, sps.StopBits);
            Port.ReadTimeout = DEFAULT_TIMEOUT_MS;
            Port.WriteTimeout = DEFAULT_TIMEOUT_MS;
        }

        protected SerialPort Port { get; set; }
        protected Thread Reader { get; set; }
        protected Thread Writer { get; set; }

        public abstract void Start();
        public abstract void Stop();

        public virtual void Write(byte b) { }
        public virtual void Write(byte[] arr) { }
        public virtual byte Read() { }
    }
}