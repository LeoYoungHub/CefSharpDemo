using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using Utils;

namespace CefSharpDemo
{
	/// <summary>
	/// CefSharp Demo 窗体
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			tabControl.AddTabItemEvent += tabControl_AddTabItemEvent;
			Application.Current.MainWindow = this;
		}

		private void tabControl_AddTabItemEvent(object sender, EventArgs e)
		{
			//CreateTabItem("https://www.cnblogs.com/");
			CreateTabItem("https://www.cnblogs.com/");
		}

		/// <summary>
		/// 新增Tab页
		/// </summary>
		private void CreateTabItem(string url = null, IRequest request = null)
		{
			TabItem tabItem = new TabItem();
			tabItem.Header = "新标签页";
			BrowserDemoCtrl ctrl = new BrowserDemoCtrl();
			ctrl.browserCtrl.Browser.StartNewWindow += (s, e) =>
			{
				CreateTabItem(e.TargetUrl, e.Request);
			};
			ctrl.browserCtrl.SetUrlEvent += (s, e) =>
			{
				ctrl.browserCtrl.Url = url;
				ctrl.browserCtrl.Request = request;
			};
			tabItem.Content = ctrl;
			tabControl.Items.Add(tabItem);
			tabControl.SelectedItem = tabItem;
			ScrollViewer scrollViewer = tabControl.Template.FindName("scrollViewer", tabControl) as ScrollViewer;
			scrollViewer.ScrollToRightEnd();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			tabControl.CloseAllTabItem(); //关闭窗体清理资源

			//程序退出时删除cache
			CefSharp.Cef.Shutdown();
			string cachePath = AppDomain.CurrentDomain.BaseDirectory + "\\cache";
			if (Directory.Exists(cachePath))
			{
				foreach (string path in Directory.GetDirectories(cachePath))
				{
					Directory.Delete(path, true);
				}
				foreach (string file in Directory.GetFiles(cachePath))
				{
					if (!file.ToLower().Contains("cookies"))
					{
						File.Delete(file);
					}
				}
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			CreateTabItem(txtPath.Text);
		}
		Task task = null;
		CancellationTokenSource cts = null;
		private async void Button1_Click(object sender, RoutedEventArgs e)
		{
			var tab = tabControl.SelectedItem as TabItem;
			BrowserDemoCtrl ctr = tab.Content as BrowserDemoCtrl;

			if (btnGetData.Tag.ToString().Equals("1"))
			{
				btnGetData.Tag = "2";
				btnGetData.Content = "暂停抓取";
			}
			else
			{
				btnGetData.Tag = "1";
				btnGetData.Content = "抓取数据";
				if (cts != null && !cts.IsCancellationRequested)
				{
					cts?.Cancel();
				}
				return;
			}
			cts = new CancellationTokenSource();
			CancellationToken ct2 = cts.Token;

			if (txtPath.Text.Contains("shuatishenqi"))
			{
				task = GetSTSQData(ctr.browserCtrl.Browser);
			}
			else
			{
				//task = GetKSBData();
			}
			await task;

		}

		private async Task GetSTSQData(ExtChromiumBrowser chromiumWebBrowser)
		{
			int topic_index = 0;

			while (!cts.Token.IsCancellationRequested)
			{
				topic_index++;
				var topic_type = string.Empty;
				var topic = string.Empty;
				try
				{
					await Task.Delay(300);
					string html = await CefWebBrowserControl.GetHtmlSource(chromiumWebBrowser);

					HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
					doc.LoadHtml(html);//html字符串
									   //获取标签里的值select-left pull-left options-w

					var enable = doc.DocumentNode.SelectSingleNode("//div[@class='btn_ct tb']/span[2]");
					if (enable.Attributes["disabled"] != null && enable.Attributes["disabled"].Value.Equals("disabled"))
					{
						break;
					}

					topic_type = doc.DocumentNode.SelectSingleNode("//div[@class='test_style']").InnerText.Split(new char[2] { '【', '】' })[1];
					//topic_index = doc.DocumentNode.SelectSingleNode("//span[@class='topic-type']/following-sibling::span").InnerText.Replace("\\n", "").Split('/')[1];
					topic = doc.DocumentNode.SelectSingleNode("//div[@class='swiper-slide swiper-slide-active']/div[@class='test_title']/span").InnerHtml;

					if ("单选题 多选题 判断题 不定项选择题 排序题".Contains(topic_type))
					{
						var answers = doc.DocumentNode.SelectNodes("//div[@class='test_bot']/dl");

						CefWebBrowserControl.ClickButtonByJsPath(chromiumWebBrowser, @"document.querySelector('div.test_bot').children[0].children[0].click()");
						await Task.Delay(200);

						listBox1.Items.Add($"[{ topic_type}]");
						listBox1.Items.Add($"{topic_index}、{topic}");
						foreach (var item in answers)
						{
							string value1 = item.SelectSingleNode("./span").InnerText;
							string value2 = item.InnerText;

							listBox1.Items.Add($"{value1}{value2}");
						}
					}

					var answerRight = doc.DocumentNode.SelectSingleNode("//span[@class='rightAnswer']").InnerText;
					var answerAnalysis = doc.DocumentNode.SelectSingleNode("//div[@class='answerText']/div[4]").InnerHtml;
					listBox1.Items.Add("答案:" + answerRight);
					listBox1.Items.Add(answerAnalysis);

					CefWebBrowserControl.ClickButtonByJsPath(chromiumWebBrowser, @"document.querySelector('div.test_bot').lastElementChild.firstElementChild.click()");

				}
				catch (Exception ex)
				{
					listBox1.Items.Add($"错误行:{topic_type},{topic_index},{topic}");
					cts?.Cancel();
					break;
				}
			}

		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			var tab = tabControl.SelectedItem as TabItem;

			BrowserDemoCtrl ctr = tab.Content as BrowserDemoCtrl;
			ctr.browserCtrl.Browser.GetBrowser().ShowDevTools();
		}
	}
}
