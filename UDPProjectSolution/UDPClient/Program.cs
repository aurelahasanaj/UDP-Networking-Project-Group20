using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class UDPClient
{
    static void Main(string[] args)
    {
        Console.Write("Shkruaj IP të serverit: ");
        string serverIP = Console.ReadLine();
        int port = 9000;
  Console.Write("A je admin? (po/jo): ");
        bool isAdmin = Console.ReadLine().Trim().ToLower() == "po";
 if (isAdmin)
        {
            Console.Write("Shkruaj fjalëkalimin e adminit: ");
            string pass = Console.ReadLine();

            if (pass != "admin123")
            {
                Console.WriteLine("Fjalëkalimi është gabim.");
                return;
            }

            Console.WriteLine("Qasje ADMIN.");
        }
        else
        {
            Console.WriteLine("Qasje normale (vetëm READ).");
        }
        UdpClient client = new UdpClient();
        IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverIP), port);

        Console.WriteLine("Klienti u nis.");
       Console.WriteLine("Komandat e lejuara:");

        if (isAdmin)
            Console.WriteLine("/list, /read, /search, /info, /delete, /upload, /download, STATS, /exit");
        else
            Console.WriteLine("/list, /read, /search, /info, /exit");

﻿
 while (true)
        {
            Console.Write("> ");
            string msg = Console.ReadLine();

            if (msg.ToLower() == "/exit")
                break;
       if (!isAdmin)
            {
                if (msg.StartsWith("/delete") ||
                    msg.StartsWith("/upload") ||
                    msg.StartsWith("/download"))
                {
                    Console.WriteLine("Nuk ke leje për këtë komandë.");
                    continue;
                }

                Thread.Sleep(1000);
            }

            byte[] data = Encoding.UTF8.GetBytes(msg);
            client.Send(data, data.Length, serverEP);
     try
            {
                client.Client.ReceiveTimeout = 5000;
                byte[] resp = client.Receive(ref serverEP);
                string response = Encoding.UTF8.GetString(resp);
                Console.WriteLine(response);
            }
            catch
            {
                Console.WriteLine("Serveri nuk u përgjigj.");
            }
        }

        client.Close();
    }
}
