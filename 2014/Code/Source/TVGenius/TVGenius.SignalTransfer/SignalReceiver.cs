using System;
using System.Threading;
using NetMQ;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TVGenius.Model;
using TVGenius.SignalTransfer.Events;
using TVGenius.Utils;

namespace TVGenius.SignalTransfer
{
    public class SignalReceiver
    {
        public event EventHandler<ActionReceivedEventArgs> ActionReceived;

        private bool _isRunning;
        private readonly MockTV _tv;

        public SignalReceiver(MockTV tv)
        {
            _tv = tv;
        }

        public void Run()
        {
            _isRunning = true;

            var t = new Thread(() =>
            {
                using (NetMQContext ctx = NetMQContext.Create())
                {
                    using (var server = ctx.CreateResponseSocket())
                    {
                        var bindStr = _tv.Bind;
                        LogUtil.Log.InfoFormat("MockTV listen single:{0}", bindStr);
                        server.Bind(bindStr);
                        while (_isRunning)
                        {
                            try
                            {
                                string message = server.ReceiveString();
                                LogUtil.Log.Debug("Got message:" + message);

                                var msgJson = JsonConvert.DeserializeObject<JObject>(message);
                                var signal = msgJson["signal"].Value<string>();
                                if (signal.Equals(SignalDefine.SCAN))
                                {
                                    server.Send(JsonConvert.SerializeObject(_tv));
                                }
                                else if (signal.Equals(SignalDefine.HEATBEAT))
                                {
                                    var msg = GetResponseMessage(SignalDefine.ECHO);
                                    server.Send(msg);
                                }
                                else if (signal.Equals(SignalDefine.ACTION))
                                {
                                    var msg = GetResponseMessage(SignalDefine.OK);
                                    server.Send(msg);
                                    var data = msgJson["data"].Value<JObject>();
                                    ActionReceived(this, new ActionReceivedEventArgs(data));
                                }
                            }
                            catch (Exception ex)
                            {
                                LogUtil.Log.Error("Receive signal excepion", ex);
                            }

                            Thread.Sleep(100);
                        }
                    }
                }

            }) {IsBackground = true};
            t.Start();
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public string GetResponseMessage(string signal)
        {
            return string.Format("{{ signal:\"{0}\", sn:\"{1}\", state:\"{2}\" }}", signal, _tv.Sn, _tv.State);
        }
    }
}
