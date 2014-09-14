using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using TVGenius.Model;
using TVGenius.SignalTransfer.Dispatcher;

namespace TVGenius.MobileApp.Controls
{
    /// <summary>
    /// Interaction logic for ControlPanel.xaml
    /// </summary>
    public partial class ControlPanel : UserControl
    {
        public event EventHandler Return;

        private SingalDispatcherBase _singalDispatcher;

        private static readonly DependencyProperty TVProperty;

        static ControlPanel()
        {
            TVProperty = DependencyProperty.Register("TV", typeof(MockTV), typeof(ControlPanel), new PropertyMetadata(OnTVPropertyChanged));
        }

        private static void OnTVPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public MockTV TV
        {
            get { return (MockTV) GetValue(TVProperty); }
            private set { SetValue(TVProperty, value);} 
        }

        public ControlPanel()
        {
            InitializeComponent();
            BindingCommand();
        }

        public void SetMockTV(MockTV tv)
        {
            TV = tv;
            TV.PropertyChanged += NewTVOnPropertyChanged;
            TVInfo.DataContext = TV;
            _singalDispatcher = DispatcherFactory.GetDispatcher(tv);
        }

        private void NewTVOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("State") && ((MockTV)sender).State == TVState.Offline)
            {
                TriggerReturn();
            }
        }

        private void BtnReturnClick(object sender, RoutedEventArgs e)
        {
            TriggerReturn();
        }

        private void TriggerReturn()
        {
            TV.PropertyChanged -= NewTVOnPropertyChanged;
            Return(this, null);
        }

        private void BtnLoacalResourceClick(object sender, RoutedEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void BtnOnlineResourceClick(object sender, RoutedEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void BtnVoiceControlClick(object sender, RoutedEventArgs e)
        {
            // throw new NotImplementedException();
        }
    }
}
