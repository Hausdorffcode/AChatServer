using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
        private const int BufferSize = 1024;

        static void Main(string[] args)
        {
            byte[] data = new byte[BufferSize];
            IPEndPoint iped = null;

            while (true)
            {
                iped = MyNetworkLibrary.AddressHelper.GetRemoteMachineIPEndPoint();
                if (iped != null)
                    break;
            }

            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                client.Connect(iped);
                while (true)
                {
                    int recv = client.Receive(data);
                    Console.WriteLine(Encoding.UTF8.GetString(data, 0, recv));
                    string msg = Console.ReadLine();
                    client.Send(Encoding.UTF8.GetBytes(msg));
                    if (msg == "exit")
                    {
                        client.Shutdown(SocketShutdown.Both);
                        break;
                    }
                }
            }
            catch(SocketException e)
            {
                Console.WriteLine("无法连接到主机：{0}，原因：{1}\nNativeErrorCode: \nSocketErrorCode: ", iped.Address, e.Message, e.NativeErrorCode, e.SocketErrorCode);
            }
            finally
            {
                client.Close();
            }
            Console.WriteLine("消息发送完毕，断开与服务端的连接。");
            Console.WriteLine("敲任意键退出...");
            Console.ReadKey();
        }
        
    }
}
