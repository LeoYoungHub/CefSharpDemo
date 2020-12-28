using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpDemo
{
    public class KeyboardHandler : CefSharp.IKeyboardHandler
    {
        public bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            //对按键的处理
            switch (windowsKeyCode)
            {
                case 123: //功能键 F12 的KeyCode
                    if (type.Equals(KeyType.RawKeyDown))
                    {
                        browser.GetHost().ShowDevTools();
                    }
                    break;
            }
            return false;
        }

        public bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            return false;
        }

    }
}
