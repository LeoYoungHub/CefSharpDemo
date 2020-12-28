using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CefSharpDemo
{
    public class CloseTabItemEventArgs : EventArgs
    {
        private TabItem _TabItem;
        public TabItem TabItem
        {
            get { return _TabItem; }
            set { _TabItem = value; }
        }

        public CloseTabItemEventArgs(TabItem tabItem)
        {
            _TabItem = tabItem;
        }
    }
}
