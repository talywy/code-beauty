using System;
using System.Windows;
using System.Windows.Controls;
using TVGenius.MobileApp.Events;
using TVGenius.Model;
using TVGenius.TVScanner;
using TVGenius.Utils;

namespace TVGenius.MobileApp.Controls
{
    /// <summary>
    /// Interaction logic for TVListControl.xaml
    /// </summary>
    public partial class TVListControl : UserControl
    {
        public event EventHandler<MockTVSelectedEventArgs> MockTVSelected;

        private readonly TVScanner.TVScanner _tvScanner;
        private readonly TVListMgr _tvListMgr;

        public TVListControl()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
      
            _tvScanner = new TVScanner.TVScanner();
            _tvScanner.TVFound += TVScannerOnTVFound;
            _tvScanner.TVHeartBeat += TVScannerOnTVHeartBeat;

            _tvListMgr = new TVListMgr();
            TVList.ItemsSource = _tvListMgr.MockTVs;
            TVList.SelectionChanged += TVListOnSelectionChanged;
        }

        private void TVListOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
            {
                return;
            }

            var tv = (MockTV)e.AddedItems[0];
            if (tv.State != TVState.Offline)
            {
                MockTVSelected(this, new MockTVSelectedEventArgs(tv));
            }
            else
            {
                MessageBox.Show("The TV is not power on!", "Tip");
            }

            TVList.SelectedItem = null;
        }

        private void TVScannerOnTVFound(object sender, TVFoundEventArgs e)
        {
            LogUtil.Log.InfoFormat("Find TV");
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_tvListMgr.ContainTV(e.TV.Sn))
                {
                    _tvListMgr.RefreshTV(e.TV.Sn, e.TV.State);
                }
                else
                {
                    var message = string.Format("Found TV: {0} - {1} \n\nDo you want to add this TV in your tv list?",
                        e.TV.Brand, e.TV.Model);
                    if (MessageBox.Show(message, "FoundTV", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        _tvListMgr.AddTV(e.TV);
                    }
                }
            }));
        }

        private void TVScannerOnTVHeartBeat(object sender, TVHeartBeatEventArgs e)
        {
            _tvListMgr.RefreshTV(e.TVSn, e.State);
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _tvScanner.Start();
        }

        public void SaveTVList()
        {
            _tvListMgr.SaveTVList();
        }

        private void BtnRefreshClick(object sender, RoutedEventArgs e)
        {
            _tvScanner.Refresh();
        }
    }
}
