using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class UDPClient
{
    static void Main(string[] args)
    {
        Console.Write("Shkruaj IP të serverit: ");
        string serverIP = Console.ReadLine();
        int port = 9000;

        UdpClient client = new UdpClient();
        IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverIP), port);

        Console.WriteLine("Klienti u nis.");
        Console.WriteLine("Shkruaj komanda: /list, /read, /search, /info, /delete, /upload, /download, STATS, /exit");
﻿
 while (true)
        {
            Console.Write("> ");
            string msg = Console.ReadLine();

            if (msg.ToLower() == "/exit")
                break;

            byte[] data = Encoding.UTF8.GetBytes(msg);
            client.Send(data, data.Length, serverEP);
