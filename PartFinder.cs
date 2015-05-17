using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class PartFinder
    {
        Byte[] downloadID;
        List<IPEndPoint> otherClients = null;
        Thread[] threads = null;
        TcpClient[] clients = null;

        public PartFinder(int downloadID, String fileName, List<IPEndPoint> otherClients) 
        {
            this.downloadID = Encoding.ASCII.GetBytes(downloadID.ToString());
            this.otherClients = otherClients;
            threads = new Thread[otherClients.Count];
            clients = new TcpClient[otherClients.Count];
        }

        public void GetParts(int port, List<IPEndPoint> otherClients)
        {
            for(int i = 0; i < otherClients.Count; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(HandleClient));
                clients[i] = new TcpClient(otherClients[i].Address.ToString(), port);
                threads[i].Start(clients[i]);
            }


        }

        private void HandleClient(Object temp)
        {
            int counter = 0;
            Byte[] data = new Byte[32 * 1024];
            Byte[] response = Encoding.ASCII.GetBytes("SendIt");
            TcpClient client = (TcpClient)temp;
            NetworkStream clientStream = client.GetStream();

            clientStream.Write(downloadID, 0, downloadID.Length);            
            clientStream.Read(data, 0, data.Length);
            String[] values = data.ToString().Split(' ');
            int startByte = Convert.ToInt32(values[0]);
            int endByte = Convert.ToInt32(values[1]);
            int size = endByte - startByte;

            while(data.ToString() != "Done")
            {
                int bytesRead = 0;
                while ((bytesRead = clientStream.Read(data, 0, data.Length)) > 0)
                {
                    counter += bytesRead;

                    if(counter < (size - bytesRead))
                    {

                    }
                }
            }
        }

            


    }
}
