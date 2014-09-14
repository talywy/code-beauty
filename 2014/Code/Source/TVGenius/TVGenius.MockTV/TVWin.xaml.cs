using System;
using System.Windows;
using System.Windows.Media.Imaging;
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

        public TVWin()
        {
            InitializeComponent();

            _signalReceiver = new SignalReceiver(MockTVConfig.Instance.TVJson);
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Icon = new BitmapImage(new Uri(MockTVConfig.Instance.Icon));
            this.Title = string.Format("{0} - {1}", MockTVConfig.Instance.Brand , MockTVConfig.Instance.Model);
            _signalReceiver.SignalReceived += SignalReceiverOnSignalReceived;
            _signalReceiver.Run();
        }

        private void SignalReceiverOnSignalReceived(object sender, SignalReceivedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                TxtScreen.Text = e.Signal;
            }));
        }
    }
}
