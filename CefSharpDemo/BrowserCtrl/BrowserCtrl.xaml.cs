using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utils;

namespace CefSharpDemo
{
	/// <summary>
	/// 浏览器用户控件
	/// </summary>
	public partial class BrowserCtrl : UserControl, IDisposable
	{
		#region 外部方法
		/*
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int CloseWindow(IntPtr hWnd);
        [DllImport("User32.dll", EntryPoint = "GetWindowText")]
        private static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int nMaxCount);
        */
		#endregion

		#region 变量属性事件
		private static bool _isCefInited = false;

		private static object _lockObject = new object();

		private JSObject _jsObject;

		private bool _firstLoad = true;

		/// <summary>
		/// 在此事件中设置URL(此事件已在线程中执行，此事件已对错误情况进行处理)
		/// </summary>
		public event EventHandler SetUrlEvent;

		/// <summary>
		/// URL
		/// </summary>
		public string Url { get; set; }

		public IRequest Request { get; set; }

		/// <summary>
		/// 浏览器FrameLoadEnd事件
		/// </summary>
		public event EventHandler FrameLoadEnd;

		private ExtChromiumBrowser _browser;

		public ExtChromiumBrowser Browser
		{
			get
			{
				WaitUtil.Wait(() => this._browser != null && this._browser.IsInitialized && _isCefInited);

				return this._browser;
			}
		}

		private static LimitedTaskScheduler _scheduler = new LimitedTaskScheduler(2);
		#endregion

		#region 构造函数
		public BrowserCtrl()
		{
			InitializeComponent();
			if (DesignerProperties.GetIsInDesignMode(this)) return;

			this.Loaded += BrowserCtrl_Loaded;

			lock (_lockObject)
			{
				if (!_isCefInited)
				{
					_isCefInited = true;
					InitCef();//初始化CefSharp
				}
			}

			_browser = new ExtChromiumBrowser();
			BindBrowser(_browser);
			grid.Children.Add(_browser);
		}
		#endregion

		#region BrowserCtrl_Loaded
		private void BrowserCtrl_Loaded(object sender, RoutedEventArgs e)
		{

		}
		#endregion

		#region SetMapCtrl
		/// <summary>
		/// 设置Map控件接口，用于C#和JS互操作
		/// </summary>
		public void SetMapCtrl(IMapCtrl mapCtrl)
		{
			_jsObject.MapCtrl = mapCtrl;
		}
		#endregion

		#region Dispose 释放资源
		/// <summary>
		/// 释放资源
		/// </summary>
		public void Dispose()
		{
			//如果有弹出窗口则先释放它
			//foreach (UIElement item in grid.Children)
			//{
			//    if (item is BrowserContainer)
			//    {
			//        (item as BrowserContainer).ClearResource();
			//    }
			//}

			_browser.ClearHandlers();
			if (_browser != null && !_browser.IsDisposed)
			{
				_browser.Dispose();
			}
		}
		#endregion

		#region Load
		public void Load(string url)
		{
			if (!string.IsNullOrWhiteSpace(url))
			{
				loadingWait.Visibility = Visibility.Visible;
				Url = url;
				_scheduler.Run(() =>
				{
					#region Wait
					WaitUtil.Wait(() =>
					{
						if (this._browser == null) return false;
						if (!this._browser.IsInitialized) return false;
						if (!_isCefInited) return false;
						bool isBrowserInitialized = false;
						this.Dispatcher.Invoke(() =>
						{
							isBrowserInitialized = this._browser.IsBrowserInitialized;
						});
						if (!isBrowserInitialized) return false;
						return true;
					});
					#endregion

					_browser.Load(Url);
				});
			}
		}
		#endregion

		#region LoadUrl
		private void LoadUrl()
		{
			if (_firstLoad)
			{
				_firstLoad = false;

				_scheduler.Run(() =>
				{
					#region Wait
					WaitUtil.Wait(() =>
					{
						if (this._browser == null) return false;
						if (!this._browser.IsInitialized) return false;
						if (!_isCefInited) return false;
						bool isBrowserInitialized = false;
						this.Dispatcher.Invoke(() =>
						{
							isBrowserInitialized = this._browser.IsBrowserInitialized;
						});
						if (!isBrowserInitialized) return false;
						return true;
					});
					#endregion

					if (Url == null && SetUrlEvent != null)
					{
						try
						{
							SetUrlEvent(this, null);
						}
						catch (Exception ex)
						{
							LogUtil.Error(ex, "BrowserCtrl LoadUrl error 获取URL失败");
						}
					}
					else
					{
						this.Dispatcher.Invoke(new Action(() =>
						{
							loadingWait.Visibility = Visibility.Collapsed;
						}));
					}

					if (Url != null)
					{
						try
						{
							if (Request == null)
							{
								_browser.Load(Url);
							}
							else
							{
								_browser.Load(Url);
								_browser.GetMainFrame().LoadRequest(Request);
								Request = null;
							}
						}
						catch (Exception ex)
						{
							LogUtil.Error(ex, "BrowserCtrl LoadUrl error Load URL失败");
						}
					}
					else
					{
						this.Dispatcher.Invoke(new Action(() =>
						{
							loadingWait.Visibility = Visibility.Collapsed;
						}));
					}
				});
			}
		}
		#endregion

		#region BindBrowser
		private void BindBrowser(ExtChromiumBrowser browser)
		{
			_jsObject = new JSObject();

			CefSharpSettings.LegacyJavascriptBindingEnabled = true;
			CefSharpSettings.WcfEnabled = true;
			browser.JavascriptObjectRepository.Register("jsObj", _jsObject, isAsync: false, options: BindingOptions.DefaultBinder);

			browser.IsBrowserInitializedChanged += (ss, ee) =>
			{
				LoadUrl();
			};
			browser.FrameLoadStart += (ss, ee) =>
			{
				this.Dispatcher.BeginInvoke(new Action(() =>
				{
					(ss as ExtChromiumBrowser).Focus();
				}));
			};
			browser.FrameLoadEnd += (ss, ee) =>
			{
				this.Dispatcher.BeginInvoke(new Action(() =>
				{
					loadingWait.Visibility = Visibility.Collapsed;
				}));
				if (FrameLoadEnd != null)
				{
					FrameLoadEnd(null, null);
				}
			};
			browser.KeyDown += (ss, ee) =>
			{
				if (ee.Key == Key.F5)
				{
					try
					{
						browser.Reload();
					}
					catch (Exception ex)
					{
						LogUtil.Error(ex, "ExtChromiumBrowser Reload error");
					}
				}
			};
			browser.PreviewTextInput += (o, e) =>
			{
				foreach (var character in e.Text)
				{
					// 把每个字符向浏览器组件发送一遍
					browser.GetBrowser().GetHost().SendKeyEvent((int)WM.CHAR, (int)character, 0);
				}

				// 不让cef自己处理
				e.Handled = true;
			};
			browser.LoadError += (s, e) =>
			{
				this.Dispatcher.BeginInvoke(new Action(() =>
				{
					loadingWait.Visibility = Visibility.Collapsed;
				}));
			};
		}
		#endregion

		#region RegisterJsObject
		public void RegisterJsObject(string name, object objectToBind, BindingOptions options = null)
		{
			try
			{
				if (_browser != null)
				{
					_browser.RegisterJsObject(name, objectToBind, options);
				}
			}
			catch (Exception ex)
			{
				LogUtil.Error(ex, "BrowserCtrl RegisterJsObject 错误");
			}
		}
		#endregion

		#region 初始化CefSharp
		public static void InitCef()
		{
			//string cefsharpFolder = "CefSharp1";

			var settings = new CefSettings();
			//The location where cache data will be stored on disk. If empty an in-memory cache will be used for some features and a temporary disk cache for others.
			//HTML5 databases such as localStorage will only persist across sessions if a cache path is specified. 
			settings.CachePath = AppDomain.CurrentDomain.BaseDirectory + "cache";

			settings.MultiThreadedMessageLoop = true;
			CefSharpSettings.FocusedNodeChangedEnabled = true;
			CefSharpSettings.LegacyJavascriptBindingEnabled = true;
			CefSharpSettings.ShutdownOnExit = true;
			CefSharpSettings.SubprocessExitIfParentProcessClosed = true;

			string logDir = AppDomain.CurrentDomain.BaseDirectory + "log/";
			if (!Directory.Exists(logDir))
			{
				Directory.CreateDirectory(logDir);
			}

			settings.BrowserSubprocessPath = AppDomain.CurrentDomain.BaseDirectory + "CefSharp.BrowserSubprocess.exe";
			settings.LogFile = logDir + DateTime.Now.ToString("yyyyMMdd") + ".log";
			settings.LocalesDirPath = AppDomain.CurrentDomain.BaseDirectory + "locales";
			settings.CefCommandLineArgs.Add("disable-gpu", "1");
			settings.CefCommandLineArgs.Add("enable-media-stream", "1");

			if (!Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: new BrowserProcessHandler()))
			{
				throw new Exception("Unable to Initialize Cef");
			}
		}
		#endregion

	}
}
