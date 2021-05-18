using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{

    private int m_day = 0;
    private int m_dayPerClick = 0;

    private void Awake()
    {
        //KEY : VALUE
        m_day = PlayerPrefs.GetInt("Day");
        m_dayPerClick = PlayerPrefs.GetInt("DayPerClick", 1);
    }
    public void SetDay(int newDay)
    {
        m_day = newDay;
        PlayerPrefs.SetInt("Day", m_day);
    }
    public void AddDay(int newDay)
    {
        m_day += newDay;
        SetDay(m_day);
    }
    public void SubDay(int newDay)
    {
        m_day -= newDay;
        SetDay(m_day);
    }

    public int GetDay()
    {
        return m_day;
    }
    public int GetDayPerClick()
    {
        return m_dayPerClick;
    }

    public void SetDayPerClick(int newDayPerClick)
    {
        m_dayPerClick = newDayPerClick;
        PlayerPrefs.SetInt("DayPerClick", m_dayPerClick);
    }
}
