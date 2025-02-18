using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using QFramework;
using UnityEngine;

namespace QFramework.Example
{
    public class TimetableModel : AbstractModel
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

        public void AddTimetableItemData(TimetableItemData timetableItemData)
        {
            TimetableData.timetableItems.Add(timetableItemData);
        }

        /// 把学生课时段分解成TimetableItemData集合
        public List<TimetableItemData> DecomposeStudentTimetable(StudentData studentData)
        {
            
            List<TimetableItemData> timetableItems = new List<TimetableItemData>();
            DateTime startTime = studentData.startTime;
            DateTime endTime = studentData.endTime;
            for (int i = 0; i < studentData.weekday.Count; i++)
            {
                int weekday = studentData.weekday[i];
                DayOfWeek dayOfWeek = (DayOfWeek) weekday;
                DateTime[] dates = startTime.GetSpecificWeekdayDates(endTime, dayOfWeek);
                foreach (var date in dates)
                {
                    for (int j = 0; j < studentData.classNumber.Count; j++)
                    {
                        TimetableItemData timetableItemData = new TimetableItemData();
                        timetableItemData.studentData = studentData;
                        timetableItemData.time = date;
                        timetableItemData.classNumber = studentData.classNumber[j];
                        timetableItems.Add(timetableItemData);
                        //Debug.Log($"{studentData.name} {date.ToString("yyyy-MM-dd")} 第{studentData.classNumber[j]}节课");
                    }
                }
            }
            return timetableItems;
        }


        /// <summary>
        /// 判断时间是否有冲突
        /// </summary>
        /// <param name="studentData"></param>
        /// <param name="timetableItems"></param>
        /// <returns> true 冲突，false 不冲突</returns>
        public bool IsTimeConflict(StudentData studentData, List<TimetableItemData> timetableItems)
        {
            DateTime startTime = studentData.startTime;
            DateTime endTime = studentData.endTime;
            DateTimeRange timeRange = new DateTimeRange(startTime, endTime);

            HashSet<(DateTime, int)> timeSet = new HashSet<(DateTime, int)>();

            //遍历课表数据，取出和学生时间有重叠的课
            List<TimetableItemData> timetableItemDatas = new List<TimetableItemData>();
            foreach (var timetableItemData in TimetableData.timetableItems)
            {
                if (timeRange.GetIsOverlap(timetableItemData.time))
                {
                    timetableItemDatas.Add(timetableItemData);
                    timeSet.Add((timetableItemData.time, timetableItemData.classNumber));
                }
            }

            //如果无重合时间，则返回false
            if (timetableItemDatas.Count <= 0)
            {
                return false;
            }
            List<TimetableItemData> conflictTimetableItems = new List<TimetableItemData>();
            //如果有重合时间，则判断是否有相同的课
            //如果有相同的课，则返回true
            foreach (var timetableItemData in timetableItems)
            {
                if (timeSet.Contains((timetableItemData.time, timetableItemData.classNumber)))
                {
                    conflictTimetableItems.Add(timetableItemData);
                    Debug.Log($"学生 {timetableItemData.studentData.name} 在{timetableItemData.time:yyyy-MM-dd} 第{timetableItemData.classNumber}节课时间冲突");
                }
            }

            if (conflictTimetableItems.Count > 0)
            {
                return true;
            }

            return false;
        }

        ///刷新显示本周课程表数据
        public void RefreshCurrentWeekTimetableData(DateTime currentTime)
        {
            DateTimeRange currentWeekTimeRange = DateTimeExtensions.GetCurrentWeekRange(currentTime);
            Debug.Log("当前周时间：" + currentWeekTimeRange.Start.ToString("yyyy-MM-dd") + " 至 " + currentWeekTimeRange.End.ToString("yyyy-MM-dd"));
            foreach (var timetableItemData in TimetableData.timetableItems)
            {
                //判断和本周时间是否有重叠
                if (currentWeekTimeRange.GetIsOverlap(timetableItemData.time))
                {
                    int weekday = timetableItemData.time.DayOfWeek.WeekdayAdjust();
                    TimetableData.UpdateTimetableItemData(timetableItemData.classNumber,
                        weekday, timetableItemData);
                    Debug.Log("当前周时间有重叠，添加到课表" + timetableItemData.time.ToString("yyyy-MM-dd") + " 第" + timetableItemData.classNumber + "节课");
                }
            }
        }
    }
}