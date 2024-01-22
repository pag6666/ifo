using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace S2
{
    internal class Program
    {
        static List<User> logs = new List<User>();
        static string Read(User user) 
        {
            string requst = "";
            try
            {
                if (user.GetStream().CanRead)
                {
                    byte[] buffer = new byte[12024];
                    int size = user.GetStream().Read(buffer, 0, buffer.Length);
                    requst = Encoding.UTF8.GetString(buffer, 0, size);
                }
            }
            catch (Exception e)
            {
                logs.Remove(user);
                Console.WriteLine(e.Message);
                Console.WriteLine("Read: client close");


            }
            return requst;
        }
        static void Write(User user, string text)
        {
            try
            {
                if (user.GetStream().CanWrite)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(text);
                    user.GetStream().Write(buffer, 0, buffer.Length);
                    user.GetStream().Flush();
                }
            }
            catch (Exception e)
            {
                logs.Remove(user);
                Console.WriteLine(e.Message);
                Console.WriteLine("Write: client close");
            }

        }
        static void Client(object obj) 
        {
            try
            {
                using (TcpClient client = (TcpClient)obj)
                {
                    using (Stream stream = client.GetStream())
                    {
                        User user = new User("", client, stream);
                        string userName = Read(user);
                        Write(user, "Hello user " + userName);
                        user.SetName(userName);
                        if (userName.Equals("admin_0000"))
                        {
                            while (true)
                            {
                                string usernamefind = Read(user);
                                foreach (var i in logs)
                                {
                                    if (i.GetName() == usernamefind)
                                    {
                                        Write(user, "found");
                                        User userFind = i;
                                        Stream strearm2 = user.GetStream();
                                        while (true)
                                        {
                                            Write(userFind, Read(user));
                                            string temp = Read(userFind);
                                            Write(user, temp);

                                        }
                                    }
                                }
                                if (usernamefind.Equals("users"))
                                {
                                    string temp = "";
                                    for (int i = 0; i < logs.Count; i++)
                                    {
                                        temp += " username: " + logs[i].GetName();
                                    }
                                    Write(user, temp);
                                }
                               
                                    Write(user, "not found");
                                
                               
                                

                            }
                        }
                        else
                        {
                            logs.Add(user);
                            while (true)
                            {

                            }
                        }
                    }
                }
            }
            
            catch (Exception e)
            {
                
            }
            
        }
        static TcpListener server;
        static void Wait(TcpListener server) 
        {
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                new Thread(Client).Start(client);
                Console.WriteLine("connected");
            }
        }
        static void Start() 
        {
            Console.WriteLine("Server start");
       
            try
            {
                server= new TcpListener(System.Net.IPAddress.Any, 25565);
                server.Start();
                Wait(server);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
        }
        static void Main(string[] args)
        {
            Start();
        }
    }
}
