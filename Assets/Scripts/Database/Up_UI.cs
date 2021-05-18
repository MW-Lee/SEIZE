////////////////////////////////////////////////
//
// Up_UI
//
// 위에 상시 존재하는 UI가 가지는 스크립트
// 19. 07. 19
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Up_UI : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    public static Up_UI instance;

    public Text DayCount;
    public Text GoldCount;

    public bool bCallByBack;

    public GameObject MyArmyInfo;
    public GameObject EnmArmyInfo;

    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// 위에 있는 UI에서 작동하는 스크립트
    /// </summary>
    public void Button_Back()
    {
        switch (InputManager.instance.WActiveWindow)
        {
            case WindowName.NodeWindow:
                InputManager.instance.gNodeWindow.SetActive(false);
                break;

            case WindowName.FormationWindow:
                if (NewFormation.instance.bIsDraging)
                    return;
                else if(NewFormation.instance.bIsChange)
                {
                    InputManager.instance.UseSimpleWindow(UseSimpleWindow.ApplyChange);
                    bCallByBack = true;
                }
                else
                {
                    InputManager.instance.gFormationWindow.SetActive(false);
                }
                break;

            case WindowName.ScoutWindow:
                NodeWindow.instance.ScoutWindow.SetActive(false);
                break;

            case WindowName.ProductionWindow:
                InputManager.instance.gNodeWindow.SetActive(false);
                InputManager.instance.BG_Black.SetActive(false);
                NodeWindow.instance.gPWindow.SetActive(false);
                break;

            case WindowName.ProductSelectWindow:
                ProductionSelectWindow.instance.Button_Cancel();
                break;

            case WindowName.AttackMove:
                InputManager.instance.BG_Black.SetActive(false);

                if (InputManager.instance.bAttackCommand)
                {
                    for (int i = 0; i < InputManager.instance.gSelectNode.lNearNodes.Count; i++)
                    {
                        InputManager.instance.gSelectNode.lNearNodes[i].GetComponent<SpriteRenderer>().sortingLayerName = "MapNode";
                    }

                    InputManager.instance.bAttackCommand = false;

                    if (MyArmyInfo.GetComponent<ArmyInfoWindow>().IsActive)
                        MyArmyInfo.GetComponent<ArmyInfoWindow>().Button_Active();
                    if (EnmArmyInfo.GetComponent<ArmyInfoWindow>().IsActive)
                        EnmArmyInfo.GetComponent<ArmyInfoWindow>().Button_Active();
                }
                else
                {
                    for (int i = 0; i < TurnManager.instance.currentPlayer.playerMapnodeList.Count; i++)
                    {
                        TurnManager.instance.currentPlayer.playerMapnodeList[i].GetComponent<SpriteRenderer>().sortingLayerName
                            = "MapNode";
                    }
                    InputManager.instance.bMoveCommand = false;
                    if (MyArmyInfo.GetComponent<ArmyInfoWindow>().IsActive)
                        MyArmyInfo.GetComponent<ArmyInfoWindow>().Button_Active();
                }

               
                break;

            case WindowName.ArmyInfoWindow:
                MyArmyInfo.GetComponent<ArmyInfoWindow>().Button_Active();
                EnmArmyInfo.GetComponent<ArmyInfoWindow>().Button_Active();
                break;
                

            case WindowName.Empty:
                //Application.Quit();
                break;

            default:
                break;
        }

        InputManager.instance.WActiveWindow = WindowName.Empty;
    }

    public void RefreshDayCount()
    {
        int day = DataManager.instance.iDay;

        if (day < 10)
            DayCount.text = "00" + day.ToString();
        else if (day >= 10 && day < 100)
            DayCount.text = "0" + day.ToString();
        else
            DayCount.text = day.ToString();
    }


    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
}
