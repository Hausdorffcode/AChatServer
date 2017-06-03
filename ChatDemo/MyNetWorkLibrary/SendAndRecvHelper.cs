using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyNetWorkLibrary
{
    public class SendAndRecvHelper
    {
        /// <summary>
        /// 接受可变长数据，规定开头4个字节注明数据长度
        /// </summary>
        public static byte[] RecvVarData(Socket s)
        {
            byte[] dataSize = new byte[4];
            byte[] data;
            try
            {
                s.Receive(dataSize, 0, 4, SocketFlags.None);
                int len = BitConverter.ToInt32(dataSize, 0);

                data = new byte[len];
                int dataleft = len;
                int totalRecv = 0;
                int recv = 0;
                while (dataleft > 0)
                {
                    recv = s.Receive(data, totalRecv, dataleft, SocketFlags.None);
                    if (recv == 0)
                    {//when recv == 0, it means the connection have been closed
                        break;
                    }
                    dataleft -= recv;
                    totalRecv += recv;
                }
            }
            catch(SocketException e)
            {
                throw e;
            }
            return data;
        }

        /// <summary>
        /// 发送可变长数据，规定开头4个字节注明数据长度
        /// </summary>
        public static int SendVarData(Socket s, byte[] data)
        {
            int len = data.Length; //the length of data
            int totalsent = 0;  //the data have been sent
            int sent = 0;       //the length of every time the data be sent
            int dataleft = len; //the data to be sent

            try
            {
                sent = s.Send(BitConverter.GetBytes(len)); //send the length of data first

                while (dataleft > 0)
                {
                    sent = s.Send(data, totalsent, dataleft, SocketFlags.None);
                    totalsent += sent;
                    dataleft -= sent;
                }
            }
            catch (SocketException e)
            {
                throw e;
            }
            
            return totalsent;
        }
    }
}
