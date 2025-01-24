using System;
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
}
