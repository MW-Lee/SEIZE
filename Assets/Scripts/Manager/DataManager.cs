////////////////////////////////////////////////
//
// DataManager
//
// 게임의 진행상황 data를 모두 소지하는 Dontdestroy 오브젝트
// 19. 06. 28
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// DataManager를 사용하기 위한 instance
    /// </summary>
    public static DataManager instance;

    //
    // 플레이어가 가지고 있어야 하는 정보
    //
    /// <summary>
    /// 진행된 로그들을 저장
    /// </summary>
    public List<List<Log>> lLogList = new List<List<Log>>();
    /// <summary>
    /// 플레이어가 소유한 지휘관 목록
    /// </summary>
    public List<Commander> lComList = new List<Commander>();

    //public List<Item> lItemList = new List<Item>();

    //
    // 전투에 사용되는 데이터
    //
    /// <summary>
    /// 공격 군대
    /// </summary>
    public Army[] lUserArmy;
    /// <summary>
    /// 수비 군대
    /// </summary>
    public Army[] lEnemyArmy;


    //
    // Map으로 돌아가서 그대로 진행하기 위한 데이터 저장
    //
    /// <summary>
    /// 전투 직전 턴 진행율
    /// </summary>
    public int iLastTurnRate;
    /// <summary>
    /// 전투 직전 턴 상황
    /// </summary>
    public TurnState tsLastTurnState;
    /// <summary>
    /// 전투 직전 명령 대기열
    /// </summary>
    public List<Order> lUserOrder;
    /// <summary>
    /// 유저가 이겼는지 확인
    /// </summary>
    public bool bIsUserWin;
    /// <summary>
    /// 전투 직전인지 확인
    /// </summary>
    public bool bAfterBattle;
    /// <summary>
    /// 공격 당한 노드의 위치를 저장 >> 노드의 소속이 변경될 때를 대비
    /// </summary>
    public MapNode bAttacked;
    /// <summary>
    /// 게임 안의 일 수를 세어주는 변수
    /// </summary>
    public int iDay = 0;
    
    //
    // Scene 호출에 쓰이는 데이터
    //
    /// <summary>
    /// 호출할 Scene 이름
    /// </summary>
    public string sSceneName;


    //
    // 튜토리얼 진행을 위한 변수들
    //
    public int iTutRate;

    public bool bIsTut;




    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// Scene을 불러오기 위한 공용 함수
    /// </summary>
    /// <param name="SceneName">불러올 Scene의 이름</param>
    public void LoadScene(string SceneName)
    {
        sSceneName = SceneName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScene");
    }

    /// <summary>
    /// 행동이 실행된 후 Log로 남기기 위하여 LogWindow에 알려주는 함수
    /// </summary>
    /// <param name="_inputOrder"></param>
    public void Notify(Order _inputOrder)
    {
        Log temp = Log.AddLog(_inputOrder);

        lLogList[iDay].Add(temp);
    }



    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        bAfterBattle = false;

        // 테스트를 위한 임시 지휘관 ★
        lComList.Add(Database.instance.dCommanderList["Aron"]);
    }
}
