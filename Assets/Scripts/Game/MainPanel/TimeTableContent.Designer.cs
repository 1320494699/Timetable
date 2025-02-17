// Generate Id:d1b627dc-ea8c-4667-807b-a65407d43dd0
using UnityEngine;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class TimeTableContent : QFramework.IController
	{

		public QFramework.Example.TimeTableItem TimeTableItem;

		QFramework.IArchitecture QFramework.IBelongToArchitecture.GetArchitecture()=>QFramework.Example.App.Interface;
	}
}
