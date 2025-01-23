using System;
using QFramework;
using UnityEngine;

public class TimetableData
{
    public event Action<int, int, TimetableItemData> OnTimetableItemDataChanged;
    public TimetableItemData[,] timetableItems;
    
    public void InitTimetableData(int row, int column)
    {
        timetableItems = new TimetableItemData[row, column];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                timetableItems[i,j] = new TimetableItemData();
            }
        }
    }
    
    //更新时间表数据
    public void UpdateTimetableItemData(int row, int column, TimetableItemData timetableItemData)
    {
        if (row < 0 || row >= timetableItems.GetLength(0) || column < 0 || column >= timetableItems.GetLength(1))
        {
            Debug.LogError("超出时间表范围");
            return;
        }
        timetableItems[row,column] = timetableItemData;
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
    //时间
    public string time;
    //星期几
    public string weekday;
    //第几节课
    public string classNumber;
}