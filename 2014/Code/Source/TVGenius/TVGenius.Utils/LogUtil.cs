using System;
using System.Diagnostics;
using System.Text;
using log4net;

namespace TVGenius.Utils
{
    /// <summary>
    /// 日志类.在使用前需要调用Init 初始化
    /// </summary>
    public class LogUtil
    {
        private static ILog _log;

        static LogUtil()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// 根据主程序的assembly name 初始化日志
        /// </summary>
        /// <param name="assemblyType"></param>
        public static void Init(Type assemblyType)
        {
            _log = (log4net.ILog)LogManager.GetLogger(assemblyType.Assembly.GetName().Name);
            WriteHead();
        }

        /// <summary>
        /// 指定名称
        /// </summary>
        /// <param name="loggerName"></param>
        public static void Init(string loggerName)
        {
            _log = (log4net.ILog)LogManager.GetLogger(loggerName);
            WriteHead();
        }

        /// <summary>
        /// 写文件头部
        /// </summary>
        private static void WriteHead()
        {
            StringBuilder sb = new StringBuilder();

            Process curProcess = Process.GetCurrentProcess();
            sb.Append(Environment.NewLine);
            sb.AppendLine("*============================================================");
            sb.AppendLine("Subject:   iFlytek Log File");
            sb.AppendLine("BeginTime: " + DateTime.Now.ToString("yyyy/MM/dd,HH:mm:ss,fff"));
            sb.AppendLine("MainApp:   " + curProcess.MainModule.FileName);
            sb.AppendLine("Version:   " + curProcess.MainModule.FileVersionInfo.FileVersion);
            sb.AppendLine(string.Format("ProcessID: {0:D}(0x{0:X4})", curProcess.Id));
            sb.AppendLine("=============================================================");
            Log.Info(sb.ToString());
        }

        public static ILog Log
        {
            get
            {
                return _log;
            }
        }
    }
}
