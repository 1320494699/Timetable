using QFramework;
using UnityEngine;

namespace QFramework.Example
{
    public class CMD_OpenMenuPanel : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent(new Event_OpenMenuPanel());
        }
    }
}