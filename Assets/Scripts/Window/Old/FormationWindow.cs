////////////////////////////////////////////////
//
// FormationWindow
//
// 전열 창에서 작동되는 스크립트
// 19. 07. 15
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationWindow : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// FormationWindow를 쓰기 위한 instsance
    /// </summary>
    public static FormationWindow instance;

    /// <summary>
    /// 현재 전열창에서 편집중인 부대
    /// </summary>
    public Army[] armies;
    /// <summary>
    /// 몇 번 전열을 편집중인지 저장
    /// </summary>
    public int SelectFormationNum;

    /// <summary>
    /// 정보창 Transform
    /// </summary>
    public Transform InfoTF;
    /// <summary>
    /// 전열창에 보이는 이미지 Transform
    /// </summary>
    public Transform FormationTF;
    /// <summary>
    /// 비어있음을 표현하는 Sprite
    /// </summary>
    public Sprite EmptySprite;
    /// <summary>
    /// 비어있음을 표현하는 아이콘 Sprite
    /// </summary>
    public Sprite EmptyInfoSprite;

    /// <summary>
    /// 병사 목록이 나열되는 GameObject
    /// </summary>
    public GameObject gSoldierList;
    /// <summary>
    /// 지휘관 목록이 나열되는 GameObject
    /// </summary>
    public GameObject gCommanderList;
    /// <summary>
    /// 병사 / 지휘관 목록 중 어느 것이 보여지는지 확인시켜주는 변수
    /// </summary>
    public bool IsChange;

    /// <summary>
    /// 선택한 병사 / 지휘관 개체
    /// </summary>
    public int  SelectSoldier;
    /// <summary>
    /// 병사 / 지휘관을 선택한 상태인지 확인시켜주는 변수
    /// </summary>
    public bool IsSelectSoldier;

    /// <summary>
    /// 전열 편집을 할 수 없을 때 막아주는 GameObject
    /// </summary>
    public GameObject gTouchBlocker;
    /// <summary>
    /// 병사 수를 입력할 수 있는 숫자 입력 창
    /// </summary>
    public GameObject gInputNumWindow;
    /// <summary>
    /// 이미지가 선택되었을때 선택된 이미지가 몇번째 칸인지 저장해주는 변수
    /// </summary>
    public int iSelectFormationImgNum;


    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// 보이는 전열이 갱신될 때 작동하는 함수
    /// </summary>
    /// <param name="index"></param>
    public void RefreshFormation()
    {
        for (int i = 0; i < 4; i++)
        {
            FormationTF.GetChild(i).GetComponent<Image>().sprite
                = armies[i].soldier.iSoldierImgBig;

            if(armies[i].soldier == Database.instance.lSoldierList[Constant.iSoldierEmpty])
            {
                FormationTF.GetChild(i).Find("Text").GetComponent<Text>().text = "-";
            }
            else
            {
                FormationTF.GetChild(i).Find("Text").GetComponent<Text>().text
                    = armies[i].count.ToString();
            }
        }


    }

    /// <summary>
    /// Info창에 표시되는 정보를 초기값으로 돌림
    /// </summary>
    public void ResetInfo()
    {
        InfoTF.Find("Name").GetChild(0).GetComponent<Text>().text = "Name";

        InfoTF.Find("Stat").GetChild(0).GetComponent<Text>().text = "AD";
        InfoTF.Find("Stat").GetChild(1).GetComponent<Text>().text = "AP";
        InfoTF.Find("Stat").GetChild(2).GetComponent<Text>().text = "HP";

        InfoTF.Find("Img").GetComponent<Image>().sprite = EmptyInfoSprite;
    }

    /// <summary>
    /// 선택된 노드의 군사를 새로고침 해주는 함수
    /// </summary>
    public void RefreshArmies()
    {
        InputManager.instance.gSelectNode.lArmy[SelectFormationNum] = armies;

        //if (!InputManager.instance.gSelectNode.bIsArmy)
        //    InputManager.instance.gSelectNode.bIsArmy = true;

        if(InputManager.instance.gSelectNode.bIsItArmy[SelectFormationNum] == ArmyState.None)
        {
            InputManager.instance.gSelectNode.bIsItArmy[SelectFormationNum] = ArmyState.Stay;
        }

        IsSelectSoldier = false;
        SelectSoldier = -1;
    }

    /// <summary>
    /// 비어있는 전열을 만드는 함수
    /// </summary>
    /// <returns>비어있는 전열</returns>
    public Army[] MakeEmptyArmy()
    {
        Army EmptyArmy = new Army(Database.instance.lSoldierList[Constant.iSoldierEmpty], 0);
        Army[] result = new Army[4]
        {
            EmptyArmy,EmptyArmy,EmptyArmy,EmptyArmy
        };

        return result;
    }

    /// <summary>
    /// 병사 / 지휘관 전환 버튼에서 작동하는 함수
    /// </summary>
    public void Button_Switch()
    {
        // ScrollRect 변경해줘야함 ★
        IsChange ^= true;

        if (IsChange)
        {
            gSoldierList.SetActive(false);
            gCommanderList.SetActive(true);
        }
        else
        {
            gCommanderList.SetActive(false);
            gSoldierList.SetActive(true);
        }
    }

    /// <summary>
    /// 전열 번호 버튼이 눌렸을 때 작동하는 함수
    /// </summary>
    public void Button_FormationNum()
    {
        if (SelectFormationNum != int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name))
        {
            SelectFormationNum = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

            switch (InputManager.instance.gSelectNode.bIsItArmy[SelectFormationNum])
            {
                case ArmyState.Stay:
                    armies = InputManager.instance.gSelectNode.lArmy[SelectFormationNum];
                    if (gTouchBlocker.activeSelf)
                        gTouchBlocker.SetActive(false);
                    break;

                case ArmyState.Ready:
                    armies = InputManager.instance.gSelectNode.lArmy[SelectFormationNum];
                    gTouchBlocker.SetActive(true);
                    break;

                case ArmyState.None:
                    armies = MakeEmptyArmy();
                    if (gTouchBlocker.activeSelf)
                        gTouchBlocker.SetActive(false);
                    break;
            }

            RefreshFormation();
        }
        else
        {
            return;
        }
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

        EmptySprite = Resources.Load("Char_Big/Empty",typeof(Sprite)) as Sprite;
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        IsChange = false;
        IsSelectSoldier = false;
        SelectFormationNum = 0;

        if (!InputManager.instance.gSelectNode.CheckIsInArmy()
            ||
            InputManager.instance.gSelectNode.lArmy[0] == null)
        {
            armies = MakeEmptyArmy();
        }
        else
        {
            armies = InputManager.instance.gSelectNode.lArmy[SelectFormationNum];
        }

        RefreshFormation();

        if(InputManager.instance.gSelectNode.bIsItArmy[SelectFormationNum] == ArmyState.Ready)
        {
            gTouchBlocker.SetActive(true);
        }
        //Button_FormationNum();
        ResetInfo();
    }

    private void OnDisable()
    {
        armies = null;

        ResetInfo();
    }
}
