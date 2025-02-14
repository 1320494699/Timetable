using UnityEngine;

public class DatePickerExample : MonoBehaviour
{
    public DatePicker DatePicker = null;
    public Calendar Calendar = null;

    public void Awake()
    {
        Calendar.OnDayClick.AddListener(time =>
        {
            Debug.Log(string.Format("Today is {0}/{1}/{2}",
                time.Year, time.Month, time.Day));
        });
        Calendar.OnMonthClick.AddListener(time =>
        {
            Debug.Log(string.Format("This month is {0}/{1}",
                time.Year, time.Month));
        });
        Calendar.OnYearClick.AddListener(time => { Debug.Log(string.Format("This yeah{0}", time.Year)); });
    }
}