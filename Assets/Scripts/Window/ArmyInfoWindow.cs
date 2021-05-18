////////////////////////////////////////////////
//
// ArmyInfoWindow
//
// 선택한 노드의 주둔중인 병력의 정보를 보는 창에서 작동하는 스크립트
// 
// 19. 10. 17
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyInfoWindow : MonoBehaviour
{
    #region 변수

    public Transform TotalTF;
    public Transform ContentTF;
    public Text TotalPower;

    public bool IsActive = false;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    public void Button_Active()
    {
        if (IsActive)
        {
            StartCoroutine(Database.StartWindowMove(TotalTF, -1400));
        }
        else
        {
            if (name == "MyArmy" && InputManager.instance.gSelectMyNode == null)
            {
                ContentTF.gameObject.SetActive(false);
            }
            else if(name == "EnemyArmy" && InputManager.instance.gSelectEnemyNode == null)
            {
                ContentTF.gameObject.SetActive(false);
            }
            else
            {
                ContentTF.gameObject.SetActive(true);
                if (name == "MyArmy")
                {
                    RefreshWindow(
                      InputManager.instance.gSelectNode.lArmy[InputManager.instance.iSelectSpace],
                      InputManager.instance.gSelectNode.bIsScout
                      );
                }
                else if(name == "EnemyArmy")
                {
                    RefreshWindow(
                      InputManager.instance.gSelectNode.lArmy[0],
                      InputManager.instance.gSelectNode.bIsScout
                      );
                }
            }

            StartCoroutine(Database.StartWindowMove(TotalTF, 0));
        }

        IsActive ^= true;
    }

    public void RefreshWindow(Army[] _inputArmies, bool IsScout)
    {
        switch (name)
        {
            case "MyArmy":
                if (InputManager.instance.gSelectNode.iAffiliation !=
                    TurnManager.instance.currentPlayer.playerCountry)
                    return;
                break;

            case "EnemyArmy":
                if (InputManager.instance.gSelectNode.iAffiliation ==
                    TurnManager.instance.currentPlayer.playerCountry)
                    return;
                break;

            default:
                break;
        }

        if (IsScout)
        {
            RefreshContent(ContentTF, _inputArmies);
        }
        else
        {
            RefreshUnknown(ContentTF);
        }

        
        return;
    }

    public void RefreshContent(Transform _inputTF, Army[] _inputArmies)
    {
        //
        // 탐색 스킬 Lv에 따라서 맨 마지막 병사가 Unknown으로 표시되어야 할 수도 있다. ★
        //

        int result = 0;
        Transform temp;

        for(int i = 0; i < 4; i++)
        {
            //if (_inputArmies[i].count == 0)
            //    continue;

            temp = _inputTF.GetChild(i);

            if(_inputArmies[i].bIsCommander)
            {
                temp.GetComponentInChildren<Image>().sprite = _inputArmies[i].commander.sCommanderImg;

                temp.Find("Name").GetComponentInChildren<Text>().text = _inputArmies[i].commander.sCommanderName;

                temp.Find("AD").GetComponentInChildren<Text>().text =
                    _inputArmies[i].stat.AD.ToString();
                temp.Find("AP").GetComponentInChildren<Text>().text =
                    _inputArmies[i].stat.AP.ToString();
                temp.Find("HP").GetComponentInChildren<Text>().text =
                    _inputArmies[i].stat.HP.ToString();

                result += _inputArmies[i].stat.Total;
            }
            else
            {
                temp.GetComponentInChildren<Image>().sprite = _inputArmies[i].soldier.iSoldierImg;

                temp.Find("Name").GetComponentInChildren<Text>().text = _inputArmies[i].soldier.sSoldierName;

                temp.Find("AD").GetComponentInChildren<Text>().text =
                    _inputArmies[i].stat.AD.ToString();
                temp.Find("AP").GetComponentInChildren<Text>().text =
                    _inputArmies[i].stat.AP.ToString();
                temp.Find("HP").GetComponentInChildren<Text>().text =
                    _inputArmies[i].stat.HP.ToString();

                result += _inputArmies[i].stat.Total;
            }            
        }

        TotalPower.text = result.ToString();
        TotalPower.color = Color.white;

        return;
    }

    public void RefreshUnknown(Transform _inputTF)
    {
        Transform temp;
        var unknown = Database.instance.lSoldierList[Constant.iSoldierUnknown];

        for (int i = 0; i < 4; i++)
        {
            temp = _inputTF.GetChild(i);

            temp.GetComponentInChildren<Image>().sprite = unknown.iSoldierImg;

            temp.Find("Name").GetComponentInChildren<Text>().text = "?";

            temp.Find("AD").GetComponentInChildren<Text>().text = "?";
            temp.Find("AP").GetComponentInChildren<Text>().text = "?";
            temp.Find("HP").GetComponentInChildren<Text>().text = "?";
        }

        TotalPower.text = "??";
        TotalPower.color = Color.red;
    }
    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Start()
    {
        IsActive = false;
    }

    private void OnEnable()
    {
        // 테스트를 위하여 맨 처음 전열의 군대의 정보를 가져옴
        //RefreshWindow(InputManager.instance.gSelectNode.lArmy[0]);
    }

    #endregion
}
