////////////////////////////////////////////////
//
// CapitalWindow
//
// 노드 선택시 나오는 창에서 작동되는 스크립트
// 19. 05. 20
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeWindow : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 열려있는 CapitalWindow를 사용하기 위한 instance
    /// </summary>
    static public NodeWindow instance;
    /// <summary>
    /// 생산창
    /// </summary>
    public GameObject gPWindow;
    /// <summary>
    /// 수도용 군사창
    /// </summary>
    public GameObject CapitalArmy;
    /// <summary>
    /// 도시 / 마을 용 군사창
    /// </summary>
    public GameObject VilArmy;
    /// <summary>
    /// 탐색 창
    /// </summary>
    public GameObject ScoutWindow;
    

    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// 창이 켜질 때 초기세팅을 위한 함수
    /// </summary>
    public void SettingWindow()
    {
        MapNode selectedMapnode = InputManager.instance.gSelectNode;
        Sprite setIcon = Database.instance.lSoldierList[Constant.iSoldierEmpty].iSoldierImg;
        Color setColor = new Color();

        switch (InputManager.instance.gSelectNode.iType)
        {
            case NodeType.Capital:
                CapitalArmy.SetActive(true);
                VilArmy.SetActive(false);

                for (int i = 0; i < 4; i++)
                {
                    switch (InputManager.instance.gSelectNode.bIsItArmy[i])
                    {
                        case ArmyState.None:
                            setIcon = Database.instance.lSoldierList[Constant.iSoldierEmpty].iSoldierImg;
                            setColor = Color.white;
                            break;

                        case ArmyState.Ready:
                            if (Constant.FindCommander(selectedMapnode.lArmy[i]) >= 0)
                                setIcon = selectedMapnode.lArmy[i][Constant.FindCommander(selectedMapnode.lArmy[i])].commander.sCommanderImg;
                            else
                                setIcon = selectedMapnode.lArmy[i][Constant.FindSoldier(selectedMapnode.lArmy[i])].soldier.iSoldierImg;
                            setColor = Color.red;
                            break;

                        case ArmyState.Stay:
                            if (Constant.FindCommander(selectedMapnode.lArmy[i]) >= 0)
                                setIcon = selectedMapnode.lArmy[i][Constant.FindCommander(selectedMapnode.lArmy[i])].commander.sCommanderImg;
                            else
                                setIcon = selectedMapnode.lArmy[i][Constant.FindSoldier(selectedMapnode.lArmy[i])].soldier.iSoldierImg;
                            setColor = Color.green;
                            break;
                    }

                    CapitalArmy.transform.GetChild(i).GetComponent<Image>().sprite = setIcon;
                    CapitalArmy.transform.GetChild(i).GetComponent<Image>().color = setColor;
                }
                return;

            case NodeType.City:
            case NodeType.Village:
                CapitalArmy.SetActive(false);
                VilArmy.SetActive(true);

                switch (InputManager.instance.gSelectNode.bIsItArmy[0])
                {
                    case ArmyState.None:
                        setIcon = Database.instance.lSoldierList[Constant.iSoldierEmpty].iSoldierImg;
                        setColor = Color.white;
                        break;

                    case ArmyState.Ready:
                        if (Constant.FindCommander(selectedMapnode.lArmy[0]) >= 0)
                            setIcon = selectedMapnode.lArmy[0][Constant.FindCommander(selectedMapnode.lArmy[0])].commander.sCommanderImg;
                        else
                            setIcon = selectedMapnode.lArmy[0][Constant.FindSoldier(selectedMapnode.lArmy[0])].soldier.iSoldierImg;
                        setColor = Color.red;
                        break;

                    case ArmyState.Stay:
                        if (Constant.FindCommander(selectedMapnode.lArmy[0]) >= 0)
                            setIcon = selectedMapnode.lArmy[0][Constant.FindCommander(selectedMapnode.lArmy[0])].commander.sCommanderImg;
                        else
                            setIcon = selectedMapnode.lArmy[0][Constant.FindSoldier(selectedMapnode.lArmy[0])].soldier.iSoldierImg;
                        setColor = Color.green;
                        break;
                }
                                
                VilArmy.transform.GetChild(0).GetComponent<Image>().sprite = setIcon;
                VilArmy.transform.GetChild(0).GetComponent<Image>().color = setColor;
                return;
        }
    }
    
    //
    // 각 버튼에서 작동하는 함수
    //
    /// <summary>
    /// 생산 버튼이 눌렸을 때 실행
    /// </summary>
    public void Button_Production()
    {
        // 열려있던 맵 노드 명령창을 닫음
        //Destroy(InputManager.instance.gNodeWindow);

        // 생산창 작동
        gPWindow.SetActive(true);
        InputManager.instance.WActiveWindow = WindowName.ProductionWindow;

        // 주변 검은색 연출을 위해 저장했던 검은 배경 활성화
        InputManager.instance.BG_Black.SetActive(true);

        // 생산 창에서는 다른 노드를 터치할 수 없게 2D Collider를 넣어준다
        // 한번 넣은 컴포턴트를 지울 수 없기 때문에 공고 (생산) 창이 커질 필요도 있음
        InputManager.instance.BG_Black.AddComponent<BoxCollider2D>();
    }

    /// <summary>
    /// 전열 버튼이 눌렸을 때 실행
    /// </summary>
    public void Button_Formation()
    {
        // 전열 창 활성화
        InputManager.instance.gFormationWindow.SetActive(true);
        // 노드 창 비활성화
        InputManager.instance.gNodeWindow.SetActive(false);

        // 현재 열린 창의 정보를 갱신
        InputManager.instance.WActiveWindow = WindowName.FormationWindow;
    }

    /// <summary>
    /// 탐색 버튼이 눌렸을 때 실행
    /// </summary>
    public void Button_Scout()
    {
        ScoutWindow.SetActive(true);

        InputManager.instance.WActiveWindow = WindowName.ScoutWindow;
    }

    /// <summary>
    /// 강화 버튼이 눌렸을 때 실행
    /// </summary>
    public void Button_Upgrade()
    {

    }

    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        SettingWindow();
    }
}
