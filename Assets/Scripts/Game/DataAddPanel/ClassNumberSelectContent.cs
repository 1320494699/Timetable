using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class ClassNumberSelectContent : ViewController
	{
		public Toggle[] toggles;
		public List<int> classNumbers =new List<int>();
		void Start()
		{
			for (int i = 0; i < toggles.Length; i++)
			{
				toggles[i].onValueChanged.AddListener((bool value) =>
				{
					if (value)
					{
						classNumbers.Add(i);
					}
					else
					{
						classNumbers.Remove(i);
					}
				});
			}
		}

		public void Init()
		{
			classNumbers.Clear();
			for (int i = 0; i < toggles.Length; i++)
			{
				toggles[i].isOn = false;
			}
		}
	}
}
