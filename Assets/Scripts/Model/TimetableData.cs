using System;
using System.Collections.Generic;
using QFramework;
using SimpleSQL;
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
public struct TimetableItemData
{
    //学生信息
    public StudentData studentData;
    //时间
    public DateTime time;
    //第几节课
    public int classNumber;
}
[System.Serializable]
public struct StudentData
{
    //姓名
    public string name;
    //年级
    public int grade;
    //时间段
    public DateTime startTime;
    public DateTime endTime;
    //地点
    public string location;
    //星期几
    public List<int> weekday;
    //第几节课
    public List<int> classNumber;
}