using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpDemo
{
    public class NewWindowEventArgs : EventArgs
    {
        private string _targetUrl;
        public string TargetUrl
        {
            get { return _targetUrl; }
            set { _targetUrl = value; }
        }

        private IRequest _request;
        public IRequest Request
        {
            get { return _request; }
            set { _request = value; }
        }

        public NewWindowEventArgs(string targetUrl, IRequest request)
        {
            _targetUrl = targetUrl;
            _request = request;
        }
    }
}
