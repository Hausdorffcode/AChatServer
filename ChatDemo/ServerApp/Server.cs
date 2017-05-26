﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerApp
{
    class ClientInfo
    {
        public string name;
        public string channel = "";
        public ClientInfo(string name)
        {
            this.name = name;
        }
    }

    public class Server
    {
        //the structure store the info of the client
        private Dictionary<Socket, ClientInfo> socketToClient = new Dictionary<Socket, ClientInfo>();
        private List<string> names = new List<string>();
        private Dictionary<string, List<Socket>> channelToSocket = new Dictionary<string, List<Socket>>();

        private const int BufferSize = 1024;
        private byte[] data;
        private Socket server;
        private IPEndPoint ipep;


        public Server()
        {
            //get the IPEndPoint
            //多个线程并发访问此数据可能出错
            IPAddress ipAddress = MyNetworkLibrary.AddressHelper.GetLocalhostIPv4Addresses().First();
            int endpoint = MyNetworkLibrary.AddressHelper.GetOneAvailablePortInLocalhost();
            ipep = new IPEndPoint(ipAddress, endpoint);

            data = new byte[BufferSize];
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(ipep);
        }

        public void run()
        {
            server.Listen(10);
            Console.WriteLine("Server {0} is listening port {1}...", ipep.Address, ipep.Port);
            using (server)
            {
                while (true)
                {
                    try
                    {
                        Socket connectionSocket = server.Accept();
                        //IPEndPoint cipep = (IPEndPoint)connectionSocket.RemoteEndPoint;
                        //Console.WriteLine("ip: {0}, port; {1} has connected.", cipep.Address, cipep.Port);
                        Thread th = new Thread(HandlerThreadMethod);
                        th.IsBackground = true;
                        th.Start(connectionSocket);
                    }
                    catch (ThreadAbortException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        private void HandlerThreadMethod(object o)
        {
            Socket connectSocket = o as Socket;
            byte[] recvData = MyNetworkLibrary.SocketHelper.ReceiveVarData(connectSocket);
            string name = Encoding.UTF8.GetString(recvData);
            if (names.Contains(name))
            {
                //todo
            }
            else
            {
                socketToClient.Add(connectSocket, new ClientInfo(name));
            }
            MyNetworkLibrary.SocketHelper.SendVarData(connectSocket, Encoding.UTF8.GetBytes(String.Format("Welcome {0}!", name)));
            while (true)
            {
                recvData = MyNetworkLibrary.SocketHelper.ReceiveVarData(connectSocket);
                string msg = Encoding.UTF8.GetString(recvData);
                if (msg == "" || msg[0] != '/')
                {
                    Thread broadcastTh = new Thread(broadcast);
                    broadcastTh.Start(new BroadcastInfo(connectSocket, socketToClient[connectSocket].channel, msg));
                }
                else
                {
                    if (msg.Substring())
                }
                
            }
        }

        private string list()
        {
            string str = "";
            foreach (var item in channelToSocket.Keys)
            {
                str += item + Environment.NewLine;
            }
            return str;
        }

        private void join(Socket s, string channel)
        {
            string oldChannel = socketToClient[s].channel;
            socketToClient[s].channel = channel;
            channelToSocket[oldChannel].Remove(s);
            channelToSocket[channel].Add(s);
        }

        private void create(Socket s, string channel)
        {
            channelToSocket.Add(channel, new List<Socket>() { s });
            socketToClient[s].channel = channel;
        }

        private void broadcast(object o)
        {
            BroadcastInfo bInfo = o as BroadcastInfo;
            Socket s = bInfo.s;
            string channel = bInfo.channel;
            string msg = bInfo.msg;


        }

        /// <summary>
        /// The param of broadcast
        /// </summary>
        class BroadcastInfo
        {
            public Socket s;
            public string channel;
            public string msg;
            public BroadcastInfo(Socket s, string channel, string msg)
            {
                this.s = s;
                this.channel = channel;
                this.msg = msg;
            }
        }
    }

}