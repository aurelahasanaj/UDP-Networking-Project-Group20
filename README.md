# UDP Networking Project – Group 20 
**Lënda:** Rrjetat Kompjuterike 
**Protokolli:** UDP
**Gjuha:** C#

 Përshkrimi i Projektit
Ky projekt implementon një **UDP Server** dhe një **UDP Client** të zhvilluar në C#, të cilët komunikojnë mes vete përmes protokollit UDP. Projekti është zhvilluar për lëndën **Rrjetat Kompjuterike**, duke demonstruar menaxhim real të klientëve, logim, operacione me file dhe kontroll qasjeje.

Serveri vepron si qendër e komunikimit, ndërsa klientët mund të jenë admin ose përdorues të thjeshtë. Projekti është testuar me disa klientë paralelisht dhe plotëson të gjitha kërkesat e detyrës.

Anëtarët e Grupit
```
• **Aurela Hasanaj** – Admin (Write/Read/Execute)
• **Delvina Elshani** – Klient (Read)
• **Arbias Bala** – Klient (Read)
• **Elion Maksutaj** – Klient (Read)
```
Funksionalitetet e Serverit
• Pranon deri në **10 klientë njëkohësisht**
• Klienti i parë bëhet automatikisht **ADMIN**
• Kontroll qasjeje për komanda specifike (admin vs user)
• Monitoron trafikun (bytes in/out)
• Regjistron çdo mesazh në **Logs/server_messages.txt**
• Regjistron statistika periodike në **Logs/server_stats.txt**
• Pasivon klientët që nuk reagojnë për 20 sekonda (timeout)
• Menaxhon file brenda folderit **server_files/**

Funksionalitetet e Klientit
• Lidhet me serverin duke futur IP (zakonisht 127.0.0.1)
• Pyet automatikisht nëse përdoruesi është admin
• Admin verifikohet me fjalëkalim
• Shfaq listën e komandave të lejuara për çdo rol
• Përdoruesit normalë kanë *delay random* për simulim real të rrjetit
• Merr përgjigje nga serveri në kohë reale

Komandat e Mbështetura
**Për të gjithë përdoruesit:**
/list – Liston file-t në server
/read <file> – Lexon përmbajtjen e një file
/search <keyword> – Kërkon tekst në të gjitha file-t
/info <file> – Informata për file (madhësia, data, modifikimi)
STATS – Statistikat e serverit
/exit – Mbyll klientin

**Vetëm për ADMIN:**
```
/upload <file> <content> – Krijon file të ri në server
/delete <file> – Fshin një file
/download <file> – Shkarkon përmbajtjen e file-t
🖥 Si Ekzekutohet Projekti?
1. Hap folderin **UDPServer** dhe starto Program.cs (CTRL + F5)
2. Serveri nis në portin **9000** dhe pret klientë
3️. Hap folderin **UDPClient** dhe starto Program.cs
4️. Shkruaj IP e serverit (p.sh. 127.0.0.1)
5️. Zgjidh rolin (admin/klient)
6️. Shkruaj komandat e dëshiruara
```
Struktura e Projektit
```
/UDPProjectSolution
├── UDPServer
│   ├── Program.cs
│   ├── server_files/
│   └── Logs/
├── UDPClient
│   └── Program.cs
└── README.md
```
Funksionalitetet e Përmbushura
• [x] Variablat IP & Port
• [x] Dëgjimi i klientëve
• [x] Refuzimi kur tejkalohen klientët max
• [x] Ruajtja e mesazheve në log
• [x] Timeout me fshirje automatike
• [x] Kontroll i nivelit të qasjes
• [x] File management i plotë
• [x] Statistika të detajuara të serverit
• [x] Logging i dyfishtë (mesazhe + statistika)
• [x] Testuar me disa klientë paralelisht

Përfundim
Projekti është funksional, stabil dhe përmbush të gjitha kërkesat e detyrës. Serveri menaxhon klientët në mënyrë efikase, logon çdo aktivitet dhe siguron komunikim të shpejtë përmes UDP. Ky projekt demonstron qartë konceptet bazike të rrjeteve kompjuterike, protokollit UDP, programimit paralel dhe menaxhimit të file-ve.
