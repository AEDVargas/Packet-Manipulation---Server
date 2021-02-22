using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread tcpThread = new Thread(TCPListener);
            tcpThread.Start();
            Thread newPortThread = new Thread(newPort);
            newPortThread.Start();

            Console.WriteLine("TCP now listening");
        }

        private static void newPort()
        {
            Socket connection;
            TcpListener listener;

            IPAddress def_Address = IPAddress.Parse("127.0.0.1");

            try
            {
                listener = new TcpListener(def_Address, 1550);
                listener.Start();

                while (true)
                {
                    connection = listener.AcceptSocket();

                    Console.WriteLine("Port 1550 FOUND ");

                    printListener(connection, listener);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR : Connection dropped ~~~~ \n" + e);
            }
        }

        private static void TCPListener()
        {
            Socket connection;
            TcpListener listener;

            IPAddress def_Address = IPAddress.Parse("127.0.0.1");

            try
            {
                listener = new TcpListener(def_Address, 1500);
                listener.Start();

                while (true)
                {
                    connection = listener.AcceptSocket();

                    Console.WriteLine("Port 1500 FOUND ");

                    printListener(connection, listener);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR : Connection dropped ~~~~ \n" + e);
            }

        }

        private static void printListener(Socket connection, TcpListener listener)   
        {
            NetworkStream socketStream = new NetworkStream(connection);

            Console.WriteLine("Connection received...");

            try
            {
                StreamReader sr = new StreamReader(socketStream); //Read input of data

                while (true)
                {
                    string reader = sr.ReadLine();

                    if (reader == "drop")
                    {
                        Console.WriteLine("Connection Dropped");
                        connection.Close();
                        listener.Stop();
                        TCPListener();
                        break;
                    }

                    Console.Write("Message TCP : ");
                    Console.WriteLine(reader);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: connection dropped."); //Error handling
                Console.WriteLine(e);
            }
        }

    }
}
