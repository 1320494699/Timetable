using System;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

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
		[SerializeField] float minSwipeDistance = 200; // 最小滑动距离  
		private Vector2 mouseStartPos; 
		private Vector2 startPos;  
		private Vector2 endPos;  
  
		void Update() {
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				if (Input.touchCount > 0) {  
					Touch touch = Input.GetTouch(0);  
              
					if (touch.phase == TouchPhase.Began) {  
						startPos = touch.position;  
					}  
					else if (touch.phase == TouchPhase.Ended) {  
						endPos = touch.position;  
						CheckSwipeDirection();  
					}  
				}  
			}
			else
			{
				if (Mouse.current.leftButton.wasPressedThisFrame) {  
					mouseStartPos = Mouse.current.position.ReadValue();  
				}  
      
				if (Mouse.current.leftButton.wasReleasedThisFrame) {  
					Vector2 offset = Mouse.current.position.ReadValue() - mouseStartPos;  
					if (Mathf.Abs(offset.x) > minSwipeDistance) {  
						Debug.Log(offset.x > 0 ? "右滑" : "左滑"); // <xinliu type="COORDINATORS" id="13" />   
					}  
				}  
			}
			
			
		}  
  
		void CheckSwipeDirection() {  
			float deltaX = endPos.x - startPos.x;  
          
			if (Mathf.Abs(deltaX) > minSwipeDistance) {  
				if (deltaX > 0) {  
					Debug.Log("右滑"); // <xinliu type="COORDINATORS" id="2,13" />   
				} else {  
					Debug.Log("左滑");  
				}  
			}  
		}  
	}
}
