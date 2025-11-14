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
if (msg.StartsWith("/delete") && isAdmin)
{
    string file = msg.Split(' ', 2)[1].Trim();
    File.Delete(Path.Combine("server_files", file));
    Dergo(clientEP, "File u fshi.");
    continue;
}

if (msg.StartsWith("/upload") && isAdmin)
{
    string[] p = msg.Split(' ', 3);
    string file = p[1].Trim();
    string content = p[2];
    File.WriteAllText(Path.Combine("server_files", file), content);
    Dergo(clientEP, "File u ngarkua.");
    continue;
}

if (msg.StartsWith("/download"))
{
    string file = msg.Split(' ', 2)[1].Trim();
    string content = File.ReadAllText(Path.Combine("server_files", file));
    Dergo(clientEP, content);
    continue;
}

Dergo(clientEP, "Mesazhi u pranua.");
﻿
