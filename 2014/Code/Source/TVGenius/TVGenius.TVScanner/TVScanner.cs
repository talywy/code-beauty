using System;
using System.Collections.Generic;
using System.Threading;
using NetMQ;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TVGenius.Model;
using TVGenius.SignalTransfer;
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
                var t = new Thread(bindStr =>
                {
                    var client = ZMQClientPool.Instance.GetClient(bindStr.ToString());
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
                }) { IsBackground = true };
                _threads.Add(t);
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
            try
            {
                Stop();
                foreach (Thread thread in _threads)
                {
                    if (thread.IsAlive)
                    {
                        thread.Abort();
                    }
                }
                _threads.Clear();
                Start();
            }
            catch (Exception ex)
            {
                LogUtil.Log.Error("Refresh tv list exception", ex);
            }
        }
    }
}
