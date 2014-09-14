using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TVGenius.Model;
using TVGenius.SignalTransfer;
using TVGenius.SignalTransfer.Events;
using TVGenius.Utils;

namespace TVGenius.TVScanner
{
    /// <summary>
    /// 电视扫描器
    /// </summary>
    public class TVScanner
    {
        public event EventHandler<TVFoundEventArgs> TVFound;
        public event EventHandler<TVHeartBeatEventArgs> TVHeartBeat;

        
        private readonly List<Thread> _threads = new List<Thread>();
        private readonly List<NetClient> _clients = new List<NetClient>();

        private const int HEART_BEAT_INTERVAL = 3000;

        private bool _isRunning;

        /// <summary>
        /// 
        /// </summary>
        private void Scan()
        {
            var binds = SignalMap.GetAllBinds();

            foreach (var bind in binds)
            {
                var client = NetClientPool.Instance.GetClient(bind);
                if (!_clients.Contains(client))
                {
                    _clients.Add(client);
                    client.MessageReceived += ClientOnMessageReceived; 
                }
               
                var scanMsg = CreateSingleMessage(SignalDefine.SCAN);
                client.SendMessage(scanMsg);

                var t = new Thread(() =>
                {
                    while (_isRunning)
                    {
                        var heartbeatMsg = CreateSingleMessage(SignalDefine.HEATBEAT);
                        client.SendMessage(heartbeatMsg);
                        Thread.Sleep(HEART_BEAT_INTERVAL);
                    }
                }) {IsBackground = true};
                t.Start();
                    
                _threads.Add(t);
            }
        }

        private void ClientOnMessageReceived(object sender, NetClinetMessageEventArgs e)
        {
            LogUtil.Log.DebugFormat("ClientOnMessageReceived:{0}", e.Message);

            var repMsgJson = JsonConvert.DeserializeObject<JObject>(e.Message);

            if (repMsgJson["signal"] != null)
            {
                var signal = repMsgJson["signal"].Value<string>();
                var stateStr = repMsgJson["state"].Value<string>();
                TVState state;
                var success = Enum.TryParse(stateStr, out state);
                if (!success)
                {
                    LogUtil.Log.ErrorFormat("Parse tv state error:{0}", stateStr);
                }
                if (signal.Equals(SignalDefine.ECHO))
                {
                    TVHeartBeat(this, new TVHeartBeatEventArgs(repMsgJson["sn"].Value<string>(), state));
                }
            }
            else
            {
                var mockTV = GetMockTV(e.Message);
                if (mockTV != null)
                {
                    TVFound(this, new TVFoundEventArgs(mockTV));
                } 
            }
        }

        private MockTV GetMockTV(string movkTVJson)
        {
            MockTV tv = null;
            try
            {
                tv = JsonConvert.DeserializeObject<MockTV>(movkTVJson);
            }
            catch (Exception ex)
            {
                LogUtil.Log.Error("GetMockTV exception", ex);
            }
            return tv;
        }

        private string CreateSingleMessage(string signal, string data = "")
        {
            return string.Format("{{signal:\"{0}\", data:\"{1}\"}}", signal, data);
        }

        /// <summary>
        /// 开始扫描
        /// </summary>
        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                Scan();
            }
        }

        /// <summary>
        /// 停止扫描
        /// </summary>
        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh()
        {
            new Thread(() =>
            {
                foreach (NetClient client in _clients)
                {
                    var scanMsg = CreateSingleMessage(SignalDefine.SCAN);
                    client.SendMessage(scanMsg);
                }
            }){IsBackground = true}.Start();
        }
    }
}
