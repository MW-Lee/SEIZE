////////////////////////////////////////////////
//
// BattleTurnManager
//
// 전투시 턴에 관련된 스크립트
// 19. 06. 24
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleTurn : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// BattleTurn을 사용하기 위한 instance
    /// </summary>
    static public BattleTurn instance;
    /// <summary>
    /// 턴의 순서를 타일로 저장함
    /// </summary>
    public List<BattleTile> lTurnWait = new List<BattleTile>();
    /// <summary>
    /// 현재 턴을 진행중인 타일
    /// </summary>
    public BattleTile currentTile;
    /// <summary>
    /// 현재 턴이 진행율을 저장
    /// </summary>
    public int turnRate;
    /// <summary>
    /// 플레이어의 턴인지 확인 >> AI 실행을 위함
    /// </summary>
    public bool bUserturn;

    /// <summary>
    /// 유저 측 Transform
    /// </summary>
    public Transform UserTF;
    /// <summary>
    /// 적군 측 Transform
    /// </summary>
    public Transform EnemyTF;
    /// <summary>
    /// 현재 진행하는 타일의 정보를 나타낼 Info 측 Transform
    /// </summary>
    public Transform InfoTF;
    /// <summary>
    /// 현재 어떤 행동이 취해지는 중인지 확인함
    /// </summary>
    public bool bAction;

    public GameObject gResultWindow;

    public GameObject gQuestWindow;

    public string sQuestname = string.Empty;

    public bool bIsBattleEnd;
    public bool bIsUserWin;

    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// 턴의 시작마다 실행되는 함수
    /// </summary>
    public void TurnStart()
    {
        // 현재 진행중인 턴을 갱신함
        currentTile = lTurnWait[turnRate];

        InfoTF.GetChild(0).GetComponent<Image>().sprite = currentTile.Img.sprite;
        InfoTF.GetChild(1).GetComponentInChildren<Text>().text = currentTile.army.stat.AD.ToString();
        InfoTF.GetChild(2).GetComponentInChildren<Text>().text = currentTile.army.stat.AP.ToString();
        InfoTF.GetChild(3).GetComponentInChildren<Text>().text = currentTile.army.stat.HP.ToString();
    }

    /// <summary>
    /// 명령에 맞게 행동이 시작되는 함수
    /// </summary>
    public void StartAction()
    {
        // 공격, 카드사용에 맞게 어울리는 이펙트 실행...
    }

    /// <summary>
    /// 턴이 종료마다 실행되는 함수
    /// </summary>
    public void TurnEnd()
    {
        // 턴 진행율 갱신 >> 만약 턴 대기열에서 끝까지 갔으면 다시 처음부터
        turnRate++;
        if (turnRate == lTurnWait.Count) turnRate = 0;

        // 턴 종료마다 양쪽의 상태를 확인하여 한쪽의 군대가 전멸하면 전투를 끝내야함
        // 양쪽 중 한쪽이 전멸하였을 때 바로 맵으로 돌아감 >> 승리 모션이 시전되야함 ★
        for (int a = 0; a < 2; a++)
        {
            // 전멸 확인용 임시변수
            int temp = 0;

            switch (a)
            {
                case 0:
                    for (int i = 0; i < UserTF.childCount; i++)
                    {
                        if (UserTF.GetChild(i).GetComponent<BattleTile>().IsDie) temp++;
                    }
                    if (temp == UserTF.childCount)
                    {
                        //BackToMap(false);
                        bIsUserWin = false;
                        bIsBattleEnd = true;
                        ShowResult();
                        return;
                    }
                    break;

                case 1:
                    for (int i = 0; i < EnemyTF.childCount; i++)
                    {
                        if (EnemyTF.GetChild(i).GetComponent<BattleTile>().IsDie) temp++;
                    }
                    if (temp == EnemyTF.childCount)
                    {
                        //BackToMap(true);
                        bIsUserWin = true;
                        bIsBattleEnd = true;
                        ShowResult();
                        return;
                    }
                    break;
            }
        }
    }

    public void ShowResult()
    {
        gResultWindow.SetActive(true);
    }

    public void SetQuest()
    {
        // 랜덤으로 퀘스트를 지정해서 작동? ★
        sQuestname = "Test";
    }

    public void ShowQuest()
    {
        gQuestWindow.SetActive(true);
        QuestWindow.instance.lQuestList = Database.instance.dQuestList[sQuestname];
        
    }


    /// <summary>
    /// 맵으로 돌아가는 함수
    /// </summary>
    /// <param name="userWin">전투가 누구의 승리로 끝났는지 확인</param>
    public void BackToMap()
    {
        // 승자정보, 전투가 끝난 직후 정보를 저장한 후 맵으로 돌아감
        DataManager.instance.bIsUserWin = bIsUserWin;
        DataManager.instance.bAfterBattle = true;

        DataManager.instance.LoadScene("MapPractice");
    }


    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 맵 Scene에서 DontDestroy로 가지고 와야함...
        //InputManager.instance.StaticFadePopup("전투 시작");

        turnRate = 0;
        currentTile = lTurnWait[turnRate];
        bUserturn = true;
        bAction = false;

        TurnStart();
    }

    private void Update()
    {
        if (!bIsBattleEnd)
        { 
            // 현재 AI가 구성되지 않았으므로, 적의 차례에는 넘긴다.
            // AI가 구현되면 변경되어야 함. ★
            if (turnRate % 2 == 1)
            {
                turnRate++;
                if (turnRate == lTurnWait.Count) turnRate = 0;
                currentTile = lTurnWait[turnRate];
                TurnStart();
            }
            // 비어있는 칸의 차례이면 넘김
            else if (currentTile.army.count == 0)
            {
                turnRate++;
                if (turnRate == lTurnWait.Count) turnRate = 0;
                TurnStart();
            }
        } 
    }
}
