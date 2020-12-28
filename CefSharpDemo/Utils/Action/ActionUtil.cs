using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Windows.Threading;

namespace Utils
{
    /// <summary>
    /// Action工具类
    /// </summary>
    public static class ActionUtil
    {
        #region TryDoAction
        /// <summary>
        /// 带异常处理的Action
        /// </summary>
        public static void TryDoAction(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
            }
        }
        #endregion

        #region Winform TryBeginInvoke
        /*
        /// <summary>
        /// Winform使用的带异常处理的BeginInvoke
        /// </summary>
        public static void TryBeginInvoke(this Control ctrl, Action action)
        {
            ctrl.BeginInvoke(new Action(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                }
            }));
        }*/
        #endregion

        #region Winform TryInvoke 
        /*
        /// <summary>
        /// Winform使用的带异常处理的Invoke
        /// </summary>
        public static void TryInvoke2(this Control ctrl, Action action)
        {
            ctrl.Invoke(new Action(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                }
            }));
        }*/
        #endregion

        #region WPF TryInvokeAsync
        /// <summary>
        /// WPF使用的带异常处理的InvokeAsync
        /// </summary>
        public static void TryInvokeAsync(this DispatcherObject ctrl, Action action)
        {
            ctrl.Dispatcher.InvokeAsync(new Action(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                }
            }));
        }
        #endregion

        #region WPF TryInvoke 
        /// <summary>
        /// WPF使用的带异常处理的Invoke
        /// </summary>
        public static void TryInvoke(this DispatcherObject ctrl, Action action)
        {
            ctrl.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                }
            }));
        }
        #endregion

    }
}
