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
    class LANExposureService
    {
        private Thread dThread = null;
        private UdpClient listener = null;

        //The default constructor.
        public LANExposureService() { }

        //Start the listening service.
        public void Start(int port)
        {
            if(listener == null && dThread == null)
            {
                listener = new UdpClient(port);
                dThread = new Thread(new ThreadStart(ListenForDiscoveryRequests));
                dThread.Start();
            }
        }

        //Stop the listening service.
        public void Stop()
        {
            if (listener != null && dThread != null)
            {
                dThread.Abort();
                listener.Close();
            }
        }

        // This method gets started in a new thread when 
        // StartListeningService is called. It can't be
        //called on it's own from outside the class.*/
        private void ListenForDiscoveryRequests()
        {
            Byte[] responseData = Encoding.ASCII.GetBytes("DDHere!");

            while (true)
            {
                IPEndPoint otherDDClient = new IPEndPoint(IPAddress.Any, 0);
                Byte[] requestData = listener.Receive(ref otherDDClient);
                String request = Encoding.ASCII.GetString(requestData);

                if (request == "DDThere?")
                    listener.Send(responseData, responseData.Length, otherDDClient);
            }
        }

        //If the reference to the object is lost
        public ~LANExposureService()
        {
            Stop();
        }
    }
}
