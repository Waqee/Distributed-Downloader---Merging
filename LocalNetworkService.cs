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
    class LocalNetworkService
    {
        private LANExposureService expService = null;
        private FileAggregationService fasService = null;

        public LocalNetworkService()
        {

        }

        public int StartExposureService()
        {
            if(expService == null)
            {
                expService = new LANExposureService();
                expService.Start(12444);
                return 1;
            }

            return 0;            
        }

        public int StartExposureService(int port)
        {
            if (expService == null)
            {
                expService = new LANExposureService();
                expService.Start(port);
                return 1;
            }

            return 0;
        }

        public int StartFileAggregationService(List<KeyValuePair<int, Tuple<int, int>>> list)
        {
            if (fasService == null)
            {
                fasService = new FileAggregationService(list);
                fasService.Start(12445);
                return 1;
            }

            return 0;
        }

        public int StartFileAggregationService(List<KeyValuePair<int, Tuple<int, int>>> list, int port)
        {
            if (fasService == null)
            {
                fasService = new FileAggregationService(list);
                fasService.Start(port);
                return 1;
            }

            return 0;
        }
    }
}
