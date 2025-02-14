using System;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using TMPro;
using UnityEngine.UI;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class WeekdayContent : ViewController
	{
		public List<Image> WeekdayImages = new List<Image>();
		public void Init()
		{
			
		}

		public void UpdateDay(DateTime dateTime)
		{
			List<DateTime> dates = GetCurrentWeekDates(dateTime);
			for (int i = 0; i < dates.Count; i++)
			{
				if (dates[i] == DateTime.Today)
				{
					WeekdayImages[i].color = Color.green;
				}
				else
				{
					WeekdayImages[i].color = Color.white;
				}
				WeekdayImages[i].transform.Find("Txt_Day").GetComponent<TextMeshProUGUI>().text = dates[i].Day.ToString();
			}
		}
		private List<DateTime> GetCurrentWeekDates(DateTime now)
		{
			
			DateTime startOfWeek;

			int offset = (int)now.DayOfWeek - (int)DayOfWeek.Monday;
			if (offset < 0)
			{
				offset += 7;
			}
			startOfWeek = now.AddDays(-offset);
			

			List<DateTime> dates = new List<DateTime>();
			for (int i = 0; i < 7; i++)
			{
				dates.Add(startOfWeek.AddDays(i));
			}
			return dates;
		}
	}
}
