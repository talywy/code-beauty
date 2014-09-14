using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private Timer _tipTimer;
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
            SetVideo(0);
        }

        private void ActionReceiverOnActionReceived(object sender, ActionReceivedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => HandleAction(e.Data)));
        }

        private void MediaOnMediaEnded(object sender, RoutedEventArgs e)
        {
            Media.Stop();
            Media.Play();
        }

        private void SetVideo(int videoIndex)
        {
            Media.Source = new Uri(_videos[videoIndex]);
        }

        private void HandleAction(JObject data)
        {
            var type = data["type"].Value<string>();

            if (type.Equals(ActionType.CHANGE_CHANNEL))
            {
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
                ShowTip("Channel:" + _tv.Channel);
                SetVideo(_tv.Channel % _videos.Count);
            }
            else if(type.Equals(ActionType.VOLUME_DECREASE))
            {
                ShowTip("Volume:" + --_tv.Volume);
                Media.Volume = (double)_tv.Volume / _tv.MaxVolume;
            }
            else if (type.Equals(ActionType.VOLUME_INCREASE))
            {
                ShowTip("Volume:" + ++_tv.Volume);
                Media.Volume = (double) _tv.Volume / _tv.MaxVolume;
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

        private void ShowTip(string tip)
        {
            _tipTimer.Stop();
            TxtTip.Visibility = Visibility.Visible;
            TxtTip.Text = tip;
            _tipTimer.Start();
        }
    }
}
