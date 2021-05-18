////////////////////////////////////////////////
//
// Button
//
// 맵 화면에 직접적으로 보이는 버튼들에서 작동하는 스크립트
// 19. 07. 22
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HELP : MonoBehaviour
{
    public GameObject gLogWindow;

    public void OnClick()
    {
        InputManager.instance.ShowTutImg.sprite = InputManager.instance.TutImg[InputManager.instance.TutRate];
        InputManager.instance.ShowTutImg.gameObject.SetActive(true);
        InputManager.instance.bIsTut = true;
    }

    public void Button_LOG()
    {
        //StartCoroutine(LogWindow.instance.StartLogMove());
        StartCoroutine(Database.StartWindowMove(gLogWindow.transform, 0));
        LogWindow.instance.IsLogWindowShow = true;
    }
}
