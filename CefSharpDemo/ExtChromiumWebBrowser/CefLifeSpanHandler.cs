using CefSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Utils;

namespace CefSharpDemo
{
    public class CefLifeSpanHandler : CefSharp.ILifeSpanHandler
    {
        private static LimitedTaskScheduler _scheduler = new LimitedTaskScheduler(2);

        public CefLifeSpanHandler()
        {

        }

        public bool DoClose(IWebBrowser browserControl, CefSharp.IBrowser browser)
        {
            if (browser.IsDisposed || browser.IsPopup)
            {
                return false;
            }

            return true;
        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {

        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            var chromiumWebBrowser = (ExtChromiumBrowser)browserControl;

            chromiumWebBrowser.Dispatcher.Invoke(new Action(() =>
            {
                BrowserPopupWin win = new BrowserPopupWin();
                win.ShowInTaskbar = false;
                win.Height = 0;
                win.Width = 0;
                win.Show();

                IntPtr handle = new WindowInteropHelper(win).Handle;
                windowInfo.SetAsChild(handle);

                _scheduler.Run(() =>
                {
                    WaitUtil.Wait(() => chromiumWebBrowser.PostData);

                    IRequest request = null;
                    if (chromiumWebBrowser.PostData != null)
                    {
                        request = frame.CreateRequest();
                        request.Url = targetUrl;
                        request.Method = "POST";

                        request.InitializePostData();
                        var element = request.PostData.CreatePostDataElement();
                        element.Bytes = chromiumWebBrowser.PostData;
                        request.PostData.AddElement(element);
                        chromiumWebBrowser.PostData = null;
                    }

                    chromiumWebBrowser.Dispatcher.Invoke(new Action(() =>
                    {
                        NewWindowEventArgs e = new NewWindowEventArgs(targetUrl, request);
                        chromiumWebBrowser.OnNewWindow(e);
                    }));

                    chromiumWebBrowser.Dispatcher.Invoke(new Action(() =>
                    {
                        win.Close();
                    }));
                });
            }));

            newBrowser = null;
            return false;
        }

    }
}
