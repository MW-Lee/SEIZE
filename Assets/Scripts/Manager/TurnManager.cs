////////////////////////////////////////////////
//
// TurnManager
//
// 턴에 관련된 스크립트
// 19. 06. 11
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 현재 턴의 상태를 나타내주는 열거형 변수
/// </summary>
public enum TurnState
{
    /// <summary>
    /// 턴 시작
    /// </summary>
    Start,
    /// <summary>
    /// 지시턴 진행중
    /// </summary>
    Ongoing,
    /// <summary>
    /// 지시턴 종료 / 행동 시작
    /// </summary>
    ActionWait,
    /// <summary>
    /// 행동 종료 / 턴 완전 종료
    /// </summary>
    End
}

public class TurnManager : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 턴을 사용하기 위한 instance
    /// </summary>
    static public TurnManager instance;
    /// <summary>
    /// 턴 대기열 리스트(순서에 변화는 없다)
    /// </summary>
    public List<Player> lTurnWait;
    /// <summary>
    /// 현재 턴을 진행중인 플레이어를 저장
    /// </summary>
    public Player currentPlayer;
    /// <summary>
    /// 대기열에서 몇 번째 플레이어가 진행중인지 저장
    /// </summary>
    public int turnRate;
    /// <summary>
    /// 현재 턴 상태
    /// </summary>
    public TurnState tsCurrentState;

    public GameObject gActionManager;

    public GameObject gDownUI;

    public static List<Order> lTurnend = new List<Order>();

    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// 턴이 시작할 때 실행되는 함수
    /// </summary>
    public void TurnStart()
    {
        // 턴을 진행하는 플레이어의 정보를 갱신
        currentPlayer = lTurnWait[turnRate];
        RenewTurnState();
        //
        // 대충 턴이 시작되는 애니메이션
        //
        if (turnRate == 0)
            InputManager.instance.StaticFadePopup(currentPlayer.userName + "의 턴");

        // Invoke를 이용하여 일정시간 후에 함수가 발동되게 함
        // 비효율 적일 수도 있음
        Invoke("RenewTurnState", 2.1f);
    }

    /// <summary>
    /// 행동이 시작될 때 실행되는 함수
    /// </summary>
    public void StartAction()
    {
        if (InputManager.instance.gNodeWindow.activeSelf)
        {
            InputManager.instance.gNodeWindow.SetActive(false);
            InputManager.instance.WActiveWindow = WindowName.Empty;
        }
        // 행동을 시작하는 팝업
        InputManager.instance.StaticFadePopup("행동 개시");

        // 팝업메시지가 사라진 후 (2.1초 후) 행동이 시작됨
        Invoke("InvokeAction", 2.1f);
        Invoke("RenewTurnState", 2.1f);
    }

    /// <summary>
    /// Invoke 함수 호출용
    /// </summary>
    private void InvokeAction()
    {
        //ActionManager.instance.DoAction();
        InputManager.instance.bOnAction = true;
        gDownUI.SetActive(false);
        gActionManager.SetActive(true);
    }

    /// <summary>
    /// 모든 행동이 끝나고 턴이 종료되면 실행되는 함수
    /// </summary>
    public void TurnEnd()
    {
        RenewTurnState();

        //
        // 대충 턴이 종료되는 애니메이션
        //
        InputManager.instance.StaticFadePopup("턴 종료");

        //
        // 모든 플레이어의 턴이 완료되었을 때 행동
        //
        turnRate++;
        if (turnRate == lTurnWait.Count)
        {
            turnRate = 0;

            DataManager.instance.iDay++;
            Up_UI.instance.RefreshDayCount();
            DataManager.instance.lLogList.Add(new List<Log>());
            LogWindow.instance.AddDayCount();
        }

        InputManager.instance.bOnAction = false;
        gDownUI.SetActive(true);
        //ActionManager.instance.DoAction();
        Invoke("TurnStart", 2.1f);
    }

    /// <summary>
    /// 게임이 시작될 때 변하지 않는 턴리스트를 만들어 놓는다
    /// 나중에 나라가 함락되었을 때를 생각해야함 ★
    /// </summary>
    public void MakeTurnList()
    {
        // 나라 별로 분류된 노드의 부모객체 임시 저장
        GameObject NodeParent = null;
        // 턴 대기열에 등록할 플레이어 임시 정보 저장
        Player player = null;
        // 대기열에 넣을 나라 순서
        Country country;

        // 현재 테스트 중이므로 두 나라만 턴에 등록함
        for (int i = 0; i < 2; i++)
        {
            // 새로 만들 플레이어 정보 중 노드를 임시 저장하는 변수
            List<MapNode> temp = new List<MapNode>();

            // Country 변수 열거 되어있는 순서대로 노드 묶음을 가져옴 > 부모객체
            NodeParent = GameObject.Find("Node").transform.GetChild(i).gameObject;
            // 생성될 플레이어 정보의 나라를 Country 열거형 변수를 사용하여 차례대로 입력
            country = (Country)i;

            // 임시 변수에 Mapnode 형으로 정보들을 저장한다
            for(int j = 0; j < NodeParent.transform.childCount; j++)
            {
                temp.Add(NodeParent.transform.GetChild(j).GetComponent<MapNode>());
            }

            // 유형에 맞게 플레이어의 정보를 생성하여
            if (i == 0)
                player = new Player("user", country, temp, true, 4);
            else
                player = new Player("Enemy" + i, country, temp, false, 4);
            // 턴 대기열에 넣음
            lTurnWait.Add(player);
        }

        return;
    }

    /// <summary>
    /// 턴 상태를 갱신해주는 함수 >> 각 페이즈마다 작동되어야 함
    /// </summary>
    public void RenewTurnState()
    {
        // 턴 상태가 변경 될때마다 현재 턴 상태를 상태 열거형 변수로 지정하여 갱신해준다
        if ((int)tsCurrentState == 4) tsCurrentState = 0;
        else tsCurrentState++;
    }

    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        // 바로 턴 대기열을 생성하여 준다
        MakeTurnList();
    }

    private void Start()
    {
        // 저장 되어있는 순서를 불러와야 한다
        if (DataManager.instance.bAfterBattle)
        {
            
        }
        else
        {
            TurnStart();
        }
    }
}
