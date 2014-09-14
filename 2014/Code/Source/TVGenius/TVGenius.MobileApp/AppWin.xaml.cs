using System;
using System.Windows;
using TVGenius.MobileApp.Controls;

namespace TVGenius.MobileApp
{
    /// <summary>
    /// Interaction logic for AppWin.xaml
    /// </summary>
    public partial class AppWin : Window
    {
        private readonly TVListControl _tvListControl;
        private readonly ControlPanel _controlPanel;

        public AppWin()
        {
            InitializeComponent();

            _tvListControl = new TVListControl();
            _tvListControl.MockTVSelected += TVListControlMockTVSelected;

            _controlPanel = new ControlPanel();
            _controlPanel.Return += ControlPanelReturn;

            this.Loaded += AppWinLoaded;
            this.Closing += OnMainWindowClosing;
        }

        void AppWinLoaded(object sender, RoutedEventArgs e)
        {
            this.Content = _tvListControl;
        }

        void ControlPanelReturn(object sender, EventArgs e)
        {
            this.Content = _tvListControl;
        }

        void TVListControlMockTVSelected(object sender, Events.MockTVSelectedEventArgs e)
        {
            this.Content = _controlPanel;
        }

        void OnMainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _tvListControl.SaveTVList();
        }
    }
}
