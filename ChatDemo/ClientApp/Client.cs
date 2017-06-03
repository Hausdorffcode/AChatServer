using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp
{
    public class Client
    {

        IPEndPoint iped = null;
        Socket client;

        public void run()
        {
            while (true)
            {
                iped = MyNetWorkLibrary.AddressHelper.GetRemoteMachineIPEndPoint();
                if (iped != null)
                    break;
            }

            //Console.Write("Please enter your name to login : ");
            //string name = Console.ReadLine();


            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                client.Connect(iped);
                Console.Write("Please enter your name to login in: ");
                string name = Console.ReadLine();
                MyNetWorkLibrary.SendAndRecvHelper.SendVarData(client, Encoding.UTF8.GetBytes(name));
                Console.Write(ClientMessage.CLIENT_MESSAGE_PREFIX);

                Thread th = new Thread(RecvHelperMethod);
                th.IsBackground = true;
                th.Start();

                while (true)
                {
                    string msg = Console.ReadLine();
                    try
                    {
                        MyNetWorkLibrary.SendAndRecvHelper.SendVarData(client, Encoding.UTF8.GetBytes(msg));
                    }
                    catch(SocketException e)
                    {
                        Console.WriteLine(String.Format(ClientMessage.CLIENT_SERVER_DISCONNECTED, iped.Address, iped.Port));
                        break;
                    }
                    Console.Write(ClientMessage.CLIENT_MESSAGE_PREFIX);
                }

            }
            catch (SocketException e)
            {
                Console.WriteLine(String.Format(ClientMessage.CLIENT_CANNOT_CONNECT, iped.Address, iped.Port));
                //Console.WriteLine("无法连接到主机：{0}，原因：{1}\nNativeErrorCode: \nSocketErrorCode: ", iped.Address, e.Message, e.NativeErrorCode, e.SocketErrorCode);
            }
            finally
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            Console.WriteLine("敲任意键退出...");
            Console.ReadKey();
        }

        private void RecvHelperMethod()
        {
            while (true)
            {
                try
                {
                    byte[] data = MyNetWorkLibrary.SendAndRecvHelper.RecvVarData(client);
                    string msg = ClientMessage.CLIENT_WIPE_ME + "\r" + Encoding.UTF8.GetString(data);

                    Console.WriteLine(msg);
                }
                catch (SocketException e)
                {
                    Console.WriteLine(String.Format(ClientMessage.CLIENT_SERVER_DISCONNECTED, iped.Address, iped.Port));
                    break;
                }
                Console.Write(ClientMessage.CLIENT_MESSAGE_PREFIX);
            }
        }
    }
}
