﻿using NetMQ;
using NetMQ.Sockets;
using TVGenius.Model;
using TVGenius.Utils;

namespace TVGenius.SignalTransfer.Dispatcher
{
    /// <summary>
    /// 命令分发器基类
    /// </summary>
    abstract public class SingalDispatcherBase
    {
      
        private bool _sendReady;
        private RequestSocket _client;

        public void BuildClient(string bindStr)
        {
            _client = ZMQClientPool.Instance.GetClient(bindStr);
            _client.SendReady += ClientOnSendReady;
            LogUtil.Log.DebugFormat("Signal dispatcher connect tv:{0}", bindStr);
        }

        protected void SendMessage(string message)
        {
            LogUtil.Log.DebugFormat("Signal Dispatcher message:{0}", message);
            _client.Send(message);
            var rep = _client.ReceiveString();
            LogUtil.Log.DebugFormat("Signal Dispatcher response:{0}", rep);
        }

        private void ClientOnSendReady(object sender, NetMQSocketEventArgs netMqSocketEventArgs)
        {
            LogUtil.Log.DebugFormat("Signal dispatcher is ready");
            _sendReady = true;
        }

        /// <summary>
        /// 更换频道
        /// </summary>
        /// <param name="action"></param>
        /// <param name="num"></param>
        public void ChangeChannel(ChannelAction action, int num = 0)
        {
            var msg = BuildActionSignal(ActionType.CHANGE_CHANNEL, string.Format("{{action:{0}, num: {1}}}", (int)action, num) );
            SendMessage(msg);  
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="action">音量设置动作</param>
        public void ChangeVolume(VolumeAction action)
        {
            var msg = BuildActionSignal(action == VolumeAction.Increase? ActionType.VOLUME_INCREASE : ActionType.VOLUME_DECREASE);
            SendMessage(msg);
        }

        /// <summary>
        /// 设置电视机状态
        /// </summary>
        /// <param name="state"></param>
        public void SetState(TVState state)
        {
            var msg = BuildActionSignal(ActionType.HIBERNATE);
            SendMessage(msg);
        }

        /// <summary>
        /// 模拟遥控器按键
        /// </summary>
        /// <param name="keyCode">按键编码</param>
        public void SiumKeyPress(string keyCode)
        {
            // TODO: need implement
        }

        /// <summary>
        /// 模拟鼠标动作
        /// </summary>
        /// <param name="action">鼠标动作</param>
        public void SiumMouseAction(MouseAction action)
        {
            // TODO: need implement
        }

        /// <summary>
        /// 模拟键盘
        /// </summary>
        /// <param name="keyCode">键盘按键值</param>
        public void SiumKeyboard(string keyCode)
        {
            // TODO: need implement
        }

        protected string BuildActionSignal(string atcionType, string data = "")
        {
            return string.Format("{{signal:\"{0}\", data:{{type:\"{1}\", data:{2}}}}}", SignalDefine.ACTION, atcionType, data);
        }
    }
}
