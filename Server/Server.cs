﻿using System;
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
    class Server
    {
        public static Thread tcpThread, udpThread, newPortThread;
        private static string default_Host = "127.0.0.1";
        private static int default_Port = 1550;

        private static IPAddress def_Address = IPAddress.Parse(default_Host);

        static void Main(string[] args)
        {
            tcpThread = new Thread(TCPListener);
            tcpThread.Start();
            threadAliveCheck(tcpThread, "tcp");

            udpThread = new Thread(UDPListener);
            udpThread.Start();
            threadAliveCheck(udpThread, "udp");

            //newPortThread = new Thread(newPort);
            //newPortThread.Start();
            //threadAliveCheck(newPortThread, "byte");
        }

        //Check Threads are active
        private static void threadAliveCheck(Thread thr, string name)
        {
            if (thr.IsAlive)
            {
                Console.Write(name.ToUpper() + " Listener ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Active\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.Write(name + " listener ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Inactive\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        //Thread active end


        //TCP Listener and Print
        private static void TCPListener()
        {
            Socket connection;
            TcpListener listener;

            try
            {
                listener = new TcpListener(def_Address, default_Port);
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
                Console.WriteLine("ERROR : TCP Connection dropped ~~~~ \n\n\n" + e);
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
                Console.WriteLine("ERROR : TCP Connection dropped ~~~~ \n\n\n" + e);
                Console.WriteLine(e);
            }
        }
        //TCP Listener and Print End

        //UDP Listener
        private static void UDPListener()
        {
            try
            {
                UdpClient udpListener = new UdpClient(default_Port);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, default_Port);

                try
                {
                    while (true)
                    {
                        byte[] reader = udpListener.Receive(ref endPoint);
                        Console.Write("Message UDP : ");
                        Console.WriteLine($" {Encoding.ASCII.GetString(reader, 0, reader.Length)}");
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    udpListener.Close();
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("ERROR : UDP Connection dropped ~~~~ \n\n\n" + e);
                Console.WriteLine(e);
            }
        }
        //UDP Listener End

        //Testing...
        private static void newPort()
        {
            Socket connection;
            TcpListener listener;

            try
            {
                listener = new TcpListener(def_Address, default_Port);
                listener.Start();

                while (true)
                {
                    connection = listener.AcceptSocket();

                    Console.WriteLine("Port " + default_Port + " FOUND ");

                    printListener(connection, listener);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR : New Port Connection dropped ~~~~ \n\n\n" + e);
            }
        }

    }
}
