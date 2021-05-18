////////////////////////////////////////////////
//
// Formation_Info_Item
//
// 아이템 상세정보 창에서 작동하는 스크립트
// 20. 01. 23
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Formation_Info_Item : MonoBehaviour
{
    #region 변수

    public Image Img;
    public Text tName;
    public Text tInfo;
    public Text tAbility;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void OnEnable()
    {
        // Item temp = 선택한 아이템

        //Img.sprite = temp.iSoldierImg;
        //tName.text = temp.sSoldierName;
        //tInfo.text = temp.sSoldierInfo;
        // 효과 갱신 필요
    }

    #endregion
}

