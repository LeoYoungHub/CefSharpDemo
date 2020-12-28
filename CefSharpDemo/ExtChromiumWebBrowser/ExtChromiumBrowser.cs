using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpDemo
{
    public class ExtChromiumBrowser : ChromiumWebBrowser
    {
        public byte[] PostData { get; set; }

        public ExtChromiumBrowser()
            : base()
        {
            this.LifeSpanHandler = new CefLifeSpanHandler();
            this.DownloadHandler = new DownloadHandler(this);
            this.MenuHandler = new MenuHandler();
            this.KeyboardHandler = new KeyboardHandler();
            this.RequestHandler = new RequestHandler(this);
        }

        public event EventHandler<NewWindowEventArgs> StartNewWindow;

        public void OnNewWindow(NewWindowEventArgs e)
        {
            if (StartNewWindow != null)
            {
                StartNewWindow(this, e);
            }
        }

        public void ClearHandlers()
        {
            //如果不清理Handler，会导致子进程CefSharp.BrowserSubprocess.exe无法释放
            this.LifeSpanHandler = null;
            this.DownloadHandler = null;
            this.MenuHandler = null;
            this.KeyboardHandler = null;
        }
    }
}
