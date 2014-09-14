using System;
using System.Runtime.InteropServices;

namespace TVGenius.Utils
{
    public class ConsoleHelper
    {
        /// <summary>
        /// 为当前进程分配控制台
        /// </summary>
        /// <returns>
        /// 分配结果
        /// <remarks>true: 分配成功, false:分配失败</remarks>
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();

        /// <summary>
        /// 释放当前进程的控制台
        /// </summary>
        /// <returns>
        /// 释放结果
        /// <remarks>true: 释放成功, false:释放失败</remarks>
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeConsole();

        /// <summary>
        /// 获取窗口句柄
        /// </summary>
        /// <param name="lpClassName">窗口类名</param>
        /// <param name="lpWindowName">窗口标题</param>
        /// <returns>
        /// 窗口句柄
        /// <remarks>0: 未找到窗口, 其他数字为窗口句柄</remarks>
        /// </returns>
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="type">
        /// 窗口显示类别
        /// <remarks>0: 后台执行，1:正常启动，2:最小化到任务栏，3:最大化</remarks>
        /// </param>
        /// <returns>
        /// 显示结果
        /// <remarks>true:设置成功， false:设置失败</remarks>
        /// </returns>
        [DllImport("User32.dll", EntryPoint = "ShowWindow")]
        private static extern bool ShowWindow(IntPtr hWnd, int type);

        /// <summary>
        /// 设置控制台状态
        /// </summary>
        /// <param name="consoleTitle">控制台标题</param>
        /// <param name="state">控制台状态</param>
        public static void SetConsoleState(string consoleTitle, ConsoleState state)
        {
            IntPtr consoleWin = FindWindow(null, consoleTitle);
            ShowWindow(consoleWin, (int)state);
        }
    }

    /// <summary>
    /// 控制台状态
    /// </summary>
    public enum ConsoleState
    {
        /// <summary>
        /// 隐藏
        /// </summary>
        Hidden,

        /// <summary>
        /// 正常状态
        /// </summary>
        Normal,

        /// <summary>
        /// 最小化
        /// </summary>
        Minimize,

        /// <summary>
        /// 最大化
        /// </summary>
        Maximize
    }
}
