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

			DateTime today = DateTime.Today;
			
			model.RefreshCurrentWeekTimetableData(today);
			WeekdayContent.UpdateDay(today);
			Txt_Month.text = today.Month+"月";
			Txt_Weekday.text = today.GetWeekNumberInMonth()+"周";
			
			var dataSaveLoadUtility = this.GetUtility<DataSaveLoadUtility>();
			
			Btn_Add.onClick.AddListener(() =>
			{
				this.SendCommand(new CMD_OpenDataAddPanel());
			});
			Btn_Menu.onClick.AddListener(() =>
			{
				this.SendCommand(new CMD_OpenMenuPanel());
			});
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidStatusBar.dimmed=false;
				AndroidStatusBar.statusBarState = AndroidStatusBar.States.TranslucentOverContent;
			}
		}
	}
}
