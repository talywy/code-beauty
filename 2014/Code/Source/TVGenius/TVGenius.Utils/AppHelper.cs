using System;
using System.IO;

namespace TVGenius.Utils
{
    /// <summary>
    /// 程序帮助类
    /// </summary>
    public static class AppHelper
    {
        private static string _appBase;
        private static string _resourceBase;
        private static string _iconBase;
        private static string _videoBase;

        /// <summary>
        /// 程序运行根路径
        /// </summary>
        public static string AppBase
        {
            get
            {
                return _appBase ?? (_appBase = AppDomain.CurrentDomain.BaseDirectory);
            }
        }

        /// <summary>
        /// 资源文件夹根路径
        /// </summary>
        public static string ResourceBase
        {
            get
            {
                return _resourceBase ?? (_resourceBase = Path.GetFullPath(Path.Combine(AppBase, "../_resource")));
            }
        }

        /// <summary>
        /// ICON图标根路径
        /// </summary>
        public static string IconBase
        {
            get
            {
                return _iconBase ?? (_iconBase = Path.Combine(ResourceBase, "icons"));
            }
        }

        /// <summary>
        /// 视频资源根路径
        /// </summary>
        public static string VideoBase
        {
            get
            {
                return _videoBase ?? (_videoBase = Path.Combine(ResourceBase, "videos"));
            }
        }
        
    }
}
