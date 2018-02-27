using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Core;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// Log4net帮助类
    /// </summary>
    public static class Log4netHelper
    {
        /// <summary>
        /// 消息队列
        /// </summary>
        private static Queue<Log4netModel> logQueue = new Queue<Log4netModel>();

        /// <summary>
        /// 标志锁
        /// </summary>
        static string logLock = "true";

        /// <summary>
        /// 是否开始自动记录日志
        /// </summary>
        private static bool isStart = false;

        /// <summary>
        /// 获取log4net
        /// </summary>
        public static ILog Get(string name)
        {
            //配置log4Net信息( 应用程序起动调用初始配置,以下任选一种方式)
            //log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(System.Web.HttpContext.Current.Server.MapPath(@"~/Config/Log4net.config")));
            //log4net.Config.XmlConfigurator.Configure();
            return log4net.LogManager.GetLogger(name);
        }

        /// <summary>
        /// 同步写日志
        /// </summary>
        public static void Writer(this ILog log, string message, Level lev = null)
        {
            WriteModel(new Log4netModel(log, message, lev));
        }

        /// <summary>
        /// 异步写日期
        /// </summary>
        public static void AsyncWriter(this ILog log, string message, Level lev = null)
        {
            // 这里需要锁上 不然会出现：源数组长度不足。请检查 srcIndex 和长度以及数组的下限。异常   
            //网上有资料说 http://blog.csdn.net/greatbody/article/details/26135057  不能多线程同时写入队列
            //其实  不仅仅 不能同时写入队列 也不能同时读和写如队列  所以  在Dequeue 取的时候也要锁定一个对象
            lock (logLock)
            {
                logQueue.Enqueue(new Log4netModel(log, message, lev));
            }
            AsyncStart();
        }

        /// <summary>
        /// 异步写日志
        /// </summary>
        private static void AsyncStart()
        {
            if (isStart)
                return;
            isStart = true;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (logQueue.Count >= 1)
                    {
                        Log4netModel model = null;
                        lock (logLock)
                            model = logQueue.Dequeue();

                        if (model == null)
                            continue;

                        if (model.Log == null)
                            continue;

                        WriteModel(model);
                    }
                    else
                    {
                        isStart = false;//标记下次可执行
                        break;//跳出循环
                    }
                }
            });

            //4.5
            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        if (logQueue.Count >= 1)
            //        {
            //            Log4netModel model = null;
            //            lock (logLock)
            //                model = logQueue.Dequeue();

            //            if (model == null)
            //                continue;

            //            if (model.Log == null)
            //                continue;

            //            WriteModel(model);
            //        }
            //        else
            //        {
            //            isStart = false;//标记下次可执行
            //            break;//跳出循环
            //        }
            //    }
            //});
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log">日志配置</param>
        /// <param name="model">信息实体</param>
        private static void WriteModel(Log4netModel model)
        {
            if (model == null)
                return;

            if (model.Log == null)
                return;

            model.Message = string.Format("[{0}]{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), model.Message);

            //写日志
            if (model.Level == Level.Warn)
            {
                model.Log.Warn(model.Message);
            }
            else if (model.Level == Level.Error)
            {
                model.Log.Error(model.Message);
            }
            else
            {
                model.Log.Info(model.Message);
            }
        }
    }

    /// <summary>
    /// 写日志实体
    /// </summary>
    public class Log4netModel
    {
        public Log4netModel(ILog log, string message, Level lev = null)
        {
            Log = log;
            Level = lev != null ? lev : Level.Info;
            Message = message;
        }

        /// <summary>
        /// 日志配置
        /// </summary>
        public ILog Log { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 消息级别
        /// </summary>
        public Level Level { get; set; }


    }
}
