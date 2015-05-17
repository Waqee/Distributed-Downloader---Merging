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
    class FileAggregationService
    {
        private Thread lnsThread = null;
        private TcpListener listener = null;
        private List<KeyValuePair<int, Tuple<int, int>>> partsAvailable= null;

        public FileAggregationService(List<KeyValuePair<int, Tuple<int, int>>> list)
        {  
            partsAvailable = list;
        }

        public void Start(int port)
        {            
            if(lnsThread == null && listener == null)
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                lnsThread = new Thread(new ThreadStart(ListenForFilePartRequests));
                lnsThread.Start();
            }
        }

        public void Stop()
        {
            if (lnsThread != null && listener != null)
            {
                listener.Stop();
                lnsThread.Abort();
            }
        }
        
        private void ListenForFilePartRequests()
        {
            Byte[] rEnd = Encoding.ASCII.GetBytes("Done!");

            while(true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream ns = client.GetStream();
                Byte[] data = new Byte[100];
                ns.Read(data, 0, data.Length);
                int fileID = Convert.ToInt32(data);

                foreach(KeyValuePair<int, Tuple<int, int>> part in partsAvailable)
                {
                    if(part.Key == fileID)
                    {                        
                        String reply = part.Value.Item1.ToString() + " " + part.Value.Item2.ToString();
                        Byte[] rStartEndBytes = Encoding.ASCII.GetBytes(reply);
                        ns.Write(rStartEndBytes, 0, rStartEndBytes.Length);
                        ns.Read(data, 0, data.Length);

                        if (data.ToString() == "SendIt")
                        {
                            String fileName = "temp\\" + part.Key.ToString() + " " + reply;
                            client.Client.SendFile(fileName);
                        }
                    }                        
                }

                ns.Write(rEnd, 0, rEnd.Length);
            }
        }

        public ~FileAggregationService()
        {
            Stop();
        }
    }
}
