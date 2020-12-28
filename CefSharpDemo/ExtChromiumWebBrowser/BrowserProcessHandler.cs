using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpDemo
{
    public class BrowserProcessHandler : IBrowserProcessHandler
    {
        /// <summary>
        /// The maximum number of milliseconds we're willing to wait between calls to OnScheduleMessagePumpWork().
        /// </summary>
        protected const int MaxTimerDelay = 1000 / 30;  // 30fps

        void IBrowserProcessHandler.OnContextInitialized()
        {
            string cefsharpFolder = "CefSharp";

            var cookieManager = Cef.GetGlobalCookieManager();
            //cookieManager.SetStoragePath(AppDomain.CurrentDomain.BaseDirectory + cefsharpFolder + "/cookies", true);
            //cookieManager.SetSupportedSchemes(new string[] { "custom" });
            cookieManager.SetSupportedSchemes(new string[] { "custom" }, true);
        }

        void IBrowserProcessHandler.OnScheduleMessagePumpWork(long delay)
        {
            //If the delay is greater than the Maximum then use MaxTimerDelay
            //instead - we do this to achieve a minimum number of FPS
            if (delay > MaxTimerDelay)
            {
                delay = MaxTimerDelay;
            }
            OnScheduleMessagePumpWork((int)delay);
        }

        protected virtual void OnScheduleMessagePumpWork(int delay)
        {
            //TODO: Schedule work on the UI thread - call Cef.DoMessageLoopWork
        }

        public virtual void Dispose()
        {

        }
    }
}
