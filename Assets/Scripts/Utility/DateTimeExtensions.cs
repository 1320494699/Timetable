using System;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Example
{
    public struct DateTimeRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateTimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }

    public static class DateTimeExtensions
    {
        // 检查两个日期段是否重叠，并返回重叠部分
        public static DateTimeRange? GetOverlap(this DateTimeRange range1, DateTimeRange range2)
        {
            // 计算重叠部分的开始日期，取两个开始日期中的最大值
            DateTime overlapStart = range1.Start > range2.Start ? range1.Start : range2.Start;
            // 计算重叠部分的结束日期，取两个结束日期中的最小值
            DateTime overlapEnd = range1.End < range2.End ? range1.End : range2.End;

            // 判断是否有重叠
            if (overlapStart < overlapEnd)
            {
                return new DateTimeRange(overlapStart, overlapEnd);
            }

            return null;
        }

        // 检查一个日期段是否在两个日期之间
        public static bool GetIsOverlap(this DateTimeRange range1, DateTime time)
        {
            bool isOverlap = (time >= range1.Start && time <= range1.End);
            //Debug.Log("start:"+range1.Start + " end:" + range1.End + " currentTime:" + time+" isOverlap:"+isOverlap);
            // 日期在范围内
            return isOverlap;
        }

        /// <summary>
        /// 获取指定日期范围内的特定星期几的日期集合
        /// </summary>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="targetWeekday">星期几</param>
        /// <returns></returns>
        public static DateTime[] GetSpecificWeekdayDates(this DateTime startDate, DateTime endDate,
            DayOfWeek targetWeekday)
        {
            // 确保开始日期不晚于结束日期
            if (startDate > endDate)
            {
                throw new ArgumentException("开始日期不能晚于结束日期。");
            }

            // 存储符合条件的日期
            List<DateTime> dates = new List<DateTime>();
            // 找到第一个符合条件的周几
            while (startDate.DayOfWeek != targetWeekday)
            {
                startDate = startDate.AddDays(1);
                if (startDate > endDate)
                {
                    return dates.ToArray();
                }
            }

            // 收集所有符合条件的日期
            while (startDate <= endDate)
            {
                dates.Add(startDate);
                startDate = startDate.AddDays(7);
            }

            return dates.ToArray();
        }

        /// <summary>
        /// 周日对应0，周六对应6
        /// 调整为 周一对应0，周日对应6 
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static int WeekdayAdjust(this DayOfWeek dayOfWeek)
        {
            int weekday = (int) dayOfWeek;
            weekday -= 1;
            if (weekday < 0)
            {
                weekday = 6;
            }

            return weekday;
        }
        /// <summary>
        /// 获取当前周的日期范围
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTimeRange GetCurrentWeekRange(DateTime dt)
        {
            DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d"))); //本周周一  
            DateTime endWeek = startWeek.AddDays(6); //本周周日
            return new DateTimeRange(startWeek, endWeek);
        }
        /// <summary>
        /// 获取当前日期是本月的第几周
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetWeekNumberInMonth(this DateTime date)
        {
            // 获取本月第一天
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);

            // 计算本月第一天是星期几（Monday 为 1，Sunday 为 7）
            int weekdayOfFirstDay = (int)firstDayOfMonth.DayOfWeek;
            if (weekdayOfFirstDay == 0)
            {
                weekdayOfFirstDay = 7; // 将 Sunday 从 0 调整为 7
            }

            // 如果本月第一天所在周的天数少于 4 天，则跳过该周
            int offset = 0;
            if (weekdayOfFirstDay > 4) // 即星期五（5）及以后开始的第一周跳过
            {
                offset = 8 - weekdayOfFirstDay;
            }

            // 计算从本月第一天到当前日期的天数
            int daysSinceFirstDay = (date - firstDayOfMonth).Days;

            // 计算当前是本月的第几周
            return (daysSinceFirstDay + weekdayOfFirstDay - 1 - offset) / 7 + 1;
        }
        /// <summary>
        /// 计算 本月有几周
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetWeeksInMonth(int year, int month)
        {
            // 获取本月的第一天
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            // 获取本月的最后一天
            DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            // 计算第一周的天数
            int daysInFirstWeek = 7 - (int)firstDayOfMonth.DayOfWeek;
            // 计算最后一周的天数
            int daysInLastWeek = (int)lastDayOfMonth.DayOfWeek + 1;

            // 计算中间的整周数量
            int totalDays = DateTime.DaysInMonth(year, month);
            int middleWeeks = (totalDays - daysInFirstWeek - daysInLastWeek) / 7;

            // 计算总周数
            int totalWeeks = middleWeeks;
            if (daysInFirstWeek >= 4)
            {
                totalWeeks++;
            }
            if (daysInLastWeek >= 4)
            {
                totalWeeks++;
            }

            return totalWeeks;
        }
        /// 根据几月、第几周计算周一日期
        public static DateTime GetMondayOfWeek(int year, int month, int weekNumber)
        {
            // 获取本月的第一天
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            int daysInFirstWeek = 7 - (int)firstDayOfMonth.DayOfWeek;

            // 检查第一周是否有效
            bool firstWeekValid = daysInFirstWeek >= 4;

            // 检查周数是否有效
            int weeksInMonth = GetWeeksInMonth(year, month);
            if (weekNumber < 1 || weekNumber > weeksInMonth)
            {
                return DateTime.MinValue;
            }

            // 计算第一周周一的日期
            DateTime firstMonday;
            if (firstWeekValid)
            {
                firstMonday = firstDayOfMonth.AddDays((int)DayOfWeek.Monday - (int)firstDayOfMonth.DayOfWeek);
            }
            else
            {
                firstMonday = firstDayOfMonth.AddDays(7 - (int)firstDayOfMonth.DayOfWeek + (int)DayOfWeek.Monday);
            }

            // 计算指定周数对应的周一日期
            return firstMonday.AddDays((weekNumber - 1) * 7);
        }
    }
}