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
    internal class RFIDTest
    {
        static void Main(string[] args)
        {
            Parallax28140Server server = Parallax28140ServerFactory.Instance.CreateServer() as Parallax28140Server;

            Console.Write("Press any key to start server... ");
            Console.ReadKey();
            server.StartServer();

            while (true)
            {
                if (server.CurrentID == string.Empty)
                    continue;
                
                Console.WriteLine(server.CurrentID);
                Console.Write("Press 's' to scan again... ");
                if (Console.ReadKey().Key == ConsoleKey.S)
                    server.ClearScanner();
            }
        }
    }
}