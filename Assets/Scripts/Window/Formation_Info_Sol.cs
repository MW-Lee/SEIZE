////////////////////////////////////////////////
//
// Formation_Info_Sol
//
// 병사 상세정보 창에서 작동하는 스크립트
// 20. 01. 23
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Formation_Info_Sol : MonoBehaviour
{
    #region 변수

    public Image Img;
    public Text tName;
    public Text tSubName;
    public Transform StatTF;
    public Text tInfo;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void OnEnable()
    {
        Soldiers temp = new Soldiers();

        if (NewFormation.instance.bSelectItem)
            temp = Database.instance.lSoldierList[NewFormation.instance.iSelectItem];
        else if (NewFormation.instance.bSelectSlot)
            temp = NewFormation.instance.ChangingArmies[NewFormation.instance.iSelectItem].soldier;

        Img.sprite = temp.iSoldierImgBig;
        tName.text = temp.sSoldierName;
        tSubName.text = temp.sSoldierInfo;
        // Stat 관련 갱신 필요
    }

    #endregion
}