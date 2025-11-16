using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

class ClientStats
{
    public DateTime LastActive { get; set; }
    public long BytesIn { get; set; }
    public long BytesOut { get; set; }
    public int Messages { get; set; }
    public bool IsAdmin { get; set; }
}

class UDPServer
{
    static string SERVER_IP = "0.0.0.0";
    static int PORT = 9000;
    static UdpClient server;
    static Dictionary<string, ClientStats> klientet = new Dictionary<string, ClientStats>();
    static int MAX_KLIENTET = 10;
    static int TIMEOUT_SECONDS = 20;
    static object locker = new object();

    static void Main(string[] args)
    {
        Console.WriteLine("SERVERI UDP U NIS...");
        server = new UdpClient(PORT);

        if (!Directory.Exists("server_files"))
            Directory.CreateDirectory("server_files");

        if (!Directory.Exists("Logs"))
            Directory.CreateDirectory("Logs");

        Thread monitorimiThread = new Thread(MonitoroTimeout);
        monitorimiThread.Start();

        Console.WriteLine($"Serveri po dëgjon në IP {SERVER_IP} portin {PORT}");
        Console.WriteLine("---------------------------------------");
        
while (true)
        {
            IPEndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = server.Receive(ref clientEP);

            string klientKey = clientEP.ToString();
            string msg = Encoding.UTF8.GetString(data);

            bool isAdmin;

            lock (locker)
            {
                if (!klientet.ContainsKey(klientKey))
                {
                    if (klientet.Count >= MAX_KLIENTET)
                    {
                        Dergo(clientEP, "Serveri është i mbushur.");
                        continue;
                    }

                    bool beAdmin = klientet.Count == 0;

                    klientet[klientKey] = new ClientStats
                    {
                        LastActive = DateTime.Now,
                        BytesIn = data.Length,
                        BytesOut = 0,
                        Messages = 1,
                        IsAdmin = beAdmin
                    };

                    Console.WriteLine($"Klient i ri: {klientKey} (admin: {beAdmin})");
                }
                else
                {
                    klientet[klientKey].LastActive = DateTime.Now;
                    klientet[klientKey].BytesIn += data.Length;
                    klientet[klientKey].Messages+= 1;
                }
  isAdmin = klientet[klientKey].IsAdmin;
            }

            Console.WriteLine($"[{klientKey}] {msg}");
            RuajLogMesazh(klientKey, msg);
                
        if (msg.ToUpper() == "STATS")
{
    string stats = GjeneroStatistikat();
    Dergo(clientEP, stats);
    RuajLogStats(stats);
    continue;
}

if (msg.StartsWith("/list", StringComparison.OrdinalIgnoreCase))
{
    string[] files = Directory.GetFiles("server_files");
    if (files.Length == 0)
    {
        Dergo(clientEP, "Nuk ka asnjë file.");
    }
    else
    {
        string fileList = string.Join("\n", files.Select(f => Path.GetFileName(f)));
        Dergo(clientEP, fileList);
    }
    continue;
}

if (msg.StartsWith("/read", StringComparison.OrdinalIgnoreCase))
{
    string[] p = msg.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
    if (p.Length < 2)
    {
        Dergo(clientEP, "Përdorimi: /read <filename>");
        continue;
    }

    string file = p[1].Trim();
    string path = Path.Combine("server_files", file);

    if (!File.Exists(path))
    {
        Dergo(clientEP, "File nuk ekziston.");
        continue;
    }

    string content = File.ReadAllText(path);
    Dergo(clientEP, content);
    continue;
}

if (msg.StartsWith("/search", StringComparison.OrdinalIgnoreCase))
{
  string[] p = msg.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                if (p.Length < 2)
                {
                    Dergo(clientEP, "Përdorimi: /search <keyword>");
                    continue;
                }

                string keyword = p[1].Trim();
                string result = "";

    foreach (string f in Directory.GetFiles("server_files"))
    {
        foreach (string line in File.ReadAllLines(f))
        {
            if (line.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                result += $"{Path.GetFileName(f)}: {line}\n";
        }
    }

     if (result == "")
    result = "Nuk u gjet asgjë.";
    
    Dergo(clientEP, result);
    continue;
}

 if (msg.StartsWith("/info", StringComparison.OrdinalIgnoreCase))
 {
    string[] p = msg.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
   if (p.Length < 2)
   {
     Dergo(clientEP, "Përdorimi: /info <filename>");
    continue;
  }

                string file = p[1].Trim();
                string path = Path.Combine("server_files", file);
     
    if (!File.Exists(path))
    {
        Dergo(clientEP, "File nuk ekziston.");
        continue;
    }

    FileInfo fi = new FileInfo(path);
    string info =
        $"Emri: {fi.Name}\n" +
        $"Madhësia: {fi.Length} bytes\n" +
        $"Krijuar: {fi.CreationTime}\n" +
        $"Modifikuar: {fi.LastWriteTime}";

    Dergo(clientEP, info);
    continue;
}

if (msg.StartsWith("/delete", StringComparison.OrdinalIgnoreCase))
{
    string[] p = msg.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                if (p.Length < 2)
                {
                    Dergo(clientEP, "Përdorimi: /delete <filename>");
                    continue;
                }

                if (!isAdmin)
                {
                    Dergo(clientEP, "Nuk ke leje për /delete.");
                    continue;
                }

                string file = p[1].Trim();
                string path = Path.Combine("server_files", file);

                if (!File.Exists(path))
                {
                    Dergo(clientEP, "File nuk ekziston.");
                    continue;
                }

                File.Delete(path);
                Dergo(clientEP, "File u fshi.");
                continue;
              }

if (msg.StartsWith("/upload", StringComparison.OrdinalIgnoreCase))
{
    string[] p = msg.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
                if (p.Length < 3)
                {
                    Dergo(clientEP, "Përdorimi: /upload <filename> <përmbajtja>");
                    continue;
                }

                if (!isAdmin)
                {
                    Dergo(clientEP, "Nuk ke leje për /upload.");
                    continue;
                }

                string file = p[1].Trim();
                string content = p[2];
                string path = Path.Combine("server_files", file);

                File.WriteAllText(path, content);
                Dergo(clientEP, "File u ngarkua.");
                continue;
              }

if (msg.StartsWith("/download", StringComparison.OrdinalIgnoreCase))
{
    string[] p = msg.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                if (p.Length < 2)
                {
                    Dergo(clientEP, "Përdorimi: /download <filename>");
                    continue;
                }

                string file = p[1].Trim();
                string path = Path.Combine("server_files", file);

                if (!File.Exists(path))
                {
                    Dergo(clientEP, "File nuk ekziston.");
                    continue;
                }

                string content = File.ReadAllText(path);
                Dergo(clientEP, content);
                continue;
              }

Dergo(clientEP, "Mesazhi u pranua.");
}
    }
    
﻿static void Dergo(IPEndPoint ep, string message)
{
    byte[] bytes = Encoding.UTF8.GetBytes(message);
    lock (locker)
    {
        string key = ep.ToString();
        if (klientet.ContainsKey(key))
            klientet[key].BytesOut += bytes.Length;
    }
    server.Send(bytes, bytes.Length, ep);
}

static void MonitoroTimeout()
{
    while (true)
    {
        Thread.Sleep(5000);
        List<string> perMeHeq = new List<string>();

        lock (locker)
        {
            foreach (var kl in klientet)
            {
                if ((DateTime.Now - kl.Value.LastActive).TotalSeconds > TIMEOUT_SECONDS)
                    perMeHeq.Add(kl.Key)
            }

            foreach (string k in perMeHeq)
                klientet.Remove(k);
        }
  Console.WriteLine($"Klienti {k} u shkëput (timeout).");
    }
}
    
            string stats = GjeneroStatistikat();
            string text = $"{DateTime.Now}\n{stats}\n";
            File.AppendAllText("Logs/server_stats.txt", text);
        }
    }

static void RuajLogMesazh(string client, string msg)
{
    string text = $"{DateTime.Now} ({client}) {msg}\n";
    File.AppendAllText("Logs/server_messages.txt", text);
}

static void RuajLogStats(string stats)
{
     string text = $"{DateTime.Now}\n{stats}\n";
     File.AppendAllText("Logs/server_stats.txt", text);
}

static string GjeneroStatistikat()
{
    StringBuilder sb = new StringBuilder();
    long totalIn = 0;
    long totalOut = 0;

      lock (locker)
        {
            sb.AppendLine("STATISTIKAT E SERVERIT");
            sb.AppendLine("-------------------------------");
            sb.AppendLine($"Klientë aktivë: {klientet.Count}");

    foreach (var kl in klientet)
    {
        totalIn += kl.Value.BytesIn;
        totalOut += kl.Value.BytesOut;

        sb.AppendLine(
             $"{kl.Key} | mesazhe: {kl.Value.Messages} | bytes in: {kl.Value.BytesIn} | bytes out: {kl.Value.BytesOut} | admin: {kl.Value.IsAdmin}"
        );
    }
}

    sb.AppendLine($"Trafiku total: {totalln + totalOut} bytes");

    return sb.ToString();
  }
}
