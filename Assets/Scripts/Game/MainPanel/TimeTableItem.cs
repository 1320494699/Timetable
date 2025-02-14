using System;
using UnityEngine;
using QFramework;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class TimeTableItem : ViewController,IPointerDownHandler,IPointerUpHandler
	{
		public UnityEvent<TimetableItemData> OnLongPressEvent = new UnityEvent<TimetableItemData>();
		TimetableItemData mTimetableItemData;
		private int mWeekday;
		private int mClassNumber;
		public void Init(int weekday,int classNumber)
		{
			mWeekday = weekday;
			mClassNumber = classNumber;
		}
		
		public void UpdateTimeTableItemData(TimetableItemData timetableItemData)
		{
			mTimetableItemData = timetableItemData;
			Txt_studentName.text = timetableItemData.studentData.name;
			Txt_location.text= timetableItemData.studentData.location;
		}

		private float mLongPressTime;
		private void Update()
		{
			if (mIsPressed)
			{
				mLongPressTime += Time.deltaTime;
				if (mLongPressTime > 1)
				{
					mLongPressTime = 0;
					OnLongPressEvent.Invoke(mTimetableItemData);
				}
			}
		}

		private bool mIsPressed;
		public void OnPointerDown(PointerEventData eventData)
		{
			mIsPressed = true;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			mIsPressed = false;
		}
	}
}
