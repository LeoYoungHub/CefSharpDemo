using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CefSharpDemo
{
    /// <summary>
    /// 浏览器控件
    /// </summary>
    public partial class BrowserDemoCtrl : UserControl, IDisposable
    {
        public BrowserDemoCtrl()
        {
            InitializeComponent();

            browserCtrl.Browser.TitleChanged += (s, e) =>
            {
                TabItem tabItem = this.Parent as TabItem;
                string title = (s as ExtChromiumBrowser).Title;
                if (title.Length > 20) title = title.Substring(0, 20) + "...";
                tabItem.Header = title;
            };
            browserCtrl.Browser.FrameLoadStart += (s, e) =>
            {
                this.Dispatcher.InvokeAsync(() =>
                {
                    txtUrl.Text = e.Url;
                });
            };
        }

        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                browserCtrl.Load(txtUrl.Text.Trim());
            }
        }

        #region Dispose 释放资源
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            browserCtrl.Dispose();
        }
        #endregion

    }
}
