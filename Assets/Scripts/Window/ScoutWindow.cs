////////////////////////////////////////////////
//
// ScoutWindow
//
// 탐색 확인 창에서 작동하는 스크립트
// 19. 08. 07
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoutWindow : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// ScoutWindow를 사용하기 위한 instance
    /// </summary>
    public static ScoutWindow instance;
    /// <summary>
    /// 탐색시 소모되는 골드 > 지휘관의 탐색 스킬 LV마다 골드양이 달라야한다
    /// </summary>
    public Text UseGold;


    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// 탐색 창에서 '예' 버튼이 눌렸을 때 작동되는 함수
    /// </summary>
    public void Button_OK()
    {
        // 골드가 차감되고
        //TurnManager.instance.currentPlayer.coast -= int.Parse(UseGold.text);
        TurnManager.instance.currentPlayer.coast -= 500;

        // 선택된 노드 중심 근처노드 확인
        for (int i = 0; i < InputManager.instance.gSelectNode.lNearNodes.Count; i++)
        {
            InputManager.instance.gSelectNode.lNearNodes[i].bIsScout = true;
        }

        Order ScoutOrder = new Order(OrderNum.Scout, InputManager.instance.gSelectNode);
        DataManager.instance.Notify(ScoutOrder);

        InputManager.instance.StaticFadePopup("탐색 완료");

        Button_Cancel();
    }

    /// <summary>
    /// 탐색 창에서 '아니오' 버튼이 눌렸을 때 작동되는 함수
    /// </summary>
    public void Button_Cancel()
    {
        if (InputManager.instance.gNodeWindow.activeSelf)
            InputManager.instance.gNodeWindow.SetActive(false);
        InputManager.instance.WActiveWindow = WindowName.Empty;

        gameObject.SetActive(false);
    }


    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
}
