using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;

namespace Client_V_2
{
    class Pro_File
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
        start:
            try
            {
               
                string host = "127.0.0.1";
                int port = 25565;
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
                                else if (command_for_Server =="users") 
                                {
                                    Write(stream, "users");
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
            catch (Exception e) { goto start; }
        }
    }
}