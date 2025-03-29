using System;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class MainPanel : ViewController
	{
		public int currentYear;
		public int currentMonth;
		public int currentWeek;
		public DateTime currentMonday;
		TimetableModel model;
		void Start()
		{
			TimeTableContent.Init();
			WeekdayContent.Init();
			
			model = this.GetModel<TimetableModel>();

			DateTime today = DateTime.Today;
			currentYear = today.Year;
			currentMonth = today.Month;
			currentWeek = today.GetWeekNumberInMonth();
			
			RefreshData(today);
			
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
			EnhancedTouchSupport.Enable();
			
		}
		
		[SerializeField] float minSwipeDistance = 200; // 最小滑动距离  
		private Vector2 mouseStartPos; 
		private Vector2 startPos;  
		private Vector2 endPos;  
  
		void Update() {
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				
                if (Touch.activeFingers.Count > 0)
                {
					Touch activeTouch = Touch.activeTouches[0];

                    if (activeTouch.phase == TouchPhase.Began)
                    {
                        startPos = activeTouch.screenPosition;
                    }
                    else if (activeTouch.phase == TouchPhase.Ended)
                    {
                        endPos = activeTouch.screenPosition;
                        Vector2 offset = endPos - startPos;
                        CheckSwipeDirection(offset.x);
                    }
                }
                //print(Input.touchCount);
                //            if (Input.touchCount > 0)
                //            {
                //                Touch touch = Input.GetTouch(0);
                //                if (touch.phase == UnityEngine.TouchPhase.Began)
                //                {
                //                    startPos = touch.position;
                //                }
                //                else if(touch.phase==UnityEngine.TouchPhase.Ended)
                //                {
                //                    endPos = touch.position;
                //                    Vector2 offset = endPos - startPos;
                //                    CheckSwipeDirection(offset.x);  
                //	}
                //}
            }
            else
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    mouseStartPos = Mouse.current.position.ReadValue();
                }

                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    Vector2 offset = Mouse.current.position.ReadValue() - mouseStartPos;
                    CheckSwipeDirection(offset.x);

                }
            }
        }  
  
		void CheckSwipeDirection(float deltaX) {  
			if (Mathf.Abs(deltaX) > minSwipeDistance) {  
				if (deltaX > 0) {  
					Debug.Log("右滑"); // 
					SubWeekNumber();
				} else {  
					Debug.Log("左滑");  
					AddWeekNumber();  
				}  
			}  
		}

		public void RefreshData(DateTime today)
		{
			model.RefreshCurrentWeekTimetableData(today);
			WeekdayContent.UpdateDay(today);
			
			Txt_Month.text = currentMonth+"月";
			Txt_Weekday.text = currentWeek+"周";
			
			currentMonday = DateTimeExtensions.GetMondayOfWeek(currentYear, currentMonth, currentWeek);
			print("currentMonday:" + currentMonday);
			//currentMonth=currentMonday.AddDays(3).Month;
			
			//          
		}
		/// <summary>
		/// 添加周数
		/// </summary>
		public void AddWeekNumber()
		{
			int totalWeek = DateTimeExtensions.GetWeeksInMonth(currentYear, currentMonth);
			print("totalWeek:" + totalWeek);
			print("currentWeek:" + currentWeek);
			if (currentWeek < totalWeek)
			{
				currentWeek++;
				print("currentWeek:" + currentWeek);
			}
			else
			{
				currentMonth++;
				if (currentMonth > 12)
				{
					currentYear++;
					currentMonth = 1;
				}
				currentWeek = 1;
			}
			currentMonday = DateTimeExtensions.GetMondayOfWeek(currentYear, currentMonth, currentWeek);
			RefreshData(currentMonday);
		}
		/// <summary>
		/// 减少周数
		/// </summary>
		public void SubWeekNumber()
		{
			
			if (currentWeek > 1)
			{
				currentWeek--;
			}
			else
			{
				currentMonth--;
				if (currentMonth < 1)
				{
					currentYear--;
					currentMonth = 12;
				}
				int totalWeek = DateTimeExtensions.GetWeeksInMonth(currentYear, currentMonth);
				currentWeek = totalWeek;
			}
			currentMonday = DateTimeExtensions.GetMondayOfWeek(currentYear, currentMonth, currentWeek);
			RefreshData(currentMonday);
		}
	}
}
