using System.IO.Ports;

namespace SerialCommunications
{
    public interface ISerialPortServerFactory
    {
        SerialPort CreatePort();
        SerialPortServer CreateServer();
    }
}