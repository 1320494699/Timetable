using System;
using DG.Tweening;
using UnityEngine;
using QFramework;
using UnityEngine.EventSystems;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class MenuPanel : ViewController,IPointerClickHandler
	{
		
		void Awake()
		{
			print("菜单面板初始化");
			Btn_DataClear.onClick.AddListener(OnBtnDataClearClick);
			this.RegisterEvent<Event_OpenMenuPanel>(e =>
			{
				Open();
			});
		}

		private void OnBtnDataClearClick()
		{
			var dataSaveLoadUtility = this.GetUtility<DataSaveLoadUtility>();
			dataSaveLoadUtility.ClearData();
			var model = this.GetModel<TimetableModel>();
			model.TimetableData.timetableItems.Clear();
			model.RefreshCurrentWeekTimetableData(DateTime.Today);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Close();
		}

		public void Open()
		{
			gameObject.SetActive(true);
			Content.DOAnchorPosX(0, 0.5f);
		}
		public void Close()
		{
			Content.DOAnchorPosX(-840, 0.5f).OnComplete(() =>
			{
				gameObject.SetActive(false);
			});
		}
	}
}
