// Generate Id:0fa42911-94b3-4134-9c20-9c380cf83606
using UnityEngine;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class MainPanel : QFramework.IController
	{

		public QFramework.Example.TimeTableContent TimeTableContent;

		public UnityEngine.UI.Button Btn_Add;

		public UnityEngine.UI.Button Month;

		public QFramework.Example.WeekdayContent WeekdayContent;

		public TMPro.TextMeshProUGUI Txt_Month;

		public TMPro.TextMeshProUGUI Txt_Weekday;

		QFramework.IArchitecture QFramework.IBelongToArchitecture.GetArchitecture()=>QFramework.Example.App.Interface;
	}
}
