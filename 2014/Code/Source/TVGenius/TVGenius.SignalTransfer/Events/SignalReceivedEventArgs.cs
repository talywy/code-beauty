using System;

namespace TVGenius.SignalTransfer.Events
{
    /// <summary>
    /// 信号接收事件参数
    /// </summary>
    public class SignalReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 信号
        /// </summary>
        public string Signal
        {
            get; set;
        }

        /// <summary>
        /// 信号事件参数
        /// </summary>
        /// <param name="signal">信号</param>
        public SignalReceivedEventArgs(string signal)
        {
            Signal = signal;
        }
    }
}
