using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace S2
{
    internal class User
    {
        private string name;
        private TcpClient client;
        private Stream stream;
        public User(string name, TcpClient client, Stream stream) 
        {
            this.name = name;
            this.client = client;
            this.stream = stream;
        }
        public TcpClient GetTcpClient() => client;
        public Stream GetStream() => stream;
        public string GetName() => name;
        public void SetName(string name) => this.name = name;
    }
}
