////////////////////////////////////////////////
//
// FormationWindow_Img
//
// 전열 창의 이미지들이 갖는 스크립트
// 19. 07. 18
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FormationWindow_Img : MonoBehaviour, IPointerDownHandler
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    public Image Img;



    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //   
    public void OnPointerDown(PointerEventData eventData)
    {
        if (FormationWindow.instance.IsSelectSoldier)
        {
            FormationWindow.instance.iSelectFormationImgNum = int.Parse(name);
            FormationWindow.instance.gInputNumWindow.SetActive(true);

            InputManager.instance.WActiveWindow = WindowName.ProductSelectWindow;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //   
    private void Awake()
    {
        Img = GetComponent<Image>();
    }
}
