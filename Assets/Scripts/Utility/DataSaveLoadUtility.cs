using QFramework;
using UnityEngine;

public class DataSaveLoadUtility:IUtility
{
    public void SaveData(string key, object data)
    {
        PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
    }

    public T LoadData<T>(string key) where T : class
    {
        var jsonData = PlayerPrefs.GetString(key);
        if (string.IsNullOrEmpty(jsonData))
        {
            return null;
        }
        return JsonUtility.FromJson<T>(jsonData);
    }
}
