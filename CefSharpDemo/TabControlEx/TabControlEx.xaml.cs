using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// TabControl控件封装
    /// </summary>
    public partial class TabControlEx : TabControl
    {
        public event EventHandler<CloseTabItemEventArgs> CloseTabItemEvent;

        public event EventHandler AddTabItemEvent;

        /// <summary>
        /// TabItem右键菜单源
        /// </summary>
        private TabItem _contextMenuSource;

        public TabControlEx()
        {
            InitializeComponent();

            this.ContextMenu = null;
        }

        private void tabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void tabItem_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _contextMenuSource = (sender as Grid).TemplatedParent as TabItem;
            this.menu.PlacementTarget = sender as Grid;
            this.menu.Placement = PlacementMode.MousePoint;
            this.menu.IsOpen = true;
        }

        #region TabItem右键菜单点击事件
        private void menuItemClick(object sender, RoutedEventArgs e)
        {
            MenuItem btn = e.Source as MenuItem;
            int data = Convert.ToInt32(btn.CommandParameter.ToString());

            if (_contextMenuSource != null)
            {
                List<TabItem> tabItemList = new List<TabItem>();
                if (data == 0)
                {
                    tabItemList.Add(_contextMenuSource);
                }
                if (data == 1)
                {
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        TabItem tabItem = this.Items[i] as TabItem;
                        if (tabItem != _contextMenuSource)
                        {
                            tabItemList.Add(tabItem);
                        }
                    }
                }
                if (data == 2)
                {
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        TabItem tabItem = this.Items[i] as TabItem;
                        if (tabItem != _contextMenuSource)
                        {
                            tabItemList.Add(tabItem);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (data == 3)
                {
                    for (int i = this.Items.Count - 1; i >= 0; i--)
                    {
                        TabItem tabItem = this.Items[i] as TabItem;
                        if (tabItem != _contextMenuSource)
                        {
                            tabItemList.Add(tabItem);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                foreach (TabItem tabItem in tabItemList)
                {
                    CloseTabItem(tabItem);
                }
            }
        }
        #endregion

        private void btnTabItemClose_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var tmplParent = (btn.Parent as Grid).TemplatedParent;
            var tabItem = tmplParent as TabItem;
            CloseTabItem(tabItem);
        }

        #region 关闭TabItem
        /// <summary>
        /// 关闭TabItem，并释放资源
        /// </summary>
        private void CloseTabItem(TabItem tabItem)
        {
            if (tabItem.Content is IDisposable)
            {
                var content = tabItem.Content as IDisposable;
                if (content != null)
                {
                    content.Dispose();
                }
                tabItem.Content = null;
                content = null;
            }
            this.Items.Remove(tabItem);
            if (CloseTabItemEvent != null)
            {
                CloseTabItemEventArgs args = new CloseTabItemEventArgs(tabItem);
                CloseTabItemEvent(this, args);
            }
        }
        #endregion

        #region 关闭所有TabItem
        /// <summary>
        /// 关闭所有TabItem，并释放资源
        /// </summary>
        public void CloseAllTabItem()
        {
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                CloseTabItem(this.Items[i] as TabItem);
            }
        }
        #endregion

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.RemovedItems)
            {
                if (item is TabItem)
                {
                    Panel.SetZIndex(item as TabItem, 99);
                }
            }
            foreach (var item in e.AddedItems)
            {
                if (item is TabItem)
                {
                    Panel.SetZIndex(item as TabItem, 999);
                }
            }
        }

        //标签栏滚动
        private void StackPanel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            StackPanel panel = sender as StackPanel;
            ScrollViewer scroll = GetParentScrollViewer(panel);
            double offset = scroll.HorizontalOffset - e.Delta;
            scroll.ScrollToHorizontalOffset(offset);
        }

        #region GetParentScrollViewer
        private ScrollViewer GetParentScrollViewer(FrameworkElement element)
        {
            if (element != null)
            {
                if (element.Parent != null)
                {
                    if (element.Parent is ScrollViewer)
                    {
                        return element.Parent as ScrollViewer;
                    }
                    else
                    {
                        return GetParentScrollViewer(element.Parent as FrameworkElement);
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        //添加新标签页
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (AddTabItemEvent != null)
            {
                AddTabItemEvent(null, null);
            }
        }
    }
}
