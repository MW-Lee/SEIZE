////////////////////////////////////////////////
//
// Formation_Info_Com
//
// 지휘관 상세정보 창에서 작동하는 스크립트
// 20. 01. 23
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Formation_Info_Com : MonoBehaviour
{
    #region 변수

    public Image Img;
    public Text tName;
    public Text tSubName;
    public Transform StatTF;
    public Transform EquipTF;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void OnEnable()
    {
        Commander temp = new Commander();

        if (NewFormation.instance.bSelectItem)
            temp = DataManager.instance.lComList[NewFormation.instance.iSelectItem];
        else if (NewFormation.instance.bSelectSlot)
            temp = NewFormation.instance.ChangingArmies[NewFormation.instance.iSelectItem].commander;

        Img.sprite = temp.sCommanderImgBig;
        tName.text = temp.sCommanderName;
        tSubName.text = temp.sCommanderInfo;
        // Stat 관련 갱신 필요
        // Equip 관련 갱신 필요
    }

    private void OnDisable()
    {
        Img.sprite = null;
        tName.text = "Name";
        tSubName.text = "SubName";
    }

    #endregion
}
