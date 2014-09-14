using System;
using Newtonsoft.Json.Linq;

namespace TVGenius.SignalTransfer.Events
{
    /// <summary>
    /// 信号接收事件参数
    /// </summary>
    public class ActionReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 信号
        /// </summary>
        public JObject Data
        {
            get; set;
        }

        /// <summary>
        /// 信号事件参数
        /// </summary>
        /// <param name="data">数据</param>
        public ActionReceivedEventArgs(JObject data)
        {
            Data = data;
        }
    }
}
