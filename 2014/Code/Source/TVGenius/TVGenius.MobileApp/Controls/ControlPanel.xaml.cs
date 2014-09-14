using System;
using System.Windows;
using System.Windows.Controls;
using TVGenius.Model;

namespace TVGenius.MobileApp.Controls
{
    /// <summary>
    /// Interaction logic for ControlPanel.xaml
    /// </summary>
    public partial class ControlPanel : UserControl
    {
        public event EventHandler Return;

        public ControlPanel()
        {
            InitializeComponent();
        }

        public void SetMockTV(MockTV tv)
        {
            
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Return(this, e);
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
