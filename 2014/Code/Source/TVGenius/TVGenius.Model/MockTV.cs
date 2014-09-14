using System;

namespace TVGenius.Model
{
    /// <summary>
    /// 模拟电视
    /// </summary>
    [Serializable]
    public class MockTV : PropertyNotifyObject
    {
        private string _brand;
        private string _model;
        private string _icon;
        private string _sn;
        private string _bind;
        private int _channel;
        private int _maxChannel;
        private int _volume;
        private int _maxVolume;
        private TVState _state;
        private DateTime _lastConnectTime;

        /// <summary>
        /// 电视品牌
        /// </summary>
        public string Brand
        {
            get { return _brand; }
            set { _brand = value; }
        }

        /// <summary>
        /// 电视型号
        /// </summary>
        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        /// <summary>
        /// 电视ICON
        /// </summary>
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        /// <summary>
        /// 电视序列号
        /// </summary>
        public string Sn
        {
            get { return _sn; }
            set { _sn = value; }
        }

        /// <summary>
        /// 电视状态
        /// </summary>
        public TVState State
        {
            get { return _state; }
            set
            {
                _state = value; 
                NotifyPropertyChanged("State");
            }
        }

        /// <summary>
        /// 通信信道
        /// </summary>
        public string Bind
        {
            get { return _bind; }
            set { _bind = value; }
        }

        /// <summary>
        /// 最后一次连接的时间
        /// </summary>
        public DateTime LastConnectTime
        {
            get { return _lastConnectTime; }
            set { _lastConnectTime = value; }
        }

        /// <summary>
        /// 当前频道
        /// </summary>
        public int Channel
        {
            get { return _channel; }
            set
            {
                if (value < 0)
                {
                    value = MaxChannel;
                }
                if (value > MaxChannel)
                {
                    value = 0;
                }
                _channel = value;
            }
        }

        /// <summary>
        /// 频道最大数目
        /// </summary>
        public int MaxChannel
        {
            get { return _maxChannel; }
            set { _maxChannel = value; }
        }

        /// <summary>
        /// 当前音量
        /// </summary>
        public int Volume
        {
            get { return _volume; }
            set
            {
                if (_volume < 0)
                {
                    value = 0;
                }
                if (_volume > MaxVolume)
                {
                    value = MaxVolume;
                }
                _volume = value;
            }
        }

        /// <summary>
        /// 最大音量
        /// </summary>
        public int MaxVolume
        {
            get { return _maxVolume; }
            set { _maxVolume = value; }
        }
    }
}
