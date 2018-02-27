using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleX.Infrastructure.Utility
{
    /// <summary>
    /// 时间工具类
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// 默认时间
        /// </summary>
        public static DateTime DefaultDateTime
        {
            get
            {
                return DateTime.Parse("1900-01-01 00:00:00");
            }
        }

        /// <summary>
        /// 获取对象时间值(出错：返回默认DefaultValue)
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>返回DateTime对象</returns>
        public static DateTime Get(object obj, DateTime? defaultValue = null)
        {
            if (defaultValue == null)
                defaultValue = DefaultDateTime;

            DateTime returnValue = defaultValue.Value;
            if (obj != null)
            {
                DateTime.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// 获取字符串时间值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回DateTime对象</returns>
        public static DateTime Get(string str, DateTime? defaultValue = null)
        {
            if (defaultValue == null)
                defaultValue = DefaultDateTime;

            DateTime returnValue = defaultValue.Value;

            if (!string.IsNullOrWhiteSpace(str))
            {
                str = str.Trim();
                DateTime.TryParse(str, out returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="format">格式</param>
        /// <returns>返回DateTime对象的字符串string</returns>
        public static DateTime FormatDateTime(DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return DateTime.Parse(dateTime.ToString(format));
        }

        /// <summary>
        /// 获取两个时间差值信息
        /// </summary>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="endDateTime">结束时间</param>
        /// <returns></returns>
        public static DateTimeDifferenceInfo GetDifferenceInfo(DateTime startDateTime, DateTime? endDateTime=null)
        {
            if (endDateTime == null) { endDateTime = DateTime.Now; };

            //TimeSpan span = DateTime.Now - dt;
            //if (span.TotalDays > 60)
            //{
            //    return dt.ToShortDateString();
            //}
            //else
            //if (span.TotalDays > 30)
            //{
            //    return "one months ago";
            //}
            //else
            //if (span.TotalDays > 14)
            //{
            //    return "tow weeks ago";
            //}
            //else
            //if (span.TotalDays > 7)
            //{
            //    return "one weeks ago";
            //}
            //else
            //if (span.TotalDays > 1)
            //{
            //    return string.Format("{0} days ago", (int)Math.Floor(span.TotalDays));
            //}
            //else
            //if (span.TotalHours > 1)
            //{
            //    return string.Format("{0} hour ago", (int)Math.Floor(span.TotalHours));
            //}
            //else
            //if (span.TotalMinutes > 1)
            //{
            //    return string.Format("{0} minutes ago", (int)Math.Floor(span.TotalMinutes));
            //}
            //else
            //if (span.TotalSeconds >= 1)
            //{
            //    return string.Format("{0} seconds ago", (int)Math.Floor(span.TotalSeconds));
            //}
            //else
            //{
            //    return "1 seconds ago";
            //}

            return new DateTimeDifferenceInfo();
        }
    }


    /// <summary>
    /// 时间信息类型
    /// </summary>
    public enum EmDateTimeType
    {
        Second,
        Minute,
        Hour,
        Day,
        Week,
        Month,
        Year,
    }

    /// <summary>
    /// 两个时间相差信息
    /// </summary>
    public class DateTimeDifferenceInfo
    {
        /// <summary>
        /// 差值年部份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 差值月部份
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 差值周部份
        /// </summary>
        public int Week { get; set; }
        /// <summary>
        /// 差值天部份
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 差值时部份
        /// </summary>
        public int Hour { get; set; }
        /// <summary>
        /// 差值分部份
        /// </summary>
        public int Minute { get; set; }
        /// <summary>
        /// 差值秒部份
        /// </summary>
        public int Second { get; set; }

        /// <summary>
        /// 差值年总值(不够为1，值为0)
        /// </summary>
        public int TotalYear { get; set; }
        /// <summary>
        /// 差值月总值(不够为1，值为0)
        /// </summary>
        public int TotalMonth { get; set; }
        /// <summary>
        /// 差值周总值(不够为1，值为0)
        /// </summary>
        public int TotalWeek { get; set; }
        /// <summary>
        /// 差值天总值(不够为1，值为0)
        /// </summary>
        public int TotalDay { get; set; }
        /// <summary>
        /// 差值时总值(不够为1，值为0)
        /// </summary>
        public int TotalHour { get; set; }
        /// <summary>
        /// 差值分总值(不够为1，值为0)
        /// </summary>
        public int TotalMinute { get; set; }
        /// <summary>
        /// 差值秒总值(不够为1，值为0)
        /// </summary>
        public int TotalSecond { get; set; }

    }
}
