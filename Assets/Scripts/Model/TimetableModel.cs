using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using QFramework;
using UnityEngine;

public class TimetableModel:AbstractModel
{
    public TimetableData TimetableData { get; set; }
    
    protected override void OnInit()
    {
        var load = this.GetUtility<DataSaveLoadUtility>();
        List<TimetableItemData> data = load.LoadData();
        TimetableData = new TimetableData();
        TimetableData.timetableItems = data;
        
        TimetableData.InitTimetableData(5, 7);
        
    }
    ///判断时间是否有冲突
    public bool IsTimeConflict(DateTime startTime, DateTime endTime)
    {
        return false;
    }
    
    //刷新显示本周课程表
    public void RefreshCurrentWeekTimetableData()
    {
        
        DateTime dt = DateTime.Today;  //当前时间  
        DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一  
        DateTime endWeek = startWeek.AddDays(6);  //本周周日
        
			
        foreach (var timetableItemData in TimetableData.timetableItems)
        {
            DateTimeRange timeRange = new DateTimeRange(startWeek, endWeek);

            //判断和本周时间是否有重叠
            if (timeRange.GetIsOverlap(timetableItemData.time))
            {
                // 0123456 对应日一二三四五六
                //让结果左移一位，因为周一对应0，周日对应6
                int weekdayAdjust(int weekday)
                {
                    weekday -= 1;
                    if (weekday < 0)
                    {
                        weekday = 6;
                    }
                    return weekday;
                }

                int weekday = weekdayAdjust((int) timetableItemData.time.DayOfWeek);
                TimetableData.UpdateTimetableItemData(timetableItemData.classNumber - 1,
                    weekday  , timetableItemData);
                Debug.Log("当前周时间有重叠，添加到课表");
            }
        }
    }
}
