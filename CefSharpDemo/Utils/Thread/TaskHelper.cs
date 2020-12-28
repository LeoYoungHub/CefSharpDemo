using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// Task帮助类基类
    /// </summary>
    public class TaskHelper
    {
        #region UI任务
        private static LimitedTaskScheduler _UITask;
        /// <summary>
        /// UI任务(4个线程)
        /// </summary>
        public static LimitedTaskScheduler UITask
        {
            get
            {
                if (_UITask == null) _UITask = new LimitedTaskScheduler(4);
                return _UITask;
            }
        }
        #endregion

        #region 计算任务
        private static LimitedTaskScheduler _CalcTask;
        /// <summary>
        /// 计算任务(8个线程)
        /// </summary>
        public static LimitedTaskScheduler CalcTask
        {
            get
            {
                if (_CalcTask == null) _CalcTask = new LimitedTaskScheduler(8);
                return _CalcTask;
            }
        }
        #endregion

        #region 网络请求
        private static LimitedTaskScheduler _RequestTask;
        /// <summary>
        /// 网络请求(32个线程)
        /// </summary>
        public static LimitedTaskScheduler RequestTask
        {
            get
            {
                if (_RequestTask == null) _RequestTask = new LimitedTaskScheduler(32);
                return _RequestTask;
            }
        }
        #endregion

        #region 数据库任务
        private static LimitedTaskScheduler _DBTask;
        /// <summary>
        /// 数据库任务(32个线程)
        /// </summary>
        public static LimitedTaskScheduler DBTask
        {
            get
            {
                if (_DBTask == null) _DBTask = new LimitedTaskScheduler(32);
                return _DBTask;
            }
        }
        #endregion

        #region IO任务
        private static LimitedTaskScheduler _IOTask;
        /// <summary>
        /// IO任务(8个线程)
        /// </summary>
        public static LimitedTaskScheduler IOTask
        {
            get
            {
                if (_IOTask == null) _IOTask = new LimitedTaskScheduler(8);
                return _IOTask;
            }
        }
        #endregion

    }
}
