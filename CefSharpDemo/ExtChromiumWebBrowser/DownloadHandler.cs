using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharpDemo
{
    public class DownloadHandler : IDownloadHandler
    {
        ExtChromiumBrowser _browser;

        public DownloadHandler(ExtChromiumBrowser browser)
        {
            _browser = browser;
        }

        public void OnBeforeDownload(IWebBrowser ExtChromiumBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    callback.Continue(downloadItem.SuggestedFileName, true);
                }
            }
        }

        public void OnDownloadUpdated(IWebBrowser ExtChromiumBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {

        }
    }
}
