using System;
using System.Windows;
using System.Windows.Controls;

namespace TVGenius.MobileApp.Controls
{
    /// <summary>
    /// Interaction logic for ResourceExplorer.xaml
    /// </summary>
    public partial class ResourceExplorer : UserControl
    {
        public event EventHandler Return;

        public ResourceExplorer()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Return(this, e);
        }
    }
}
