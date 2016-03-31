// ***********************************************************************************
//  filename    :   SerialPortServerFactory
//  purpose     :   provide an easy way to create the concrete classes for the various
//                  SerialPortServers in use in the Automated Fleet Management System
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-30
// ***********************************************************************************


using System;


namespace SerialCommunications
{
    public static class SerialPortServerFactory
    {
        public enum SerialPortServerType { RS232, Parallax28140 }

        public static SerialPortServer CreateServer(SerialPortServerType serverType)
        {
            var builder = new SerialPortBuilder();

            switch (serverType)
            {
                case SerialPortServerType.RS232:
                    builder.PortName = "COM1";
                    builder.BaudRate = 19200;
                    return new RS232Server(builder.GetSerialPort());
                case SerialPortServerType.Parallax28140:
                    builder.PortName = "COM3";
                    builder.BaudRate = 2400;
                    return new Parallax28140Server(builder.GetSerialPort());
                default:
                    throw new ArgumentException();
            }
        }
    }
}
