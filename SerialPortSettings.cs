using System;
using System.Collections.Generic;
using System.IO.Ports;


namespace Capstone
{
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
