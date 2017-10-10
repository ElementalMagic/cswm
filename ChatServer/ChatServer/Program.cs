using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ChatServer
{
    public class ServerObject
    {
        static TcpListener tcpListener;
        List<ClientObject> clients = new List<ClientObject>();
        private int _port;
        public ServerObject(int port)
        {
            this._port = port;
        }
        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }
        protected internal void RemoveConnection(string id)
        {
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
                clients.Remove(client);
        }

        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, _port);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }
        public List<ClientObject> clientsOnline
        {
            get { return clients; }
        }

        protected internal void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id)
                {
                    clients[i].Stream.Write(data, 0, data.Length);
                }
            }
        }
        protected internal void SendListOfClients(List<ClientObject> clOnLine)
        {
            string message;
            for (int i = 0; i < clients.Count; i++)
            {
                message = "/Clients:";
                bool _first = true;
                for (int y = 0; y < clients.Count; y++)
                {
                    if (i != y)
                    {
                        if (_first)
                        {
                            message += clOnLine[y].Id+","+clOnLine[y].userName;
                            _first = false;
                        }
                        else { message += ";"+ clOnLine[y].Id+"," + clOnLine[y].userName; }
                    }
                }
                byte[] data = Encoding.Unicode.GetBytes(message);
                clients[i].Stream.Write(data, 0, data.Length);
            }
        }
        internal void SendPrivateMessage(string message, string _ReceiverId, string _UserName, string _SenderId)
        {
            foreach(ClientObject _client in clients)
            {
                if (_client.Id == _ReceiverId)
                {
                    message = string.Format("/id:{2},{0}: {1}",_UserName,message,_SenderId);
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    _client.Stream.Write(data,0,data.Length);
                }
            }
        }
        protected internal void Disconnect()
        {
            tcpListener.Stop(); 

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); 
            }
            Environment.Exit(0);
        }
    }
    public class ClientObject
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        public string userName;
        TcpClient client;
        ServerObject server; 

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
               
                string message = GetMessage();
                userName = message;
                Thread myThr = new Thread(SendMess);
                myThr.Start();
                message = userName + " вошел в чат";
                server.SendListOfClients(server.clientsOnline);                                
                server.BroadcastMessage(message, this.Id);
                Console.WriteLine(message);
               
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                       if (message != "")
                        {
                            if (Regex.IsMatch(message, "/id:"))
                            {
                                message = Regex.Replace(message, "/id:", "");
                                string ID = message.Split(',')[0];
                                message = Regex.Replace(message, ID + ",", "");
                                server.SendPrivateMessage(message,ID,userName, Id);
                                Console.WriteLine(message);
                            }
                            else {
                                message = String.Format("{0}: {1}", userName, message);
                                Console.WriteLine(message);
                                server.BroadcastMessage(message, this.Id);
                            }
                        }
                        if (message == "")
                        {
                            message = String.Format("{0}: покинул чат", userName);
                            Console.WriteLine(message);
                            server.BroadcastMessage(message, this.Id);
                            server.RemoveConnection(Id);
                            server.SendListOfClients(server.clientsOnline);
                            break;
                        }
                    }
                    catch
                    {                       
                        message = String.Format("{0}: покинул чат", userName);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                        server.RemoveConnection(Id);
                        server.SendListOfClients(server.clientsOnline);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {                
                server.RemoveConnection(this.Id);
                server.SendListOfClients(server.clientsOnline);
                Close();
            }
        }
        private void SendMess()
        {
            while (true)
            {
                server.BroadcastMessage("Администратор: "+Console.ReadLine(), 999.ToString());
            }
        }
        
        private string GetMessage()
        {
            byte[] data = new byte[64]; 
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);
            return builder.ToString();
        }

        
        protected internal void Close()
        {
            
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }        
    }
    class Program
    {
        static ServerObject server; 
        static Thread listenThread; 
        static int _hostPort;
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Введите номер порта, который вы хотите использовать :");
                _hostPort = Int32.Parse(Console.ReadLine());
                server = new ServerObject(_hostPort);
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
