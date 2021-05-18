using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class One_day : MonoBehaviour
{

    public DataController dataController;

    public void OnClick()
    {
        //day = day + dayPerClick;
        int dayPerClick = dataController.GetDayPerClick();
        dataController.AddDay(dayPerClick);
    }

}
