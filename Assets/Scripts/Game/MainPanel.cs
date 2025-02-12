using System;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class MainPanel : ViewController
	{
		void Start()
		{
			TimeTableContent.Init();

			var model = this.GetModel<TimetableModel>();

			var dataSaveLoadUtility = this.GetUtility<DataSaveLoadUtility>();
			
			//添加测试数据
			// StudentData studentData = new StudentData();
			// studentData.name = "某某某";
			// studentData.location = "某某机构";
			// studentData.grade = 8;
			// studentData.startTime = new DateTime(2025,1,11);
			// studentData.endTime = new DateTime(2025,2,12);
			// studentData.weekday = new List<int>() {2, 4};
			// studentData.classNumber = new List<int>() {1, 3};
   //      
			// DateTime startTime = studentData.startTime;
			// DateTime endTime = studentData.endTime;
			// for (int i = 0; i < studentData.weekday.Count; i++)
			// {
			// 	int weekday = studentData.weekday[i];
			// 	DayOfWeek dayOfWeek = (DayOfWeek)weekday;
			// 	DateTime[] dates = startTime.GetSpecificWeekdayDates(endTime,dayOfWeek);
			// 	for (int j = 0; j < studentData.classNumber.Count; j++)
			// 	{
			// 		foreach (var date in dates)
			// 		{
			// 			TimetableItemData timetableItemData = new TimetableItemData();
			// 			timetableItemData.studentData = studentData;
			// 			timetableItemData.time = date;
			// 			timetableItemData.classNumber = studentData.classNumber[j];
			// 			model.TimetableData.timetableItems.Add(timetableItemData);
			// 			Debug.Log($"{studentData.name} {date.ToString("yyyy-MM-dd")} 第{studentData.classNumber[j]}节课");
			// 			dataSaveLoadUtility.SaveData(timetableItemData);
			// 		}
			// 	}
   //          
			// }
			
			
			//model.RefreshCurrentWeekTimetable();
		}
	}
}
