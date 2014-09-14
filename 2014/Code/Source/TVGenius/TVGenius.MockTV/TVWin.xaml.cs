using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
using TVGenius.ResourceMgr;
using TVGenius.SignalTransfer;
using TVGenius.SignalTransfer.Events;
using TVGenius.Utils;
using TVModel = TVGenius.Model.MockTV;

namespace TVGenius.MockTV
{
    /// <summary>
    /// Interaction logic for TVWin.xaml
    /// </summary>
    public partial class TVWin : Window
    {
        private static readonly DependencyProperty IsHibernateProperty;

        static TVWin()
        {
            IsHibernateProperty = DependencyProperty.Register("IsHibernate", typeof(bool), typeof(TVWin), new PropertyMetadata(OnIsHibernatePropertyChanged));
        }

        private static void OnIsHibernatePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((TVWin)obj)._tv.State = (bool)e.NewValue ? Model.TVState.Hibernate : Model.TVState.PowerOn;
        }

        public bool IsHibernate
        {
            get { return (bool)GetValue(IsHibernateProperty); }
            private set { SetValue(IsHibernateProperty, value); } 
        }

        private readonly SignalReceiver _signalReceiver;
        private List<string> _videos;
        private readonly Timer _tipTimer;
        private const int TIP_DURATION = 3000;

        private readonly TVModel _tv;

        public TVWin()
        {
            InitializeComponent();

            _tv = new TVModel
            {
                Brand = MockTVConfig.Instance.Brand,
                Model = MockTVConfig.Instance.Model,
                Channel = MockTVConfig.Instance.Channel,
                MaxChannel = MockTVConfig.Instance.MaxChannel,
                MaxVolume = MockTVConfig.Instance.MaxVolume,
                Volume = MockTVConfig.Instance.Volume,
                Bind = MockTVConfig.Instance.Bind,
                Icon = MockTVConfig.Instance.Icon,
                Sn = MockTVConfig.Instance.Sn,
                State = IsHibernate  ? Model.TVState.Hibernate : Model.TVState.PowerOn
            };

            var timer = new Timer(1000);
            timer.Elapsed += TimerElapsed;
            timer.Start();
            _signalReceiver = new SignalReceiver(_tv);

            _tipTimer = new Timer(TIP_DURATION);
            _tipTimer.Elapsed += TipTimerElapsed;
            this.Loaded += OnLoaded;
        }

        void TipTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _tipTimer.Stop();
            this.Dispatcher.Invoke(new Action(() =>
            {
                TxtTip.Visibility = Visibility.Collapsed;
            }));
        }

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                TxtTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }));
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Icon = new BitmapImage(new Uri(MockTVConfig.Instance.Icon));
            this.Title = string.Format("{0} - {1}", MockTVConfig.Instance.Brand , MockTVConfig.Instance.Model);
            _signalReceiver.ActionReceived += ActionReceiverOnActionReceived;
            _signalReceiver.Run();
           _videos = VideoList.Instance.GetVideList();
            SetVideo();
        }

        private void ActionReceiverOnActionReceived(object sender, ActionReceivedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => HandleAction(e.Data)));
        }

        /// <summary>
        /// 处理收到的信号数据
        /// </summary>
        /// <param name="data">数据</param>
        private void HandleAction(JObject data)
        {
            var type = data["type"].Value<string>();

            if (type.Equals(ActionType.CHANGE_CHANNEL))
            {
                if (IsHibernate)
                {
                    return;
                }

                var channelAction = (ChannelAction)data["data"]["action"].Value<int>();
                if (channelAction == ChannelAction.Next)
                {
                    ++_tv.Channel;
                }
                else if (channelAction == ChannelAction.Previous)
                {
                    --_tv.Channel;
                }
                else
                {
                    var num = data["num"].Value<int>();
                    _tv.Channel = num;
                }
                
                SetVideo();
            }
            else if (type.Equals(ActionType.VOLUME_DECREASE))
            {
                if (IsHibernate)
                {
                    return;
                }
                --_tv.Volume;
                SetVolume();
            }
            else if (type.Equals(ActionType.VOLUME_INCREASE))
            {
                if (IsHibernate)
                {
                    return;
                }
                ++_tv.Volume;
                SetVolume();
            }
            else if(type.Equals(ActionType.HIBERNATE))
            {
                IsHibernate = !IsHibernate;
            }
            else
            {
                LogUtil.Log.WarnFormat("Unsupport action:" + type);
            }
        }

        private void SetVolume()
        {
            ShowTip("Volume:" + _tv.Volume);
            Player.Volume = (double)_tv.Volume / _tv.MaxVolume;
        }

        private void SetVideo()
        {
            ShowTip("Channel:" + _tv.Channel);
            var index = _tv.Channel % _videos.Count;
            Player.Source = new Uri(_videos[index]);
        }

        private void ShowTip(string tip)
        {
            _tipTimer.Stop();
            TxtTip.Visibility = Visibility.Visible;
            TxtTip.Text = tip;
            _tipTimer.Start();
        }

        private void Player_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            Player.Position = new TimeSpan(0, 0, 1);
            Player.Play();
        }
    }
}
