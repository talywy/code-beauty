using System;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using TVGenius.SignalTransfer;
using TVGenius.SignalTransfer.Events;

namespace TVGenius.MockTV
{
    /// <summary>
    /// Interaction logic for TVWin.xaml
    /// </summary>
    public partial class TVWin : Window
    {
        private readonly SignalReceiver _signalReceiver;
        private Timer _timer;

        public TVWin()
        {
            InitializeComponent();

            _timer = new Timer(1000);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
            _signalReceiver = new SignalReceiver(MockTVConfig.Instance.TVJson);
            this.Loaded += OnLoaded;
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
        }

        private void ActionReceiverOnActionReceived(object sender, ActionReceivedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                TxtScreen.Text =  JsonConvert.SerializeObject(e.Data);
            }));
        }
    }
}
