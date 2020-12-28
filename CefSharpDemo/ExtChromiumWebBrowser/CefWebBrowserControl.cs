using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharpDemo;

namespace CefSharpDemo
{
	/// <summary>
	/// 对CefSharpWebBrowser的操作类
	/// </summary>
	public static class CefWebBrowserControl
	{

		/// <summary>
		/// 获取html内容
		/// </summary>
		/// <param name="chromiumWebBrowser">CefSharp的webbrowser</param>
		/// <returns></returns>
		public static async Task<string> GetHtmlSource(ExtChromiumBrowser chromiumWebBrowser)
		{
			string htmlStr = string.Empty;
			await chromiumWebBrowser.GetSourceAsync().ContinueWith(taskHtml =>
			{
				htmlStr = taskHtml.Result;
			});
			return htmlStr;
		}

		/// <summary>
		/// 给input赋值
		/// </summary>
		/// <param name="chromiumWebBrowser">CefSharp的webbrowser</param>
		/// <param name="id">根据id</param>
		public static string SetValueById(ExtChromiumBrowser chromiumWebBrowser, string id, string value)
		{
			if (id.Length > 0)
			{
				string js = @"(
                            function()
                            {
                                var ele= document.getElementById('" + id + @"');
                                if(ele!=null)
                                {
                                    ele.value='" + value + @"';
                                }
                            }
                           )()";
				ExecuteJs(chromiumWebBrowser, js);
				return "赋值成功";
			}
			else
				return "id不能为空";
		}

		/// <summary>
		/// 给Input赋值
		/// </summary>
		/// <param name="chromiumWebBrowser">CefSharp的webbrowser</param>
		/// <param name="name">根据控件的名称</param>
		/// <param name="innerHtml">内部html包含***</param>
		/// <param name="outHtml">外部html包含***</param>
		public static string SetValueByName(ExtChromiumBrowser chromiumWebBrowser, string name, string value, SearchType searchType = SearchType.ControlName, string innerHtml = "", string outHtml = "")
		{
			string js = string.Empty;
			string forStr = string.Empty;
			if (name.Length > 0)
			{
				if (innerHtml.Length > 0)
				{
					forStr = "if(eleAug[i].innerText.indexOf('" + innerHtml + "') != -1) ";

				}
				if (outHtml.Length > 0)
				{
					forStr = "if(eleAug[i].outerText.indexOf('" + outHtml + "') != -1) ";

				}
				string searchjs = string.Empty;
				switch (searchType)
				{
					case SearchType.ControlName: { searchjs = " var eleAug= document.getElementsByName('" + name + @"');"; } break;
					case SearchType.ClassName: { searchjs = " var eleAug= document.getElementsByClassName('" + name + @"');"; } break;
					case SearchType.TagName: { searchjs = " var eleAug= document.getElementsByTagName('" + name + @"');"; } break;
				}
				js = @"(
                            function()
                            {
                               " + searchjs + @"
                                if(eleAug!=null)
                                {
                                    for(var i=0;i< eleAug.length;i++)
                                    {                                    
                                        " + forStr + @"
                                            { ele.value='" + value + @"';}
                                    }
                                }
                            }
                           )()";
			}
			else
			{
				return "名称不能为空";
			}
			ExecuteJs(chromiumWebBrowser, js);
			return "点击成功";
		}


		/// <summary>
		/// 点击按钮
		/// </summary>
		/// <param name="chromiumWebBrowser">CefSharp的webbrowser</param>
		/// <param name="id">根据id</param>
		public static string ClickButtonById(ExtChromiumBrowser chromiumWebBrowser, string id)
		{
			if (id.Length > 0)
			{
				string js = @"(
                            function()
                            {
                                var ele= document.getElementById('" + id + @"');
                                if(ele!=null)
                                {
                                    ele.click();
                                }
                            }
                           )()";
				ExecuteJs(chromiumWebBrowser, js);
				return "点击成功";
			}
			else
				return "id不能为空";
		}
		//document.querySelector("#body > div > div > div.form-main > form > div > div:nth-child(1) > div > div > div > input").value="18126015434"
		//document.querySelector("#body > div > div > div.form-main > form > div > div:nth-child(2) > div > div > div > input").value="yl123456"
		//document.querySelector("#body > div > div > div.form-main > form > div > div.mb20 > button")
		public static void ClickButtonByJsPath(ExtChromiumBrowser chromiumWebBrowser, string JsPath)
		{
			ExecuteJs(chromiumWebBrowser, JsPath);
		}

		/// <summary>
		/// 点击按钮
		/// </summary>
		/// <param name="chromiumWebBrowser">CefSharp的webbrowser</param>
		/// <param name="name">根据控件的名称</param>
		/// <param name="innerHtml">内部html包含***</param>
		/// <param name="outHtml">外部html包含***</param>
		public static string ClickButtonByName(ExtChromiumBrowser chromiumWebBrowser, string name, SearchType searchType = SearchType.ControlName, string innerHtml = "", string outHtml = "")
		{
			string js = string.Empty;
			string forStr = string.Empty;
			if (name.Length > 0)
			{
				if (innerHtml.Length > 0)
				{
					forStr = "if(eleAug[i].innerText.indexOf('" + innerHtml + "') != -1) ";

				}
				if (outHtml.Length > 0)
				{
					forStr = "if(eleAug[i].outerText.indexOf('" + outHtml + "') != -1) ";

				}
				string searchjs = string.Empty;
				switch (searchType)
				{
					case SearchType.ControlName: { searchjs = " var eleAug= document.getElementsByName('" + name + @"');"; } break;
					case SearchType.ClassName: { searchjs = " var eleAug= document.getElementsByClassName('" + name + @"');"; } break;
					case SearchType.TagName: { searchjs = " var eleAug= document.getElementsByTagName('" + name + @"');"; } break;
				}
				js = @"(
                            function()
                            {
                               " + searchjs + @"
                                if(eleAug!=null)
                                {
                                    for(var i=0;i< eleAug.length;i++)
                                    {                                    
                                        " + forStr + @"
                                            { eleAug[i].click();}
                                    }
                                }
                            }
                           )()";
			}
			else
			{
				return "名称不能为空";
			}
			ExecuteJs(chromiumWebBrowser, js);
			return "点击成功";
		}

		/// <summary>
		/// 执行JS
		/// </summary>
		/// <param name="chromiumWebBrowser">wpf版本的ExtChromiumBrowser</param>
		/// <param name="js"></param>
		public static void ExecuteJs(ExtChromiumBrowser chromiumWebBrowser, string js)
		{
			var frame = chromiumWebBrowser.GetMainFrame();
			frame.EvaluateScriptAsync(js);
		}
	}

	
	public enum SearchType
	{
		ControlName,
		ClassName,
		TagName
	}


}
