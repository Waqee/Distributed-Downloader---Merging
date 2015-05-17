using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    // This class is used to find other Distributed 
    // Download clients on the local network. It has a single
    // static method which returns a list of IPEndPoint which
    // may be further used to create UDP/TCP connections to other
    // machines. You have to provide the port number on which
    // the LANDiscoveryService of other clients is running.
    class LANDiscovery
    {
        public static List<IPEndPoint> Discover()
        {
            UdpClient client = new UdpClient();
            List<IPEndPoint> otherDDClients = new List<IPEndPoint>();
            Byte[] requestData = Encoding.ASCII.GetBytes("DDThere?");

            client.EnableBroadcast = true;
            client.Client.ReceiveTimeout = 5000;
            client.Send(requestData, requestData.Length, new IPEndPoint(IPAddress.Broadcast, port));

            while (true)
            {
                IPEndPoint otherDDClient = new IPEndPoint(IPAddress.Any, 0);
                Byte[] responseData = null;

                try
                {
                    responseData = client.Receive(ref otherDDClient);
                }

                catch (SocketException)
                {
                    break;
                }

                String response = Encoding.ASCII.GetString(responseData);

                if (response == "DDHere!")
                    otherDDClients.Add(otherDDClient);
            }

            client.Close();

            return otherDDClients;
        }

        public static List<IPEndPoint> Discover(int port)
        {
            UdpClient client = new UdpClient();
            List<IPEndPoint> otherDDClients = new List<IPEndPoint>();
            Byte[] requestData = Encoding.ASCII.GetBytes("DDThere?");

            client.EnableBroadcast = true;
            client.Client.ReceiveTimeout = 5000;
            client.Send(requestData, requestData.Length, new IPEndPoint(IPAddress.Broadcast, port));

            while (true)
            {
                IPEndPoint otherDDClient = new IPEndPoint(IPAddress.Any, 0);
                Byte[] responseData = null;

                try
                {
                    responseData = client.Receive(ref otherDDClient);
                }

                catch (SocketException)
                {
                    break;
                }

                String response = Encoding.ASCII.GetString(responseData);

                if (response == "DDHere!")
                    otherDDClients.Add(otherDDClient);
            }

            client.Close();

            return otherDDClients;
        }
    }
}
