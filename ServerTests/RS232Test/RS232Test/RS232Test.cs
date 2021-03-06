﻿// ******************************************************************************
//  filename    :   RS232Test.cs
//  purpose     :   test I/O and responses of microprocessor with the RS232Server
//
//  written by Jonathan Melcher and Brennan MacGregor on 2016-03-30
// ******************************************************************************


using System;
using System.Threading;
using System.Threading.Tasks;
using SerialCommunications;


namespace RS232Test
{
    internal class RS232Test
    {
        private const int DELAY = 500;
        
        static void Main(string[] args)
        {
            RS232Server server = RS232ServerFactory.Instance.CreateServer() as RS232Server;

            Console.Write("Press any key to start server... ");
            Console.ReadKey();

            server.StartServer();

            Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine($"Received: {server.Read()}");
                    Thread.Sleep(DELAY);
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine("Write a byte to the microprocessor:");
                    string input = Console.ReadLine();
                    byte b;

                    if (byte.TryParse(input, out b))
                        server.Write(b);

                    Thread.Sleep(DELAY);
                }
            });
        }
    }
}