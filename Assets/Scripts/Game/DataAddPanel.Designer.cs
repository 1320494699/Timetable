// Generate Id:5add0fa9-31fe-46ff-b42f-ea79a09a81fe
using UnityEngine;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class DataAddPanel : QFramework.IController
	{

		public TMPro.TMP_InputField IF_Name;

		public TMPro.TMP_InputField IF_Price;

		public TMPro.TMP_InputField IF_Grade;

		public UnityEngine.UI.Button Btn_Close;

		public DatePicker DP_EndTime;

		public DatePicker DP_StartTime;

		public QFramework.Example.WeekdaySelectContent WeekdaySelectContent;

		public QFramework.Example.ClassNumberSelectContent ClassNumberSelectContent;

		public UnityEngine.UI.Button Btn_Success;

		public TMPro.TMP_InputField IF_Location;

		public TMPro.TextMeshProUGUI Txt_Message;

		QFramework.IArchitecture QFramework.IBelongToArchitecture.GetArchitecture()=>QFramework.Example.App.Interface;
	}
}
