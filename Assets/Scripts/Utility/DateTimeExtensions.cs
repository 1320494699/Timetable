using System;
using System.Collections.Generic;
using UnityEngine;

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
        // 日期在范围内
        return (time >= range1.Start && time <= range1.End);
    }
    /// <summary>
    /// 获取指定日期范围内的特定星期几的日期集合
    /// </summary>
    /// <param name="startDate">开始日期</param>
    /// <param name="endDate">结束日期</param>
    /// <param name="targetWeekday">星期几</param>
    /// <returns></returns>
    
    public static DateTime[] GetSpecificWeekdayDates(this DateTime startDate, DateTime endDate, DayOfWeek targetWeekday)
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
        int weekday = (int)dayOfWeek;
        weekday -= 1;
        if (weekday < 0)
        {
            weekday = 6;
        }
        return weekday;
    }

    public static DateTimeRange GetCurrentWeekRange(DateTime dt)
    {
        DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一  
        DateTime endWeek = startWeek.AddDays(6);  //本周周日
        return new DateTimeRange(startWeek, endWeek);
    }
}
