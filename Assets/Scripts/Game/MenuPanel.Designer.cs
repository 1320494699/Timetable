// Generate Id:9db80efe-b538-4f8d-b173-d49a493e8ad7
using UnityEngine;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class MenuPanel : QFramework.IController
	{

		public UnityEngine.UI.Button Btn_DataClear;

		public UnityEngine.RectTransform Content;

		QFramework.IArchitecture QFramework.IBelongToArchitecture.GetArchitecture()=>QFramework.Example.App.Interface;
	}
}
