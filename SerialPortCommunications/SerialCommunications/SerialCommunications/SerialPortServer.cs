// **************************************************************************************
//  filename    :   SerialPortServer.cs
//  purpose     :   provide an abstract backing for the various serial port communication
//                  servers being used in the Automated Fleet Management System
//
//  written by Jonathan Melcher and Brennan MacGregor
//  last updated 2016-03-30
// **************************************************************************************


using System;
using System.IO.Ports;
using System.Threading;


namespace SerialCommunications
{
    abstract public class SerialPortServer
    {
        protected volatile bool _isRunning;

        protected SerialPortServer(SerialPort port)
        {
            if (port == null)
                throw new ArgumentNullException();
            Port = port;
        }

        public abstract void StartServer();
        public abstract void StopServer();

        protected SerialPort Port { get; set; }
        protected Thread Reader { get; set; }
        protected Thread Writer { get; set; }
    }
}
