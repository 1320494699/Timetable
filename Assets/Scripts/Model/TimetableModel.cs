using Newtonsoft.Json;
using QFramework;
using UnityEngine;

public class TimetableModel:AbstractModel
{
    public TimetableData TimetableData { get; set; }
    
    protected override void OnInit()
    {
        //TODO: 先加载本地数据，如果没有，则初始化数据
        
        TimetableData = new TimetableData();
        TimetableData.InitTimetableData(5,7);
        string json = JsonConvert.SerializeObject(TimetableData);
        Debug.Log(json);
    }
}
