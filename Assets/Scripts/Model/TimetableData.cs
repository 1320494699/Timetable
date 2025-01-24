using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class TimetableData
{
    /// <summary>
    /// 所有时间表数据
    /// </summary>
    public List<TimetableItemData> timetableItems = new ();
    public event Action<int, int, TimetableItemData> OnTimetableItemDataChanged;
    ///当前周的时间表
    public TimetableItemData[,] currWeekTimetableItems;
    
    public void InitTimetableData(int row, int column)
    {
        currWeekTimetableItems = new TimetableItemData[row, column];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                currWeekTimetableItems[i,j] = new TimetableItemData();
            }
        }
        TimetableItemData timetableItemData = new TimetableItemData();
        timetableItemData.studentName = "某某某";
        timetableItemData.location = "某某机构";
        timetableItemData.startTime = new DateTime(2025,1,11).ToString();
        timetableItemData.endTime = new DateTime(2025,2,16).ToString();
        timetableItemData.weekday = new List<int>() {2, 4};
        timetableItemData.classNumber = new List<int>() {1, 3};
        
        timetableItems.Add(timetableItemData);
    }
    
    ///更新当前周时间表数据
    public void UpdateTimetableItemData(int row, int column, TimetableItemData timetableItemData)
    {
        if (row < 0 || row >= currWeekTimetableItems.GetLength(0) || column < 0 || column >= currWeekTimetableItems.GetLength(1))
        {
            Debug.LogError("超出时间表范围");
            return;
        }
        currWeekTimetableItems[row,column] = timetableItemData;
        OnTimetableItemDataChanged?.Invoke(row, column, timetableItemData);
    }
        
}
[System.Serializable]
public class TimetableItemData
{
    //学生姓名
    public string studentName;
    //地点
    public string location;
    //开始时间
    public string startTime;
    //结束时间
    public string endTime;
    //星期几
    public List<int> weekday;
    //第几节课
    public List<int> classNumber;
}