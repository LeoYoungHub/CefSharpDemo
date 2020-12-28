using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace CefSharpDemo
{
	public class RequestHandler : IRequestHandler
	{
		private ExtChromiumBrowser _browser;

		public RequestHandler(ExtChromiumBrowser browser)
		{
			_browser = browser;
		}

		public bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
		{
			return false;
		}

		public IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
		{
			if (request.Method.ToUpper() == "POST" && request.PostData != null)
			{
				if (request.PostData.Elements.Count > 0)
				{
					_browser.PostData = new byte[request.PostData.Elements[0].Bytes.Length];
					request.PostData.Elements[0].Bytes.CopyTo(_browser.PostData, 0);
				}
			}
			return null;
		}

		public bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
		{
			return false;
		}

		public bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
		{
			return false;
		}

		public bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
		{
			return false;
		}

		public void OnPluginCrashed(IWebBrowser chromiumWebBrowser, IBrowser browser, string pluginPath)
		{

		}

		public bool OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
		{
			return false;
		}

		public void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status)
		{

		}

		public void OnRenderViewReady(IWebBrowser chromiumWebBrowser, IBrowser browser)
		{

		}

		public bool OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser, bool isProxy, string host, int port, X509Certificate2Collection certificates, ISelectClientCertificateCallback callback)
		{
			return false;
		}

		void IRequestHandler.OnDocumentAvailableInMainFrame(IWebBrowser chromiumWebBrowser, IBrowser browser)
		{

		}
	}
}
