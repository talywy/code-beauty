using System;
using System.Threading;
using NetMQ;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TVGenius.Model;
using TVGenius.Utils;

namespace TVGenius.TVScanner
{
    public class TVScanner
    {
        public event EventHandler<TVFoundEventArgs> TVFound;
        public event EventHandler<TVHeartBeatEventArgs> TVHeartBeat;

        private const int HEART_BEAT_INTERVAL = 3000;

        private bool _isRunning;

        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                Scan(); 
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
            }
        }

        private void Scan()
        {
            var binds = SignalMap.GetAllBinds();

            foreach (var bind in binds)
                {
                    var t = new Thread(bindStr =>
                    {
                           using (NetMQContext ctx = NetMQContext.Create())
                            {
                                using (var client = ctx.CreateRequestSocket())
                                {
                                    client.Connect(bindStr.ToString());
                                    LogUtil.Log.DebugFormat("Try connect tv:{0}", bindStr);
                                    var scanMsg = CreateSingleMessage(SignalDefine.SCAN);
                                    client.Send(scanMsg);
                                    var message = client.ReceiveString();
                                    LogUtil.Log.InfoFormat(message);
                                    var mockTV = GetMockTV(message);
                                    if (mockTV != null)
                                    {
                                        TVFound(this, new TVFoundEventArgs(mockTV));
                                    }

                                    while (_isRunning)
                                    {
                                        var heartbeatMsg = CreateSingleMessage(SignalDefine.HEATBEAT);
                                        client.Send(heartbeatMsg);
                                        var repMsg = client.ReceiveString();
                                        var repMsgJson = JsonConvert.DeserializeObject<JObject>(repMsg);
                                        var signal = repMsgJson["signal"].Value<string>();
                                        if (signal.Equals(SignalDefine.ECHO))
                                        {
                                            TVHeartBeat(this, new TVHeartBeatEventArgs(repMsgJson["sn"].Value<string>()));
                                        }
                                
                                        Thread.Sleep(HEART_BEAT_INTERVAL);
                                    }
                                }
                            }
                    }) {IsBackground = true};
                    t.Start(bind);
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
    }
}
