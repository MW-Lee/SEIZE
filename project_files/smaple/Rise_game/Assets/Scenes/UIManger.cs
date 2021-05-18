using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : MonoBehaviour
{

    public Text dayDisplayer;

    public Text dayPerClickDisplayer;

    public DataController dataController;

    void Update()
    {
        dayDisplayer.text = "Day:" + dataController.GetDay();
        dayPerClickDisplayer.text = "Day PER CLICK:" + dataController.GetDayPerClick();
    }

}
