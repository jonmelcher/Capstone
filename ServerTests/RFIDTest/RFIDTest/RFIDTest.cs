// *******************************************************************************
//  filename    :   RFIDTest.cs
//  purpose     :   test RFID ID scanning functionality of the Parallax28140Server
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-30
// *******************************************************************************


using System;
using SerialCommunications;


namespace RFIDTest
{
    class RFIDTest
    {
        static void Main(string[] args)
        {
            Parallax28140Server server = SerialPortServerFactory.CreateServer(
                SerialPortServerFactory.SerialPortServerType.Parallax28140) as Parallax28140Server;

            Console.Write("Press any key to start server... ");
            Console.ReadKey();
            server.StartServer();


            while (true)
            {
                if (server.CurrentScan != string.Empty)
                {
                    Console.WriteLine(server.CurrentScan);
                    Console.Write("Press 's' to scan again... ");
                    if (Console.ReadKey().Key == ConsoleKey.S)
                        server.ClearTransmission();
                }
            }
        }
    }
}