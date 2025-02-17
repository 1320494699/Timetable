using QFramework;
using UnityEngine;

namespace QFramework.Example
{
    public class App : Architecture<App>
    {
        protected override void Init()
        {
            // 注册所有的 Module
            RegisterModel(new TimetableModel());

            //注册所有的Utility
            RegisterUtility(new DataSaveLoadUtility());
        }
    }
}