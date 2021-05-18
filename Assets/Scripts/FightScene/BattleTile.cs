////////////////////////////////////////////////
//
// BattleTile
//
// 전투의 각 칸들이 소지하는 스크립트
// 19. 06. 25
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleTile : MonoBehaviour, IPointerDownHandler
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 타일이 모두 죽었는지 확인해줌
    /// </summary>
    public bool IsDie;
    /// <summary>
    /// 타일이 지휘관인지 확인해줌
    /// </summary>
    public bool isCommander;
    /// <summary>
    /// 타일에 존재하는 군대
    /// </summary>
    public Army army;
    /// <summary>
    /// 타일의 전투력
    /// </summary>
    public int power;
    /// <summary>
    /// 타일의 체력
    /// </summary>
    public int hp;
    /// <summary>
    /// 타일의 군대 이미지
    /// </summary>
    public Image Img;
    /// <summary>
    /// 타일의 Transform
    /// </summary>
    private Transform TF;


    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// 시작하자마자 각 타일이 정보를 갱신한다
    /// </summary>
    public void InsertInfo()
    {
        // 타일이 어느 쪽에 소속되어 있는지 확인 후 갱신
        // 해당 칸에 들어갈 군대가 없으면 정보를 갱신하지 않고 끝냄
        if (transform.parent.name == "User")
        {
            if(DataManager.instance.lUserArmy[int.Parse(name)].count == 0)
            {
                IsDie = true;
                return;
            }
            army = DataManager.instance.lUserArmy[int.Parse(name)];
        }
        else
        {
            if (DataManager.instance.lEnemyArmy[int.Parse(name)].count == 0)
            {
                IsDie = true;
                return;
            }
            army = DataManager.instance.lEnemyArmy[int.Parse(name)];
        }

        // 갱신한 군대의 정보로 전투력, 체력을 계산함
        //power = army.soldier.iSoldierPower * army.count;
        //hp = army.soldier.iSoldierHp * army.count;

        // 병사 수가 1명이면 지휘관으로 판명
        // 수정해야함 >> 병사 정보에 지휘관이라고 별도의 표기가 필요 ★
        //if ((int)army.soldier.eSoldierID == -1) isCommander = true;

        // 해당 타일을 맞는 병사의 이미지로 변경
        if (army.bIsCommander)
            Img.sprite = army.commander.sCommanderImg;
        else
            Img.sprite = army.soldier.iSoldierImg;
    }

    /// <summary>
    /// 전투를 전부 담당할 부분
    /// </summary>
    /// <param name="eventData">기본입력데이터</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        // 현재 어떠한 행동이 취해지고 있으면 터치해도 반응하지 않아야함.
        // ex) 공격 이펙트 시전, 카드 효과 시전
        if (BattleTurn.instance.bAction) return;

        // 현재 턴을 진행중인 타일 정보와 터치되 타일 정보를 비교, 행동이 결정된다
        if(BattleTurn.instance.currentTile == GetComponent<BattleTile>())
        {
            // 현재 턴을 진행중인 타일을 터치했을 때
            Debug.Log("현재 턴, 상대를 눌러야함");
        }
        else if(TF.parent.name == "User")
        {
            // 현재 턴을 진행중인 타일과 같은 소속을 터치했을 때
            Debug.Log("같은 팀, 상대를 눌러야함");
        }
        else if(TF.parent.name == "Enemy")
        {
            if (BattleTurn.instance.currentTile.name == name)
            {
                // 현재 턴을 진행중인 타일의 범위 안에 있는 적을 터치했을 때
                Debug.Log("공격");

                // 전투가 이루어짐
                // 이펙트 추가 가시화 필요 ★
                if (BattleTurn.instance.currentTile.power < power)
                {
                    army.count -= BattleTurn.instance.currentTile.army.count;
                    BattleTurn.instance.currentTile.army.count = 0;
                    SetDie(BattleTurn.instance.currentTile);
                }
                else if(BattleTurn.instance.currentTile.power == power)
                {
                    army.count = BattleTurn.instance.currentTile.army.count = 0;
                    SetDie(this);
                    SetDie(BattleTurn.instance.currentTile);
                }
                else if (BattleTurn.instance.currentTile.power > power)
                {
                    BattleTurn.instance.currentTile.army.count -= army.count;
                    army.count = 0;
                    SetDie(this);
                }

                // 다음 타일로 턴을 넘김
                BattleTurn.instance.TurnEnd();
            }
            else
                // 적 소속이긴 하나 타일의 군대가 때릴 수 없는 범위의 적을 터치했을 때
                Debug.Log("범위 밖");
        }
    }

    public void SetDie()
    {
        this.army.count = 0;
        this.Img.color = Color.red;
        this.IsDie = true;
    }

    public void SetDie(BattleTile tile)
    {
        tile.Img.color = Color.red;
        tile.IsDie = true;
    }


    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        // 사전에 미리 파싱해놓음
        Img = GetComponent<Image>();
        TF = transform;

        // 전투가 시작되면 바로 타일을 갱신
        InsertInfo();
    }

    private void Start()
    {

    }
}
