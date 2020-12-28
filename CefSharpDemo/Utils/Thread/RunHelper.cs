using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// 线程工具类
    /// </summary>
    public static class RunHelper
    {
        #region 变量属性事件

        #endregion

        #region 线程中执行
        /// <summary>
        /// 线程中执行
        /// </summary>
        public static Task Run(this LimitedTaskScheduler scheduler, Action<object> doWork, object arg = null, Action<Exception> errorAction = null)
        {
            Task task = task = Task.Factory.StartNew((obj) =>
            {
                try
                {
                    doWork(obj);
                }
                catch (Exception ex)
                {
                    if (errorAction != null) errorAction(ex);
                    LogUtil.Error(ex, "ThreadUtil.Run错误");
                }
            }, arg, CancellationToken.None, TaskCreationOptions.None, scheduler);
            return task;
        }
        #endregion

        #region 线程中执行
        /// <summary>
        /// 线程中执行
        /// </summary>
        public static Task Run(this LimitedTaskScheduler scheduler, Action doWork, Action<Exception> errorAction = null)
        {
            Task task = task = Task.Factory.StartNew(() =>
            {
                try
                {
                    doWork();
                }
                catch (Exception ex)
                {
                    if (errorAction != null) errorAction(ex);
                    LogUtil.Error(ex, "ThreadUtil.Run错误");
                }
            }, CancellationToken.None, TaskCreationOptions.None, scheduler);
            return task;
        }
        #endregion

    }
}
