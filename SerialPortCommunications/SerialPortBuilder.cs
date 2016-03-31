// **************************************************************************************
//  filename    :   SerialPortBuilder.cs
//  purpose     :   provide an easy way to build up a SerialPort class before creating it
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-30
// **************************************************************************************


using System.IO.Ports;


namespace SerialPortCommunications
{
    public class SerialPortBuilder
    {
        private const int DEFAULT_DELAY_MS = 50;
        private const string DEFAULT_PORT_NAME = "COM1";
        private const int DEFAULT_BAUD_RATE = 19200;
        private const Parity DEFAULT_PARITY = Parity.None;
        private const int DEFAULT_DATA_BITS = 8;
        private const StopBits DEFAULT_STOP_BITS = StopBits.One;
        private const Handshake DEFAULT_HANDSHAKE = Handshake.None;
        private const int MAX_DATA_BITS = 8;
        private const int MIN_DATA_BITS = 5;

        public int Delay { get; set; } = DEFAULT_DELAY_MS;
        public string PortName { get; set; } = DEFAULT_PORT_NAME;
        public int BaudRate { get; set; } = DEFAULT_BAUD_RATE;
        public Parity Parity { get; set; } = DEFAULT_PARITY;
        public int DataBits { get; set; } = DEFAULT_DATA_BITS;
        public StopBits StopBits { get; set; } = DEFAULT_STOP_BITS;
        public Handshake Handshake { get; set; } = DEFAULT_HANDSHAKE;

        public SerialPort GetSerialPort()
        {
            var serialPort = new SerialPort(PortName, BaudRate, Parity, DataBits, StopBits);
            serialPort.Handshake = Handshake;
            serialPort.ReadTimeout = Delay;
            serialPort.WriteTimeout = Delay;
            return serialPort;
        }
    }
}
