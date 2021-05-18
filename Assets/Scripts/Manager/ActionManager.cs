////////////////////////////////////////////////
//
// ActionManager
//
// 플레이어가 턴을 종료한 후 실행되는 모든 행동
// 19. 05. 15
// 20. 02. 14 >> Remaster
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 저장된 명령을 구분해주는 열거형 변수
/// </summary>
public enum OrderNum
{
    Move,
    Attack,
    Production,
    UpgradeWall,
    Scout
}
/// <summary>
/// 행동이 진행되는 상태를 나타내는 열거형 변수
/// </summary>
public enum ActionState
{
    SettingOrder,
    CameraMove,
    ArmyMove,
    DelayOrder,
    ActiveOrder,
    End,

    MAX
}

/// <summary>
/// 플레이어의 행동을 표현해주는 구조체
/// </summary>
public struct Order
{
    /// <summary>
    /// 어떤 명령인지 저장됨
    /// </summary>
    public OrderNum order;
    /// <summary>
    /// 자신의 이동할 군대
    /// </summary>
    public Army[] lMyArmy;
    /// <summary>
    /// 상대의 군대
    /// </summary>
    public Army[] lEnemyArmy;
    /// <summary>
    /// 목적지 노드
    /// </summary>
    public MapNode mDestination;
    /// <summary>
    /// 명령의 시작 노드
    /// </summary>
    public MapNode mStartPoint;
    // public playerinfo;

    //
    // 생성자
    //
    /// <summary>
    /// 이동/생산 시 필요 생성자
    /// </summary>
    /// <param name="ord">명령 종류</param>
    /// <param name="lArmy">움직일/생성할 군대</param>
    /// <param name="dest">목적지</param>
    public Order(OrderNum ord, Army[] lArmy, MapNode dest, MapNode start)
    {
        order = ord;
        lMyArmy = lArmy;
        lEnemyArmy = null;
        mDestination = dest;
        mStartPoint = start;
    }
    /// <summary>
    /// 공격 시 필요 생성자
    /// </summary>
    /// <param name="myArmy">공격측 군대</param>
    /// <param name="enmArmy">수비측 군대</param>
    /// <param name="Attacked">전투가 일어날 장소</param>
    public Order(Army[] myArmy, Army[] enmArmy, MapNode Attacked, MapNode start)
    {
        order = OrderNum.Attack;
        lMyArmy = myArmy;
        lEnemyArmy = enmArmy;
        mDestination = Attacked;
        mStartPoint = start;
    }
    /// <summary>
    /// 성벽강화/탐색 시 공통 필요 생성자
    /// </summary>
    /// <param name="ord">명령 종류</param>
    /// <param name="dest">명령을 수행할 노드</param>
    public Order(OrderNum ord, MapNode dest)
    {
        order = ord;
        lMyArmy = null;
        lEnemyArmy = null;
        mDestination = null;
        mStartPoint = dest;
    }

    /// <summary>
    /// 명령시 선 색을 변경하기 위해 해당하는 선을 찾아 반환해주는 함수
    /// </summary>
    /// <returns>찾아낸 선</returns>
    public Transform FindLine()
    {
        Transform result;

        if (result = mStartPoint.transform.Find(mDestination.name))
            return result;
        else if (result = mDestination.transform.Find(mStartPoint.name))
            return result;

        return result;
    }
}

public class ActionManager : MonoBehaviour
{
    #region 변수

    /// <summary>
    /// 행동을 사용하기 위한 instance
    /// </summary>
    static public ActionManager instance;
    /// <summary>
    /// 플레이어가 실행한 행동을 저장할 Queue
    /// </summary>
    //public Queue<Order> qTurnend = new Queue<Order>();
    public List<Order> lTurnend;
    /// <summary>
    /// 맵의 노드들이 모여있는 오브젝트
    /// </summary>
    public Transform NodeTF;

    //
    // 카메라의 이동을 위한 변수들
    //
    /// <summary>
    /// 카메라 속도 및 진행속도를 저장하는 변수
    /// </summary>
    private float fCameraTrackingSpeed;
    /// <summary>
    /// 목적지 좌표
    /// </summary>
    private Vector3 vDest;
    /// <summary>
    /// 카메라 목적지 좌표
    /// </summary>
    private Vector3 vCameraDest;
    /// <summary>
    /// 움직이는 대상의 최종 목적지
    /// </summary>
    private Vector3 vLastTargetPos;
    /// <summary>
    /// 움직이는 대상의 시작점
    /// </summary>
    private Vector3 vStartPos;
    /// <summary>
    /// 행동 시작할 때 보이는 아이콘
    /// </summary>
    public GameObject gMoveIcon;
    /// <summary>
    /// 현재 행동 상태를 저장하는 변수
    /// </summary>
    private ActionState currentAction;
    /// <summary>
    /// 노드 사이의 길을 그릴 때 필요한 Material
    /// </summary>
    public Material MTRoad;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수
    //
    // 명령을 수행하는 함수들
    //

    /// <summary>
    /// 이동 명령
    /// </summary>
    /// <param name="armies">움직일 군대</param>
    /// <param name="dest">도착지</param>
    public void Order_Move(Order _order)
    {
        // 이동할 군대를 도착지로 옮김
        _order.mDestination.lArmy[0] = _order.lMyArmy;
        _order.mDestination.bIsItArmy[0] = ArmyState.Stay;

        // 출발지에 이동한 군대칸을 비우고, 비워져 있는 상태로 변경
        for(int i = 0; i < 4; i++)
        {
            if (_order.mStartPoint.lArmy[i] == _order.lMyArmy)
            {
                Array.Clear(_order.mStartPoint.lArmy, i, 1);
                _order.mStartPoint.lArmy[i] = (Army[])Constant.aEmptyArmies.Clone();
                _order.mStartPoint.bIsItArmy[i] = ArmyState.None;
                return;
            }
        }

        //Array.Clear(InputManager.instance.gSelectNode.lArmy, InputManager.instance.iSelectSpace, 1);                
    }

    /// <summary>
    /// 공격 명령
    /// </summary>
    /// <param name="MyArmies">플레이어 군대</param>
    /// <param name="EnmArmies">도착지의 군대</param>
    public void Order_Attack(Order _order)
    {
        // 전투 액션
        // Scene을 전환하기 전 맵에서의 진행 상황을 Datamanager에 저장한다.
        // >> 전투를 마치고 나온 후에 계속 진행 되어야 하기 때문
        //DataManager.instance.qUserOrder = qTurnend;
        DataManager.instance.lUserOrder = lTurnend;
        DataManager.instance.iLastTurnRate = TurnManager.instance.turnRate;
        DataManager.instance.tsLastTurnState = TurnManager.instance.tsCurrentState;
        DataManager.instance.lUserArmy = _order.lMyArmy;
        DataManager.instance.lEnemyArmy = _order.lEnemyArmy;
        DataManager.instance.bAttacked = _order.mDestination;
        //Copy(ref DataManager.instance.bAttacked, AttackedNode);

        // 이후 Scene을 전환하여 전투로 넘어간다
        DataManager.instance.LoadScene("BattleScene");
    }

    /// <summary>
    /// 공격명령 후 실행되는 함수
    /// </summary>
    public void Order_AfterBattle()
    {
        //qTurnend = DataManager.instance.qUserOrder;
        lTurnend = DataManager.instance.lUserOrder;
        TurnManager.instance.turnRate = DataManager.instance.iLastTurnRate;
        TurnManager.instance.tsCurrentState = DataManager.instance.tsLastTurnState;

        // 전투 결과에 따라서 노드를 먹을지 정해짐
        // 얕은 복사로 인해 정보가 넘어가지 않음 >> 대체 왜그러는 건가
        //if (DataManager.instance.bIsUserWin)
        //{
        //    for(int i = 0; i < NodeTF.childCount; i++)
        //    {
        //        if (NodeTF.GetChild(i).Find(DataManager.instance.bAttacked.sName))
        //        {
        //            NodeTF.GetChild(i).Find(DataManager.instance.bAttacked.sName).GetComponent<MapNode>().iAffiliation
        //                = TurnManager.instance.currentPlayer.playerCountry;
        //        }
        //    }
        //}

        // 정보를 불러 온 후 기본 정보를 초기화 해줌
        DataManager.instance.bIsUserWin = false;
        DataManager.instance.bAfterBattle = false;
    }

    /// <summary>
    /// 벽 강화 명령
    /// </summary>
    /// <param name="dest">벽을 강화할 노드</param>
    public void Order_UpgradeWall(MapNode dest)
    {
        //
        // 자원 소모 요소가 첨가 되어야 함! ★
        //
        dest.sWallLV++;
        dest.SumDefensive();
    }


    //
    // 단계별 행동을 위한 함수
    //
    /// <summary>
    /// 행동 표출 전 세팅해주는 함수
    /// </summary>
    public void SettingOrder()
    {
        // 맨 앞에 있는 명령 받아서 저장
        Order _order = lTurnend[0];
        
        // 시작점과 도착점 저장
        vStartPos = _order.mStartPoint.transform.position;
        vDest = _order.mDestination.transform.position;
        // 시작점과 도착점 사이의 좌표를 카메라 좌표로 설정
        vCameraDest = new Vector3((vStartPos.x + vDest.x) / 2, (vStartPos.y + vDest.y) / 2, -10);

        // 움직일 전열에서 지휘관 찾아서 아이콘 변경
        if (Constant.FindCommander(_order.lMyArmy) >= 0)
            gMoveIcon.GetComponent<SpriteRenderer>().sprite = _order.lMyArmy[Constant.FindCommander(_order.lMyArmy)].commander.sCommanderImg;
        // 없는 경우 맨 처음 군사로 아이콘 변경
        else
            gMoveIcon.GetComponent<SpriteRenderer>().sprite = _order.lMyArmy[Constant.FindSoldier(_order.lMyArmy)].soldier.iSoldierImg;

        // 아이콘 초기 위치를 시작점으로 이동
        gMoveIcon.transform.position = vStartPos;

        // 카메라 확대 크기를 고정
        Camera.main.orthographicSize = 4f;

        // 세팅이 끝났으므로 다음 행동 단계로 이동
        currentAction++;
    }

    /// <summary>
    /// 카메라를 이동시키는 함수
    /// </summary>
    public void CameraMove()
    {
        // 현재 카메라의 위치를 저장
        vLastTargetPos = Camera.main.transform.position;

        // 카메라의 위치를 계산한 카메라 위치로 이동
        Camera.main.transform.position = Vector3.Slerp(vLastTargetPos, vCameraDest, 0.2f);

        // 카메라가 해당 위치까지 갔으면 다음 행동 단계로 이동
        if (Camera.main.transform.position == vCameraDest)
        {
            currentAction++;            
        }
    }

    /// <summary>
    /// 아이콘을 이동시키는 함수
    /// </summary>
    public void IconMove()
    {
        // 현재 아이콘의 위치를 저장
        vLastTargetPos = gMoveIcon.transform.position;

        // 아이콘의 위치를 도착점 까지 이동
        gMoveIcon.transform.position = Vector3.Lerp(vStartPos, vDest, fCameraTrackingSpeed);

        // 일정한 속도로 움직임
        fCameraTrackingSpeed += 0.02f;

        // 아이콘이 해당 위치까지 갔으면 다음 행동 단계로 이동
        if(gMoveIcon.transform.position == vDest)
        {
            currentAction++;
            fCameraTrackingSpeed = 0.05f;
        }
    }

    /// <summary>
    /// 아이콘이 도착 후 잠깐의 딜레이를 위한 함수
    /// </summary>
    public void OnDelay()
    {
        // 프레임 수에 비례해서 잠깐의 시간동안 딜레이
        fCameraTrackingSpeed += Time.deltaTime / .5f;

        // 일정 프레임 이상 지나면 다음 행동 단계로 이동
        if (fCameraTrackingSpeed >= 1f)
        {
            currentAction++;
            fCameraTrackingSpeed = 0.05f;
        }
    }

    /// <summary>
    /// 해당하는 명령 수행됨
    /// </summary>
    public void ActivateOrder()
    {
        // 맨 앞에 있는 명령 저장
        Order _order = lTurnend[0];

        // 종류에 따라 명령 수행
        switch (_order.order)
        {
            case OrderNum.Move:
                Order_Move(_order);
                break;

            case OrderNum.Attack:
                Order_Attack(_order);
                break;            
        }

        // 명령을 수행한 후 변경한 길의 색상을 기존의 길로 복구
        _order.FindLine().GetComponent<LineRenderer>().material = MTRoad;
        // 행동이 실행되었음을 알림
        DataManager.instance.Notify(_order);
        // 수행한 명령 삭제
        lTurnend.RemoveAt(0);

        if (lTurnend.Count == 0)
        {
            // 대기 명령들이 모두 끝남
            currentAction++;
        }
        else
        {
            // 대기중인 명령 하나가 끝났으므로 다음 명령으로 이동
            currentAction = 0;
        }
    }


    public void Copy(ref MapNode a, MapNode b)
    {
        a = b.DeepCopy();
        
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행
    private void Awake()
    {
        // instance를 사용하기 위한 싱글톤 제작
        // 현재 게임오브젝트를 싱글톤으로 설정 및 중복생성 방지
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);

        NodeTF = GameObject.Find("Node").transform;
    }

    private void Start()
    {
        // 맵이 시작할 때 전투를 하고 왔는지 확인 후 사항을 적용
        // 전투를 하고온 경우 라면 이전만큼 진행된 상황에서 이어서 진행되어야 하기 때문
        if (DataManager.instance.bAfterBattle)
            Order_AfterBattle();
    }

    private void Update()
    {
        // 모든 명령이 끝날 때 까지 실행
        switch (currentAction)
        {
            case ActionState.SettingOrder:
                SettingOrder();
                break;

            case ActionState.CameraMove:
                CameraMove();
                break;

            case ActionState.ArmyMove:
                IconMove();
                break;

            case ActionState.DelayOrder:
                OnDelay();
                break;

            case ActionState.ActiveOrder:
                ActivateOrder();
                break;

                // 모든 명령이 끝났으므로 턴을 넘김
            case ActionState.End:
                TurnManager.instance.TurnEnd();
                gMoveIcon.SetActive(false);
                gameObject.SetActive(false);
                break;
        }
    }

    private void OnEnable()
    {
        // 해당 턴에 아무 명령도 내려지지 않았다면 아무 활동 없이 다음 턴으로 이동
        if (TurnManager.lTurnend.Count == 0)
        {
            TurnManager.instance.TurnEnd();
            this.gameObject.SetActive(false);
            return;
        }

        fCameraTrackingSpeed = 0.05f;
        currentAction = 0;

        gMoveIcon.SetActive(true);

        vDest = Vector3.zero;
        vCameraDest = Vector3.zero;
        vStartPos = Vector3.zero;
        vLastTargetPos = Vector3.zero;

        lTurnend = TurnManager.lTurnend;        
    }

    #endregion
}
