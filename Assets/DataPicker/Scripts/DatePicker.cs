using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DatePicker : UIBehaviour
{
    private TextMeshProUGUI _dateText = null;
    private Calendar _calendar = null;
    private DateTime _dateTime = DateTime.Today;

    // 通过这个属性获取日期
    public DateTime DateTime
    {
        get { return _dateTime; }
        set
        {
            _dateTime = value;
            RefreshDateText();
        }
    }

    public string Text
    {
        get { return _dateText.text; }
    }
    protected override void Awake()
    {
        _dateText = transform.Find("DateText").GetComponent<TextMeshProUGUI>();
        _calendar = transform.Find("Calendar").GetComponent<Calendar>();
        _calendar.OnDayClick.AddListener(dateTime => { DateTime = dateTime; });
        transform.Find("PickButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            _calendar.gameObject.SetActive(true);
        });
        RefreshDateText();
    }

    private void RefreshDateText()
    {
        if (_calendar.DisplayType == E_DisplayType.Standard)
        {
            switch (_calendar.CalendarType)
            {
                case E_CalendarType.Day:
                    _dateText.text = DateTime.ToShortDateString();
                    break;
                case E_CalendarType.Month:
                    _dateText.text = DateTime.Year + "/" + DateTime.Month;
                    break;
                case E_CalendarType.Year:
                    _dateText.text = DateTime.Year.ToString();
                    break;
            }
        }
        else
        {
            switch (_calendar.CalendarType)
            {
                case E_CalendarType.Day:
                    _dateText.text = DateTime.Year + "-" + DateTime.Month + "-" + DateTime.Day ;
                    break;
                case E_CalendarType.Month:
                    _dateText.text = DateTime.Year + "-" + DateTime.Month ;
                    break;
                case E_CalendarType.Year:
                    _dateText.text = DateTime.Year.ToString();
                    break;
            }
        }

        _calendar.gameObject.SetActive(false);
    }
}