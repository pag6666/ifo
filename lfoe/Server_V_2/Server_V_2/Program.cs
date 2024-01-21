using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
namespace Server_V_2
{
    class Server : IDisposable
    {
        private List<User> users = new List<User>();
        private TcpListener server;
        public int MaxReadByte = 2024;
        private void WriteUser(Stream stream, string text, User user)
        {
            try
            {
                if (stream.CanWrite)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(text);
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                }
            }
            catch
            {
                Console.WriteLine("Write: client close");
                if (users.Count > 0)
                {
                    users.Remove(user);
                }
                Wait(true);
            }

        }
        private void Write(Stream stream, string text)
        {
            try
            {
                if (stream.CanWrite)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(text);
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Write: client close");
                if (id_incre > 0)
                    id_incre = id_incre - 1;
                Wait(true);
            }

        }
        private string ReadUser(Stream stream, User user)
        {

            string requst = "not connect";
            try
            {
                if (stream.CanRead)
                {
                    byte[] buffer = new byte[MaxReadByte];
                    int size = stream.Read(buffer, 0, buffer.Length);
                    requst = Encoding.UTF8.GetString(buffer, 0, size);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Read: client close");
                if (users.Count > 0)
                {
                    users.Remove(user);
                }
                Wait(true);
            }
            return requst;

        }
        private string Read(Stream stream)
        {

            string requst = "not connect";
            try
            {
                if (stream.CanRead)
                {
                    byte[] buffer = new byte[MaxReadByte];
                    int size = stream.Read(buffer, 0, buffer.Length);
                    requst = Encoding.UTF8.GetString(buffer, 0, size);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Read: client close");
                Wait(true);
            }
            return requst;

        }
        public Server(IPAddress address, int port)
        {
            
            server = new TcpListener(address, port);
            server.Start();
        }
        private string ReadWrite(Stream stream, string text, User user)
        {

            string requst = "null";
            Thread.Sleep(1000);
            requst = ReadUser(stream, user);
            WriteUser(stream, text, user);
            return requst;
        }
        private string WriteRead(Stream stream, string text, User user)
        {
            string requst = "null";
            WriteUser(stream, text, user);
            Thread.Sleep(1000);
            requst = ReadUser(stream, user);
            return requst;
        }

        //////////////////////////////////////////main
        private void Client_Work(object obj_client)
        {
            User user = (User)obj_client;
            using (TcpClient client = user.GetTcpClient())
            {
                using (Stream stream = client.GetStream())
                {
                    id_incre++;
                   
                    while (true)
                    {
                        Console.WriteLine("Enter the command");
                        string command = WriteRead(stream, Console.ReadLine(), user);
                        Console.WriteLine("answer = "+command);
                        if (command == "close")
                        {
                            if (users.Count > 0)
                            {
                                users.Remove(user);
                            }
                            //close
                            string answer = WriteRead(stream, "close", user);
                            Console.WriteLine(answer);
                            Thread.Sleep(1000);
                            //close
                            break;
                        }
                        else if (command == "telebot_connect")
                        {
                            string answer = Read(stream);
                            if (answer == "telebot_connected")
                            {
                                
                            }
                        }
                        else if (command == "users")
                        {
                            foreach (var i in users)
                            {
                                Console.WriteLine($"id = {i.GetId()} | username = {i.GetName()}");
                            }
                        }
                        else 
                        {
                            foreach (User i in users) 
                            {
                                if (i.GetName().Equals(command)) {
                                    Console.WriteLine("find");
                                }

                            }
                        }
                    }
                }
            }

        }
        private int id_incre = 0;
        ///////////////////////////////////////////main
        private void Client_telebot()
        {
            Thread.Sleep(2000);
            error:
            try
            {
                while (true)
                {
                    foreach (User i in users)
                    {
                        Stream stream = i.GetTcpClient().GetStream();
                        if (Read(stream) == "find_pc")
                        {
                            Write(stream, "ok");
                        }
                        else 
                        {
                            Write(stream, "_ERROR_ANSWER");
                        }
                    }

                }
            }
            catch 
            {
                goto error;   
            }
        }

        public void Wait(bool start)
        {
        connect:
            try
            {
                if (id_incre != 0)
                {
                    --id_incre;
                }

                while (start)
                {
                    Console.WriteLine("Wait connect");
                    TcpClient client = server.AcceptTcpClient();
                    string username = Read(client.GetStream());
                    Console.WriteLine("connect");

                    Write(client.GetStream(), username);
                    users.Add(new User(id_incre, username, client));
                    User us = new User();
                    if (users.Count > 0 || users.Count < id_incre)
                    {
                        us = users[id_incre];

                    }
                    new Thread(Client_Work).Start(us);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                goto connect;
            }
        }

        public void Dispose()
        {
            server.Stop();
        }
    }

    class Pro_File
    {
        static void Main()
        {
            using (Server server = new Server(IPAddress.Any, 25565))
            {
                server.Wait(true);
            }
        }
    }
}