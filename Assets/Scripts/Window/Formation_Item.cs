////////////////////////////////////////////////
//
// Formation_Item
//
// 전열창 내부의 List 밑에서 작동하는 스크립트
// 19. 12. 26
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Formation_Item : MonoBehaviour,
    IPointerExitHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {
        NewFormation temp = NewFormation.instance;

        if (temp.bSelectItem && !temp.bIsDraging)
        {
            temp.bIsPointerOut = true;
            temp.bIsDraging = true;

            temp.ItemCloneTF.GetComponent<UnityEngine.UI.Image>().sprite =
                temp.ListTF.GetChild(int.Parse(transform.parent.name)).GetComponentInChildren<UnityEngine.UI.Image>().sprite;

            temp.ActiveBlack();
            temp.ActiveInfoWindow();
        }
    }
}
