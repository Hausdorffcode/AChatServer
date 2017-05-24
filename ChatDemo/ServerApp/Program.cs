using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    class Program
    {
        private const int BufferSize = 1024;

        static void Main(string[] args)
        {
            IPAddress ipAddress = MyNetworkLibrary.AddressHelper.GetLocalhostIPv4Addresses().First();
            int endpoint = MyNetworkLibrary.AddressHelper.GetOneAvailablePortInLocalhost();
            IPEndPoint ipep = new IPEndPoint(ipAddress, endpoint);

            byte[] data = new byte[BufferSize];
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            using (server)
            {
                server.Bind(ipep);
                server.Listen(10);
                
                while (true)
                {
                    Console.WriteLine("主机 {0} 正在监听端口 {1}...", ipAddress, endpoint);
                    Socket client = server.Accept();
                    IPEndPoint cipep = (IPEndPoint)client.RemoteEndPoint;
                    Console.WriteLine("已经接受来自ip为{0}，端口为{1}的客户连接。", cipep.Address, cipep.Port);
                    client.Send(Encoding.UTF8.GetBytes("欢迎使用本服务，输入“exit”断开连接。"));
                    while (true)
                    {
                        int recv = client.Receive(data);
                        string response = Encoding.UTF8.GetString(data, 0, recv);
                        if (response != "exit")
                        {
                            Console.WriteLine(String.Format("客户 {0} ：", cipep.Address) + response);
                            client.Send(Encoding.UTF8.GetBytes("服务器回答：" + response + " too."));
                        }
                        else
                        {
                            Console.WriteLine("客户 {0} 已断开。", cipep.Address);
                            client.Shutdown(SocketShutdown.Both);
                            client.Close();
                            break;
                        }
                    }
                }
            }
        }
    }
}
