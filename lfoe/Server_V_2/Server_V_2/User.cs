using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Sockets;


namespace Server_V_2
{
    class User
    {
        private int _id;
        private string _name;
        private TcpClient _client;
        public User()
        {

        }
        public User(int id, string name, TcpClient client)
        {
            this._id = id;
            this._name = name;
            this._client = client;
        }
        public int GetId()
        {
            return _id;
        }
        public string GetName()
        {
            return _name;
        }
        public TcpClient GetTcpClient()
        {
            return _client;
        }
    }
}
