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
			//添加监听
			timeTable.TimetableData.OnTimetableItemDataChanged+=OnTimetableItemDataChanged;
			
			timeTable.RefreshCurrentWeekTimetableData();
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
						.Parent(this)
						.Identity()
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
