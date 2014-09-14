using System.Collections.Generic;
using NetMQ;
using NetMQ.Sockets;

namespace TVGenius.SignalTransfer
{
    public class ZMQClientPool
    {
        private readonly Dictionary<string, RequestSocket> _clientPool = new Dictionary<string, RequestSocket>();
        private static ZMQClientPool _instance;
        private static readonly object Locker = new object();

        private readonly NetMQContext _ctx;

        private ZMQClientPool()
        {
            _ctx = NetMQContext.Create();
        }

        public static ZMQClientPool Instance
        {
            get
            {
                lock (Locker)
                {
                    if (_instance == null)
                    {
                        _instance = new ZMQClientPool();
                    }
                }

                return _instance;
            }
        }

        public RequestSocket GetClient(string bindStr)
        {
            if (!_clientPool.ContainsKey(bindStr))
            {
                var client = _ctx.CreateRequestSocket();
                client.Connect(bindStr);
                _clientPool.Add(bindStr, client);
            }

            return _clientPool[bindStr];
        }
    }
}
