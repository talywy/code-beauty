using System.Configuration;
using System.Windows;
using System.Windows.Threading;
using TVGenius.Utils;

namespace TVGenius.MockTV
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var isDebugStr = ConfigurationManager.AppSettings["IsDebug"];
            bool isDebug;
            bool.TryParse(isDebugStr, out isDebug);

            if (isDebug)
            {
                ConsoleHelper.AllocConsole();
            }

            LogUtil.Init(typeof(App));

            this.Startup += OnStartup;
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length != 1)
            {
                LogUtil.Log.Error("Invalid startup args");
            }
            else
            {
                var configFile = e.Args[0];
                MockTVConfig.Instance.Init(configFile);
            }
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            LogUtil.Log.Error("OnDispatcherUnhandledException", e.Exception);
            MessageBox.Show(e.Exception.Message, "OnDispatcherUnhandledException");
        }
    }
}
