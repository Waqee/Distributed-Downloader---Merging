using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class TCPConnection
    {
        private TcpClient client = null;
        private NetworkStream ns = null;

        public TCPConnection(String ip, int port)
        {
            client = new TcpClient(ip, port);
            ns = client.GetStream();
        }

        //returs the number of bytes read
        public int Read(Byte[] data)
        {
            return ns.Read(data, 0, data.Length);
        }

        public void Write(Byte[] data)
        {
            ns.Write(data, 0, data.Length);
        }

        public ~TCPConnection()
        {
            if (client != null && ns != null)
            {
                ns.Close();
                client.Close();
            }
        }
    }
}
