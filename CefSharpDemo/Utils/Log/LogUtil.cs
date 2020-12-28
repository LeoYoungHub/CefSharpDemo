using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// 写日志类
    /// </summary>
    public class LogUtil
    {
        #region 字段
        private static object _lock = new object();
        private static string _path = null;
        private static Dictionary<string, int> _dictIndex = new Dictionary<string, int>();
        private static Dictionary<string, long> _dictSize = new Dictionary<string, long>();
        private static Dictionary<string, FileStream> _dictStream = new Dictionary<string, FileStream>();
        private static Dictionary<string, StreamWriter> _dictWriter = new Dictionary<string, StreamWriter>();
        private static LimitedTaskScheduler _scheduler = new LimitedTaskScheduler(2);
        private static int _fileSize = 10 * 1024 * 1024; //日志分隔文件大小
        #endregion

        #region 写文件
        /// <summary>
        /// 写文件
        /// </summary>
        private static void WriteFile(string log, string path)
        {
            try
            {
                FileStream fs = null;
                StreamWriter sw = null;

                if (!(_dictStream.TryGetValue(path, out fs) && _dictWriter.TryGetValue(path, out sw)))
                {
                    foreach (StreamWriter item in _dictWriter.Values.ToList())
                    {
                        item.Close();
                    }
                    _dictWriter.Clear();

                    foreach (FileStream item in _dictStream.Values.ToList())
                    {
                        item.Close();
                    }
                    _dictWriter.Clear();

                    if (!Directory.Exists(Path.GetDirectoryName(path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                    }

                    fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                    sw = new StreamWriter(fs);
                    _dictWriter.Add(path, sw);
                    _dictStream.Add(path, fs);
                }

                sw.WriteLine(log);
                sw.Flush();
                fs.Flush();
            }
            catch { }
        }
        #endregion

        #region 生成日志文件路径
        /// <summary>
        /// 生成日志文件路径
        /// </summary>
        private static string CreateLogPath(string folder, string log)
        {
            try
            {
                if (_path == null)
                {
                    UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
                    _path = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
                }

                string pathFolder = Path.Combine(_path, folder);
                if (!Directory.Exists(Path.GetDirectoryName(pathFolder)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(pathFolder));
                }

                int currentIndex;
                long size;
                string strNow = DateTime.Now.ToString("yyyyMMdd");
                string strKey = pathFolder + strNow;
                if (!(_dictIndex.TryGetValue(strKey, out currentIndex) && _dictSize.TryGetValue(strKey, out size)))
                {
                    _dictIndex.Clear();
                    _dictSize.Clear();

                    GetIndexAndSize(pathFolder, strNow, out currentIndex, out size);
                    if (size >= _fileSize) currentIndex++;
                    _dictIndex.Add(strKey, currentIndex);
                    _dictSize.Add(strKey, size);
                }

                int index = _dictIndex[strKey];
                string logPath = Path.Combine(pathFolder, strNow + (index == 1 ? "" : "_" + index.ToString()) + ".txt");

                _dictSize[strKey] += Encoding.UTF8.GetByteCount(log);
                if (_dictSize[strKey] > _fileSize)
                {
                    _dictIndex[strKey]++;
                    _dictSize[strKey] = 0;
                }

                return logPath;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 拼接日志内容
        /// <summary>
        /// 拼接日志内容
        /// </summary>
        private static string CreateLogString(string prefix, string log)
        {
            return string.Format(@"{0} {1} {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), prefix.PadRight(7, ' '), log);
        }
        #endregion

        #region 获取初始Index和Size
        /// <summary>
        /// 获取初始Index和Size
        /// </summary>
        private static void GetIndexAndSize(string pathFolder, string strNow, out int index, out long size)
        {
            index = 1;
            size = 0;
            Regex regex = new Regex(strNow + "_*(\\d*).txt");
            string[] fileArr = Directory.GetFiles(pathFolder);
            string currentFile = null;
            foreach (string file in fileArr)
            {
                Match match = regex.Match(file);
                if (match.Success)
                {
                    string str = match.Groups[1].Value;
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        int temp = Convert.ToInt32(str);
                        if (temp > index)
                        {
                            index = temp;
                            currentFile = file;
                        }
                    }
                    else
                    {
                        index = 1;
                        currentFile = file;
                    }
                }
            }

            if (currentFile != null)
            {
                FileInfo fileInfo = new FileInfo(currentFile);
                size = fileInfo.Length;
            }
        }
        #endregion

        #region 写调试日志
        /// <summary>
        /// 写调试日志
        /// </summary>
        public static Task Debug(string log)
        {
            return Task.Factory.StartNew(() =>
            {
                lock (_lock)
                {
                    log = CreateLogString("[Debug]", log);
                    WriteFile(log, CreateLogPath("Log\\Debug\\", log));
                }
            }, CancellationToken.None, TaskCreationOptions.None, _scheduler);
        }
        #endregion

        #region 写错误日志
        public static Task Error(Exception ex, string log = null)
        {
            return Error(string.IsNullOrEmpty(log) ? ex.Message + "\r\n" + ex.StackTrace : (log + "：") + ex.Message + "\r\n" + ex.StackTrace);
        }
        /// <summary>
        /// 写错误日志
        /// </summary>
        public static Task Error(string log)
        {
            return Task.Factory.StartNew(() =>
            {
                lock (_lock)
                {
                    log = CreateLogString("[Error]", log);
                    WriteFile(log, CreateLogPath("Log\\Error\\", log));
                }
            }, CancellationToken.None, TaskCreationOptions.None, _scheduler);
        }
        #endregion

        #region 写操作日志
        /// <summary>
        /// 写操作日志
        /// </summary>
        public static Task Log(string log)
        {
            return Task.Factory.StartNew(() =>
            {
                lock (_lock)
                {
                    log = CreateLogString("[Info]", log);
                    WriteFile(log, CreateLogPath("Log\\Info\\", log));
                }
            }, CancellationToken.None, TaskCreationOptions.None, _scheduler);
        }
        #endregion

    }
}
