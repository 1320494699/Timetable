using Newtonsoft.Json;
using QFramework;
using UnityEngine;

public class TimetableModel:AbstractModel
{
    public TimetableData TimetableData { get; set; }
    
    protected override void OnInit()
    {
        //TODO: 先加载本地数据，如果没有，则初始化数据
        var load = this.GetUtility<DataSaveLoadUtility>();
        TimetableData = load.LoadData<TimetableData>("");
        if (TimetableData == null)
        {
            TimetableData = new TimetableData();
            TimetableData.InitTimetableData(5, 7);
        }
    }
}
