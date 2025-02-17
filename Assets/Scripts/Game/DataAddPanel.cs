using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using QFramework;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class DataAddPanel : ViewController
	{
		TimetableModel mModel;
		void Awake()
		{
			mModel = this.GetModel<TimetableModel>();
			Btn_Close.onClick.AddListener(Close);
			Btn_Success.onClick.AddListener(Success);
			DP_StartTime.OnDayClick.AddListener(t =>
			{
				DP_EndTime.DateTime = t;
			});
			this.RegisterEvent<Event_OpenDataAddPanel>(e =>
			{
				print("打开数据添加面板");
				Init();
			});
		}

		public void Init()
		{
			this.Show();
			IF_Name.text = "";
			IF_Location.text = "";
			IF_Price.text = "";
			IF_Grade.text = "";
			DP_StartTime.DateTime = DateTime.Now;
			DP_EndTime.DateTime = DateTime.Now;
			WeekdaySelectContent.Init();
			ClassNumberSelectContent.Init();
		}
		private async void Success()
		{
			StudentData data = new StudentData();
			string name =IF_Name.text;
			DateTime startTime = DP_StartTime.DateTime;
			DateTime endTime = DP_EndTime.DateTime;
			string location = IF_Location.text;
			string price = IF_Price.text;
			string grade = IF_Grade.text;
			var weekday=WeekdaySelectContent.weekdays;
			var classNumber=ClassNumberSelectContent.classNumbers;
			if (startTime > endTime)
			{
				ShowMessage("开始时间不能大于结束时间");
				
				return;
			}
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(price) ||
			    string.IsNullOrEmpty(grade) || weekday.Count == 0 || classNumber.Count == 0)
			{
				ShowMessage("输入信息不完整");
				
				return;
			}
			data.name = name;
			data.startTime = startTime;
			data.endTime = endTime;
			data.location = location;
			data.price = float.Parse(price);
			data.grade = int.Parse(grade);
			data.weekday = weekday;
			data.classNumber = classNumber;
			var  existedData = mModel.DecomposeStudentTimetable(data);
			if (mModel.IsTimeConflict(data, existedData))
			{
				ShowMessage("时间冲突");
				
				return;
			}
			foreach (var timetableItemData in existedData)
			{
				mModel.AddTimetableItemData(timetableItemData);
			}
			
			mModel.RefreshCurrentWeekTimetableData(DateTime.Today);
			ShowMessage("添加成功");
			await UniTask.Delay(TimeSpan.FromSeconds(1));
			Close();
		}
		CancellationTokenSource mCancelTokenSource;
		public async void ShowMessage(string message)
		{
			if (mCancelTokenSource != null)
			{
				mCancelTokenSource.Cancel();
				mCancelTokenSource = null;
			}
			Txt_Message.text = message;
			mCancelTokenSource = new CancellationTokenSource();
			await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: mCancelTokenSource.Token);
			Txt_Message.text = "";
		}
		private void Close()	
		{
			this.Hide();
		}
	}
}
