////////////////////////////////////////////////
//
// Formation_Slot
//
// 전열창 내부의 ArmySlot에서 작동하는 스크립트
// 20. 01. 07
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Formation_Slot : MonoBehaviour,
     IPointerEnterHandler, IPointerExitHandler
{
    #region 변수

    public UnityEngine.UI.Outline outline;
    private UnityEngine.UI.Image image;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (NewFormation.instance.bIsDraging)
            outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        NewFormation temp = NewFormation.instance;

        if (temp.bIsDraging)
            outline.enabled = false;
        else if (temp.bSelectSlot
           &&
           !temp.bIsDraging
           )
        {
            temp.bIsPointerOut = true;
            temp.bIsDraging = true;

            temp.ItemCloneTF.GetComponent<UnityEngine.UI.Image>().sprite =
                image.sprite;
            image.color = new Color(1, 1, 1, .3f);
            outline.enabled = false;

            temp.ActiveBlack();
            temp.ActiveInfoWindow();
        }
    }



    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Start()
    {
        outline = GetComponent<UnityEngine.UI.Outline>();
        image = GetComponent<UnityEngine.UI.Image>();
    }

    #endregion
}
