using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Threading;


namespace SerialServer
{
    public class SerialServer
    {
        private volatile bool _isRunning;
        private Queue<char> _incoming;
        private Queue<char> _outgoing;

        public SerialServer(SerialPortSettings sps)
        {
            Port = new SerialPort(sps.PortName, sps.BaudRate, sps.Parity, sps.DataBits, sps.StopBits);
            Port.ReadTimeout = 1000;
            Port.WriteTimeout = 1000;
        }

        private SerialPort Port { get; set; }
        private Thread Reader { get; set; }
        private Thread Writer { get; set; }

        public void Start()
        {
            if (_isRunning)
                return;

            _incoming = new Queue<char>();
            _outgoing = new Queue<char>();

            Reader = new Thread(Read);
            Reader.IsBackground = true;

            Writer = new Thread(Write);
            Writer.IsBackground = true;

            Port.Open();
            _isRunning = true;
            Reader.Start();
            Writer.Start();
        }

        public void Stop()
        {
            if (!_isRunning)
                return;

            _isRunning = false;
            Reader.Join();
            Writer.Join();
            Port.Close();
        }

        private void Read()
        {
            while (_isRunning)
            {
                lock (_incoming)
                {
                    try
                    {
                        _incoming.Enqueue((char)Port.ReadChar());
                    }
                    catch (TimeoutException) { }
                }
            }
        }

        private void Write()
        {
            while (_isRunning)
            {
                lock (_outgoing)
                {
                    var buffer = _outgoing.ToArray();
                    _outgoing.Clear();

                    try
                    {
                        Port.Write(buffer, 0, buffer.Length);
                    }
                    catch (TimeoutException) { }
                }
            }
        }
    }

    public class SerialPortSettings
    {
        private static readonly List<int> AVAILABLE_BAUD_RATES = new List<int>
        {
            110, 150, 300, 1200, 2400, 4800, 9600, 19200,
            38400, 57600, 115200, 230400, 460800, 921600
        };

        private const string DEFAULT_PORT_NAME = "COM1";
        private const int DEFAULT_BAUD_RATE = 19200;
        private const Parity DEFAULT_PARITY = Parity.None;
        private const int DEFAULT_DATA_BITS = 8;
        private const StopBits DEFAULT_STOP_BITS = StopBits.One;
        private const Handshake DEFAULT_HANDSHAKE = Handshake.None;
        private const int MAX_DATA_BITS = 8;
        private const int MIN_DATA_BITS = 5;

        private readonly string _portName;
        private readonly int _baudRate;
        private readonly Parity _parity;
        private readonly int _dataBits;
        private readonly StopBits _stopBits;
        private readonly Handshake _handshake;

        public SerialPortSettings(string pN, int bR, Parity p, int dB, StopBits sB, Handshake hs)
        {
            var sC = StringComparer.OrdinalIgnoreCase;
            var availablePorts = SerialPort.GetPortNames();

            _portName = availablePorts.Any(port => sC.Equals(port, pN)) ? pN : DEFAULT_PORT_NAME;
            _baudRate = AVAILABLE_BAUD_RATES.Contains(bR) ? bR : DEFAULT_BAUD_RATE;
            _parity = p;
            _dataBits = (dB >= MIN_DATA_BITS && dB <= MAX_DATA_BITS) ? dB : DEFAULT_DATA_BITS;
            _stopBits = sB != StopBits.None ? sB : DEFAULT_STOP_BITS;
            _handshake = hs;
        }

        public string PortName { get { return _portName; } }
        public int BaudRate { get { return _baudRate; } }
        public Parity Parity { get { return _parity; } }
        public int DataBits { get { return _dataBits; } }
        public StopBits StopBits { get { return _stopBits; } }
        public Handshake Handshake { get { return _handshake; } }
    }
}
