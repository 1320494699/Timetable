using System;
using UnityEngine;
using QFramework;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class TimeTableContent : ViewController
	{
		private TimeTableItem[,] mTimeTableItems;
		
		public void Init()
		{
			var timeTable = this.GetModel<TimetableModel>();
			GenerateTable(timeTable.TimetableData.currWeekTimetableItems);
			timeTable.TimetableData.OnTimetableItemDataChanged+=OnTimetableItemDataChanged;
			
			DateTime dt = DateTime.Today;  //当前时间  
			DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一  
			DateTime endWeek = startWeek.AddDays(6);  //本周周日
        
			
			foreach (var timetableItemData in timeTable.TimetableData.timetableItems)
			{
				DateTimeRange timeRange1 = new DateTimeRange(timetableItemData.startTime.ToDateTime(), timetableItemData.endTime.ToDateTime());
				DateTimeRange timeRange2 = new DateTimeRange(startWeek, endWeek);

				DateTimeRange? overlapTimeRange = timeRange1.GetOverlap(timeRange2);
				if (overlapTimeRange.HasValue)
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

					print("当前周时间有重叠，添加到课表");
					//计算重叠开始时 是当前周的周几
					int overlapStartWeekday = weekdayAdjust((int)overlapTimeRange.Value.Start.DayOfWeek);
					//计算重叠结束时 是当前周的周几
					int overlapEndWeekday =weekdayAdjust((int)overlapTimeRange.Value.End.DayOfWeek);
				
					print("overlapStartWeekday: "+overlapStartWeekday);
					print("overlapEndWeekday: "+overlapEndWeekday);
					foreach (var weekday in timetableItemData.weekday)
					{
						foreach (var classNumber in timetableItemData.classNumber)
						{
							if (overlapEndWeekday >= weekday - 1 && overlapStartWeekday <= weekday - 1)
							{
								timeTable.TimetableData.UpdateTimetableItemData(classNumber - 1,
									weekday - 1 , timetableItemData);
							} 
						}
					}
					
				}
				else
				{
					print("当前周时间无重叠");
				}
			}
		}

		private void OnTimetableItemDataChanged(int row, int col, TimetableItemData timetableItemData)
		{
			//更新表格
			GetItem(row, col).Init(timetableItemData);
		}

		//初始化生成表格
		public void GenerateTable(TimetableItemData[,] timetableItemDatas)
		{
			int rowCount = timetableItemDatas.GetLength(0);
			print("rowCount:" + rowCount);
			int columnCount = timetableItemDatas.GetLength(1);
			mTimeTableItems = new TimeTableItem[rowCount, columnCount];
			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < columnCount; j++)
				{
					Instantiate(this.TimeTableItem)
						.Identity()
						.Parent(this)
						.Self(self =>
						{
							mTimeTableItems[i,j] = self;
							self.name = "TimeTableItem_" + i + "_" + j;
							self.Init(timetableItemDatas[i,j]);
						})
						.Show();
					
				}
			}
			TimeTableItem.DestroyGameObj();
		}
		
		//获取某行某列的Item
		public TimeTableItem GetItem(int row, int column)
		{
			if (row < 0 || row >= mTimeTableItems.GetLength(0) || column < 0 || column >= mTimeTableItems.GetLength(1))
			{
				Debug.LogError("GetItem: Index out of range");
				return null;
			}
			return mTimeTableItems[row,column];
		}
	}
}
