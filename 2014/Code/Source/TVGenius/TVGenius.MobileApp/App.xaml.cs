using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TVGenius.SignalTransfer;
using TVGenius.Utils;

namespace TVGenius.MobileApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ConsoleHelper.AllocConsole();

            LogUtil.Init(typeof(App));

            this.Startup += OnStartup;
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnStartup(object sender, StartupEventArgs startupEventArgs)
        {
            var signalMapPath = Path.Combine(AppHelper.ResourceBase, "SignalMap.xml");
            SignalMap.Init(signalMapPath);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            LogUtil.Log.Error("OnDispatcherUnhandledException", e.Exception);
        }
    }
}
