////////////////////////////////////////////////
//
// SimpleWindow
//
// 간단한 확인 취소 창을 위한 스크립트
// 19. 06. 13
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SimpleWindow를 어떻게 사용할 것인지 상태를 나타내는 열거형 변수
/// </summary>
public enum UseSimpleWindow
{
    TurnEnd,
    CancelOrder,
    ApplyChange,

    Max
}

public class SimpleWindow : MonoBehaviour
{
    #region 변수

    /// <summary>
    /// SimpleWindow를 외부에서 사용하기 위한 instance
    /// </summary>
    public static SimpleWindow instance;
    /// <summary>
    /// 창에 나타나는 메세지
    /// </summary>
    public Text tMainText;

    public Material MTRoad;


    #endregion


    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    /// <summary>
    /// OK 버튼을 누를 때 작동하는 함수
    /// </summary>
    public void Btn_OK()
    {
        switch (InputManager.instance.eHowToUse)
        {
            case UseSimpleWindow.TurnEnd:
                TurnManager.instance.StartAction();
                break;


            case UseSimpleWindow.CancelOrder:
                FindOrder(InputManager.instance.gSelectNode.lArmy[InputManager.instance.iSelectSpace]);
                break;

            case UseSimpleWindow.ApplyChange:                
                NewFormation.instance.RefreshArmies();
                NewFormation.instance.btn_Confirm.interactable = false;
                break;
        }

        Btn_Cancel();
    }

    /// <summary>
    /// Cancel 버튼을 누를 때 작동하는 함수
    /// </summary>
    public void Btn_Cancel()
    {
        switch (InputManager.instance.eHowToUse)
        {
            case UseSimpleWindow.CancelOrder:
                InputManager.instance.WActiveWindow = WindowName.NodeWindow;
                InputManager.instance.iSelectSpace = 0;
                break;

            case UseSimpleWindow.ApplyChange:                
                if (Up_UI.instance.bCallByBack)
                {
                    InputManager.instance.WActiveWindow = WindowName.Empty;
                    NewFormation.instance.gameObject.SetActive(false);
                    Up_UI.instance.bCallByBack = false;
                }
                else if(NewFormation.instance.bCallbyForm)
                {
                    InputManager.instance.WActiveWindow = WindowName.FormationWindow;
                    NewFormation.instance.AfterFormationButton();
                    NewFormation.instance.bCallbyForm = false;
                }
                else
                {
                    InputManager.instance.WActiveWindow = WindowName.FormationWindow;
                }
                break;
        }

        gameObject.SetActive(false);
    }

    /// <summary>
    /// 창에 나타나는 메세지를 갱신해주는 함수
    /// </summary>
    public void SettingWindow()
    {
        switch (InputManager.instance.eHowToUse)
        {
            case UseSimpleWindow.TurnEnd:
                tMainText.fontSize = 63;
                tMainText.text = "턴을 종료하겠습니까?";
                break;

            case UseSimpleWindow.CancelOrder:
                tMainText.fontSize = 55;
                tMainText.text = "행동 대기중인 군대입니다\n명령을 취소하겠습니까?";
                break;

            case UseSimpleWindow.ApplyChange:
                tMainText.fontSize = 45;
                tMainText.text = "전열에 변경사항이 있습니다\n변경사항을 저장하겠습니까?";
                break;
        }
    }

    /// <summary>
    /// 명령 취소를 실행했을 때 작동하는 함수
    /// </summary>
    /// <param name="_inputArmies">명령을 취소시킬 군대</param>
    public void FindOrder(Army[] _inputArmies)
    {
        // 명령 대기리스트를 임시로 받아옴 (for문에서 계속 호출하는건 비효율적이라 판단)
        var temp = TurnManager.lTurnend;
        // 찾아낸 군대가 몇 번째 칸에 있는지 확인
        int result = -1;

        // 해당 군대와 맞는 명령을 찾음 >> 한 군대는 하나의 명령만 할 수 있는점에 착안
        for(int i = 0; i < temp.Count; i++)
        {
            if(temp[i].lMyArmy == _inputArmies)
            {
                result = i;
                break;
            }
        }

        // 명령을 취소시키고 군을 대기상태로 변경, 노드창을 갱신시킴
        //ActionManager.instance.lTurnend.RemoveAt(result);
        TurnManager.lTurnend[result].FindLine().GetComponent<LineRenderer>().material = MTRoad;
        TurnManager.lTurnend.RemoveAt(result);
        InputManager.instance.gSelectNode.bIsItArmy[InputManager.instance.iSelectSpace] = ArmyState.Stay;
        NodeWindow.instance.SettingWindow();
        
        return;
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Awake()
    {
        if (instance != this)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        SettingWindow();
    }

    private void OnDisable()
    {
        tMainText.text = string.Empty;
    }

    #endregion
}
