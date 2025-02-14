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
			WeekdayContent.Init();
			
			var model = this.GetModel<TimetableModel>();

			DateTime today = new DateTime(2025, 2, 12);
			model.RefreshCurrentWeekTimetableData(today);
			WeekdayContent.UpdateDay(today);
			Txt_Month.text = today.Month+"月";
			Txt_Weekday.text = GetWeekNumberInMonth(today)+"周";
			var dataSaveLoadUtility = this.GetUtility<DataSaveLoadUtility>();
			
			//model.RefreshCurrentWeekTimetable();
		}
		
		private int GetWeekNumberInMonth(DateTime date)
		{
			// 获取本月第一天
			DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);

			// 计算本月第一天是星期几（Monday 为 1，Sunday 为 7）
			int weekdayOfFirstDay = (int)firstDayOfMonth.DayOfWeek;
			if (weekdayOfFirstDay == 0)
			{
				weekdayOfFirstDay = 7; // 将 Sunday 从 0 调整为 7
			}

			// 如果本月第一天所在周的天数少于 4 天，则跳过该周
			int offset = 0;
			if (weekdayOfFirstDay > 4) // 即星期五（5）及以后开始的第一周跳过
			{
				offset = 8 - weekdayOfFirstDay;
			}

			// 计算从本月第一天到当前日期的天数
			int daysSinceFirstDay = (date - firstDayOfMonth).Days;

			// 计算当前是本月的第几周
			return (daysSinceFirstDay + weekdayOfFirstDay - 1 - offset) / 7 + 1;
		}
	}
}
