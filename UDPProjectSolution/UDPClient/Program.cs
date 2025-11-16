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

        UdpClient checkClient = new UdpClient();
        IPEndPoint checkEP = new IPEndPoint(IPAddress.Parse(serverIP), port);

        checkClient.Client.ReceiveTimeout = 2000;

        byte[] checkMsg = Encoding.UTF8.GetBytes("CHECK_ADMIN");
        checkClient.Send(checkMsg, checkMsg.Length, checkEP);

        string adminStatus = "";
        try
        {
            byte[] resp = checkClient.Receive(ref checkEP);
            adminStatus = Encoding.UTF8.GetString(resp);
        }
        catch { }

        checkClient.Close();

        if (adminStatus == "EXISTS" && isAdmin)
        {
            Console.WriteLine("Admini ekziston tashmë. Nuk mund të hysh si admin.");
            isAdmin = false;
        }
        
 if (isAdmin)
 {
    UdpClient makeAdmin = new UdpClient();
    makeAdmin.Client.ReceiveTimeout = 2000;

    byte[] msg = Encoding.UTF8.GetBytes("SET_ADMIN");
    makeAdmin.Send(msg, msg.Length, checkEP);

    try
    {
        string response = Encoding.UTF8.GetString(makeAdmin.Receive(ref checkEP));

        if (response == "EXISTS")
        {
            Console.WriteLine("Admini ekziston tashmë. Nuk mund të hysh si admin.");
            isAdmin = false;
        }
        else
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
    }
     catch
   {
    Console.WriteLine("Gabim gjatë marrjes së përgjigjes.");
    isAdmin = false;
}

            makeAdmin.Close();
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

               Thread.Sleep(new Random().Next(800, 1500));

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
