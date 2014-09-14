using TVGenius.Model;

namespace TVGenius.SignalTransfer.Dispatcher
{
    /// <summary>
    /// 命令分发器基类
    /// </summary>
    abstract  public class SingalDispatcherBase
    {
        public void Run(string bindStr)
        {
            
        }

        /// <summary>
        /// 更换频道
        /// </summary>
        /// <param name="channelNum"></param>
        public abstract void ChangeChannel(int channelNum);

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="action">音量设置动作</param>
        public abstract void ChangeVolume(VolumeAction action);

        /// <summary>
        /// 设置电视机状态
        /// </summary>
        /// <param name="state"></param>
        public abstract void SetState(TVState state);

        /// <summary>
        /// 模拟遥控器按键
        /// </summary>
        /// <param name="keyCode">按键编码</param>
        public abstract void SiumKeyPress(string keyCode);

        /// <summary>
        /// 模拟鼠标动作
        /// </summary>
        /// <param name="action">鼠标动作</param>
        public abstract void SiumMouseAction(MouseAction action);

        /// <summary>
        /// 模拟键盘
        /// </summary>
        /// <param name="keyCode">键盘按键值</param>
        public abstract void SiumKeyboard(string keyCode);
    }
}
