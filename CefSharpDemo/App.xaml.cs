using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Utils;

namespace CefSharpDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;
            this.DispatcherUnhandledException += Application_DispatcherUnhandledException;

            base.OnStartup(e);
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LogUtil.Error(e.Exception, "Application_DispatcherUnhandledException");
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        private void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogUtil.Error(e.ExceptionObject as Exception, "AppDomain_UnhandledException");
        }

    }
}
