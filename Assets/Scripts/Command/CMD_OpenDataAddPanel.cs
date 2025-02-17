using QFramework;
using UnityEngine;

namespace QFramework.Example
{
    public class CMD_OpenDataAddPanel : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent(new Event_OpenDataAddPanel());
        }
    }
}