////////////////////////////////////////////////
//
// InputManager
//
// 게임에 입력되는 터치를 총괄하는 스크립트
// 19. 02. 22
// MWLee
////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 현재 활성화된 창이 어떤 창인지 구분해주는 열거형 변수
/// </summary>
public enum WindowName
{
    NodeWindow,
    FormationWindow,
    ScoutWindow,
    ProductionWindow,
    ProductSelectWindow,
    QuestWindow,
    SimpleWindow,
    ArmyInfoWindow,

    AttackMove,

    Max,
    Empty
}

/// <summary>
/// 플레이어의 터치를 총괄하는 매니저
/// </summary>
public class InputManager : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// InputManager를 외부에서 사용하기 위한 instance
    /// </summary>
    static public InputManager instance;

    //
    // Window & UI
    //
    /// <summary>
    /// 활성화된 창이 무슨 창인지 구분하는 열거형 변수
    /// </summary>
    public WindowName WActiveWindow;
    /// <summary>
    /// 노드가 선택되어 나온 창
    /// </summary>
    public GameObject gNodeWindow;
    /// <summary>
    /// 전열 세팅하는 전열 창
    /// </summary>
    public GameObject gFormationWindow;
    /// <summary>
    /// 플레이어의 군대 정보를 보여주는 창
    /// </summary>
    public GameObject gMyArmyInfoWindow;
    /// <summary>
    /// 적의 군대 정보를 보여주는 창
    /// </summary>
    public GameObject gEnemyArmyInfoWindow;
    /// <summary>
    /// 간단한 질문을 실행시키는 창
    /// </summary>
    public GameObject gSimpleWindow;
    /// <summary>
    /// SimpleWindow를 어떻게 사용할 것인지 저장
    /// </summary>
    public UseSimpleWindow eHowToUse;
    /// <summary>
    /// MainCanvas의 transform 미리 파싱해놓음
    /// </summary>
    [HideInInspector]
    public Transform MCTF;
    /// <summary>
    /// 주변을 어둡게하기 위한 배경
    /// </summary>
    public GameObject BG_Black;
    
    //
    // 맵에서 행동을 하기 위한 변수
    //    
    /// <summary>
    /// 터치로 선택한 노드를 저장하는 역할
    /// </summary>
    public MapNode gSelectNode;
    /// <summary>
    /// 터치로 선택한 플레이어의 노드를 저장하는 역할
    /// </summary>
    public MapNode gSelectMyNode;
    /// <summary>
    /// 터치로 선택한 적의 노드를 저장하는 역할
    /// </summary>
    public MapNode gSelectEnemyNode;
    /// <summary>
    /// 이동 명령시 목적지를 저장하는 변수
    /// </summary>
    public MapNode gMoveDest = null;
    /// <summary>
    /// 현재 공격 명령 중인지 확인하는 bool 변수
    /// </summary>
    [HideInInspector]
    public bool bAttackCommand = false;
    /// <summary>
    /// 현재 이동 명령 중인지 확인하는 bool 변수
    /// </summary>
    [HideInInspector]
    public bool bMoveCommand = false;
    /// <summary>
    /// 현재 선택된 노드가 플레이어의 노드인지 확인
    /// </summary>
    [HideInInspector]
    public bool bIsEnemyNode = false;
    /// <summary>
    /// 화면 확대 속도
    /// </summary>
    [HideInInspector]
    public float fZoomSpeed = .005f;
    /// <summary>
    /// 화면 이동 속도
    /// </summary>
    [HideInInspector]
    public float fMoveSpeed = .001f;
    /// <summary>
    /// 선택한 전열의 번호를 저장함
    /// </summary>
    public int iSelectSpace;
    /// <summary>
    /// 현재 행동이 실행중인지 저장해주는 변수
    /// </summary>
    public bool bOnAction;
    /// <summary>
    /// 이동 명령시 이동색으로 길을 변경하기 위한 Material
    /// </summary>
    public Material MTMove;
    /// <summary>
    /// 공격 명령시 공격색으로 길을 변경하기 위한 Material
    /// </summary>
    public Material MTAttack;
    
    //
    // 팝업 메세지 관련 변수
    //
    /// <summary>
    /// 팝업창을 미리 파싱해놓음
    /// </summary>
    public GameObject gPopupMsg;
    /// <summary>
    /// Lerp 시작
    /// </summary>
    private float fStart;
    /// <summary>
    /// Lerp 끝
    /// </summary>
    private float fEnd;
    /// <summary>
    /// Lerp 를 진행하는데 걸리는 시간
    /// </summary>
    private float fAnimTime = .7f;
    /// <summary>
    /// Lerp 진행을 위한 실시간 변수
    /// </summary>
    private float fTime;
    /// <summary>
    /// 현재 팝업창이 떠있는지 확인
    /// </summary>
    private bool bIsPlaying = false;
    /// <summary>
    /// 팝업창의 Coroutine을 담는 변수 >> StopCoroutine 사용을 위함
    /// </summary>
    private Coroutine cPopupCoroutine = null;

    //
    // Tutorial >> 수정 필요 ★
    //
    [SerializeField]
    public List<Sprite> TutImg;
    public Image ShowTutImg;
    public bool bIsTut;
    public int TutRate;


    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// 공격 명령을 내렸을 때 작동하는 함수
    /// </summary>
    /// <param name="hits">선택된 노드</param>
    public void AttackCommand(RaycastHit2D hits)
    {
        string msg;

        // 근처의 노드인지 확인
        if (CheckIsNear(hits.collider.GetComponent<MapNode>())
            &&
            hits.collider.GetComponent<MapNode>().iAffiliation != TurnManager.instance.currentPlayer.playerCountry)
        {
            Order temp = new Order(
                    gSelectNode.lArmy[iSelectSpace],
                    hits.collider.GetComponent<MapNode>().lArmy[0],
                    hits.collider.GetComponent<MapNode>(),
                    gSelectNode
                    );

            // 명령 List에 공격 명령을 추가한다
            TurnManager.lTurnend.Add(temp);
            // 해당 군대를 행동대기 상태로 변경
            gSelectNode.bIsItArmy[iSelectSpace] = ArmyState.Ready;
            // 노드 사이의 길 색을 변경함
            temp.FindLine().GetComponent<LineRenderer>().material = MTAttack;

            // 주변의 화면 효과 제거
            BG_Black.SetActive(false);
            for (int i = 0; i < gSelectNode.lNearNodes.Count; i++)
            {
                gSelectNode.lNearNodes[i].GetComponent<SpriteRenderer>().sortingLayerName = "MapNode";
            }

            if (gMyArmyInfoWindow.GetComponent<ArmyInfoWindow>().IsActive)
                gMyArmyInfoWindow.GetComponent<ArmyInfoWindow>().Button_Active();
            if (gEnemyArmyInfoWindow.GetComponent<ArmyInfoWindow>().IsActive)
                gEnemyArmyInfoWindow.GetComponent<ArmyInfoWindow>().Button_Active();


            // 공격 선택 효과 해제
            bAttackCommand = false;
            WActiveWindow = WindowName.Empty;

            msg = "공격 명령 완료";
        }
        // 자신의 국가에 소속된 노드인지 확인
        else if (hits.collider.GetComponent<MapNode>().iAffiliation 
            == TurnManager.instance.currentPlayer.playerCountry)
        {
            msg = "적의 노드에만 진격 가능합니다";
        }
        // 근처의 노드가 아니라는 팝업 생성
        else
        {
            msg = "근처의 노드만 진격 가능합니다";
        }

        // 알맞는 Msg 출력
        StaticFadePopup(msg);
    }

    /// <summary>
    /// 이동 명령을 내렸을 때 작동하는 함수
    /// </summary>
    /// <param name="hits">선택된 노드</param>
    public void MoveCommand(RaycastHit2D hits)
    {
        string msg;

        // 자기 소속의 알맞은 노드를 선택했을 때
        if (CheckIsAffiliated(hits.collider.GetComponent<MapNode>()))
        {
            Order temp = new Order(
                    OrderNum.Move,
                    gSelectNode.lArmy[iSelectSpace],
                    gMoveDest,
                    gSelectNode
                    );

            // 이동이 시작된다
            TurnManager.lTurnend.Add(temp);
            // 해당 군대를 행동대기 상태로 변경
            gSelectNode.bIsItArmy[iSelectSpace] = ArmyState.Ready;
            // 노드 사이의 길 색을 변경함
            temp.FindLine().GetComponent<LineRenderer>().material = MTMove;

            // 어두운 효과를 제거, 하이라이트에서 효과에서 뺀다
            BG_Black.SetActive(false);
            for (int i = 0; i < TurnManager.instance.currentPlayer.playerMapnodeList.Count; i++)
            {
                TurnManager.instance.currentPlayer.playerMapnodeList[i].GetComponent<SpriteRenderer>().sortingLayerName
                    = "MapNode";
            }

            // 플레이어의 군대 정보창이 열려있다면 닫아준다
            if (gMyArmyInfoWindow.activeSelf)
                gMyArmyInfoWindow.GetComponent<ArmyInfoWindow>().Button_Active();            

            // 이동명령이 종료되었음을 알림
            bMoveCommand = false;
            WActiveWindow = WindowName.Empty;
            msg = "이동 명령 완료";
        }
        // 자신 소속의 노드가 아닐 때
        else
        {
            msg = "자신의 노드에만 이동할 수 있습니다";
        }

        // 알맞는 Msg 출력
        StaticFadePopup(msg);
    }

    /// <summary>
    /// 선택된 노드가 주변노드인지 확인해주는 함수
    /// </summary>
    /// <param name="select">선택된 노드</param>
    /// <returns>주변 노드면 true, 아니면 false를 반환한다</returns>
    public bool CheckIsNear(MapNode select)
    {
        bool isNear = false;

        for (int i = 0; i < gSelectNode.lNearNodes.Count; i++)
        {
            if (select == gSelectNode.lNearNodes[i])
            {
                isNear = true;
                break;
            }
        }

        return isNear;
    }

    /// <summary>
    /// 선택된 노드가 해당 턴 유저의 노드인지 확인해주는 함수
    /// </summary>
    /// <param name="select">선택된 노드</param>
    /// <returns>해당 유저의 노드이면 true, 아니면 false를 반환한다</returns>
    public bool CheckIsAffiliated(MapNode select)
    {
        bool isAffiliated = false;

        if(select.iAffiliation == TurnManager.instance.currentPlayer.playerCountry)
        {
            isAffiliated = true;
            gMoveDest = select;
        }

        return isAffiliated;
    }

    /// <summary>
    /// 선택된 노드의 창 정보 갱신해주는 함수
    /// </summary>
    /// <param name="temp">충돌로 터치입력된 오브젝트</param>
    public void RefreshWindow(RaycastHit2D hits)
    {
        // 저장된 노드를 선택한 노드로 갱신
        gSelectNode = hits.collider.GetComponent<MapNode>();

        // 선택한 노드가 본인의 노드가 아닐경우
        if (TurnManager.instance.currentPlayer.playerCountry
            != hits.collider.GetComponent<MapNode>().iAffiliation)
        {
            bIsEnemyNode = true;
            gSelectEnemyNode = gSelectNode;

            if(gNodeWindow.activeSelf)
                gNodeWindow.SetActive(false);

            // 적의 병력 UI창이 등장해야함
            // 선택한 적의 노드가 저장되어 병력이 표시되어야함
            var temp = gEnemyArmyInfoWindow.GetComponent<ArmyInfoWindow>();

            if (!bAttackCommand)
            {
                if (temp.IsActive)
                {
                    temp.RefreshWindow(gSelectNode.lArmy[0], gSelectNode.bIsScout);
                }
                else
                {
                    temp.Button_Active();
                }
            }

            return;
        }

        // 창이 활성화 되어있지 않다면
        if (!gNodeWindow.activeSelf)
        {
            // 창을 활성화
            gNodeWindow.SetActive(true);
            WActiveWindow = WindowName.NodeWindow;            
        }
        else
        {
            NodeWindow.instance.SettingWindow();
        }

        // 저장된 노드와 선택된 노드가 다를 경우
        if (gSelectNode != hits.collider.gameObject)
        {
            // 카메라를 이동 (보간이동)
            StartCoroutine(StartLerpCamera(gSelectNode.transform.position));
        }
        // 창의 위치를 선택한 노드로 변경
        gNodeWindow.transform.position = gSelectNode.transform.position;        
        // 창을 화면 기준 좌표로 변환, 저장
        gNodeWindow.transform.position = Camera.main.WorldToScreenPoint(gSelectNode.transform.position);
        // 창을 현재 확대된 크기에 맞게 변경
        gNodeWindow.transform.localScale = new Vector3(
            2.2f / Camera.main.orthographicSize,
            2.2f / Camera.main.orthographicSize,
            1);
    }

    /// <summary>
    /// SimpleWindow를 작동시키는 함수
    /// </summary>
    /// <param name="_inputHow">어떻게 사용할 것인가</param>
    public void UseSimpleWindow(UseSimpleWindow _inputHow)
    {
        eHowToUse = _inputHow;
        gSimpleWindow.SetActive(true);
        WActiveWindow = WindowName.SimpleWindow;
    }

    /// <summary>
    /// 노드 선택시 부드러운 화면 전환 효과를 위하 코루틴
    /// </summary>
    /// <param name="vTargetPos"> 목표 위치 </param>
    /// <returns> 카메라가 부드럽게 움직임 </returns>
    public IEnumerator StartLerpCamera(Vector3 vTargetPos)
    {
        // 카메라가 움직이는 속도 (Slerp 마지막 변수)
        float fCameraTrackingSpeed = 0;
        // 움직이기 전 카메라의 위치
        Vector3 vLastTargetPos  = Vector3.zero;
        // 목표 위치
        Vector3 vCurrentTargetPos = Vector3.zero;

        // 카메라 이동 시작
        while (fCameraTrackingSpeed < 1)
        {
            // 움직이기 전 카메라의 위치를 가져옴
            vLastTargetPos = Camera.main.transform.position;
            // 목표위치를 설정함
            vCurrentTargetPos = vTargetPos + new Vector3(0, 0, -10);
            // 조금씩 증가하여 끝점에 도달
            fCameraTrackingSpeed += 0.2f;

            // 보간 이동 시작
            //Camera.main.transform.position = Vector3.Lerp(vLastTargetPos, vCurrentTargetPos, fCameraTrackingSpeed);   // 선형 보간
            Camera.main.transform.position = Vector3.Slerp(vLastTargetPos, vCurrentTargetPos, fCameraTrackingSpeed);    // 원형 보간

            gNodeWindow.transform.position = Camera.main.WorldToScreenPoint(gSelectNode.transform.position);

            yield return null;
        }

        // 코루틴으로 이동하므로 창의 위치가 틀어지는 경우 대비, 도착 후 창의 위치를 다시 한번 조정해준다
        //gNodeWindow.transform.position = Camera.main.WorldToScreenPoint(gSelectNode.transform.position);
        yield return null;
    }

    /// <summary>
    /// 팝업 창 사용
    /// </summary>
    /// <param name="msg">팝업 창에 띄울 내용</param>
    /// <returns></returns>
    IEnumerator StartFadePopup(string msg)
    {
        bIsPlaying = true;

        fStart = 0f;
        fEnd = 0.65f;
        fTime = 0f;
        
        //gActivePopup = Instantiate(gPopupMsg, MCTF);
        gPopupMsg.SetActive(true);

        gPopupMsg.transform.Find("Text").GetComponent<Text>().text = msg;

        Color popupColor = new Color(0, 0, 0, 0);
        Color popupTextColor = new Color(1, 1, 1, 0);

        popupColor.a = Mathf.Lerp(fStart, fEnd, fTime);
        popupTextColor.a = Mathf.Lerp(fStart, 1f, fTime);

        while (popupColor.a < 0.65f)
        {
            fTime += Time.deltaTime / fAnimTime;

            popupColor.a = Mathf.Lerp(fStart, fEnd, fTime);
            popupTextColor.a = Mathf.Lerp(fStart, 1f, fTime);

            gPopupMsg.transform.Find("Panel").GetComponent<Image>().color = popupColor;
            gPopupMsg.transform.Find("Text").GetComponent<Text>().color = popupTextColor;

            yield return null;
        }

        yield return new WaitForSeconds(.5f);
        fTime = 0f;

        while (popupColor.a > 0f)
        {
            fTime += Time.deltaTime / fAnimTime;

            popupColor.a = Mathf.Lerp(fEnd, fStart, fTime);
            popupTextColor.a = Mathf.Lerp(1f, fStart, fTime);

            gPopupMsg.transform.Find("Panel").GetComponent<Image>().color = popupColor;
            gPopupMsg.transform.Find("Text").GetComponent<Text>().color = popupTextColor;

            yield return null;
        }

        bIsPlaying = false;
        gPopupMsg.SetActive(false);
        yield return null;
    }

    /// <summary>
    /// 팝업 창 사용 (전역)
    /// </summary>
    /// <param name="msg">팝업 창에 띄울 내용</param>
    public void StaticFadePopup(string msg)
    {
        if (bIsPlaying)
        {
            bIsPlaying = false;
            StopCoroutine(cPopupCoroutine);
            gPopupMsg.SetActive(false);
        }
        cPopupCoroutine = StartCoroutine(StartFadePopup(msg));
    }

    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        // instance를 사용하기 위한 싱글톤 제작
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);

        // MainCanvas Transform 미리 파싱해놓음
        MCTF = GameObject.Find("Canvas").transform;
    }

    private void Start()
    {
        TutRate = 0;

        WActiveWindow = WindowName.Empty;

        bOnAction = false;
    }

    private void Update()
    {
        // 터치가 입력 되었을 때만 작동
        if (Input.touchCount > 0 && !bOnAction)
        {
            // 만약 터치된게 UI일 경우 그 뒤의 오브젝트를 선택하지 않고 return 시킨다.
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }
            else
            {
                // 어떤 노드가 터치되었는지 확인하기 위한 Raycast 작동, 터치 값 저장
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                Ray2D ray2D = new Ray2D(
                    new Vector2(ray.origin.x, ray.origin.y),
                    new Vector2(ray.direction.x, ray.direction.y)
                    );
                RaycastHit2D hits = Physics2D.Raycast(ray2D.origin, ray2D.direction);

                // 검은 배경을 터치되는 경우 아무 일도 일어나지 않아야 한다
                if (hits.collider != null && hits.collider.tag == "DefenceTouch") return;

                // 현재 터치가 입력된 갯수에 따라 switch문으로 운용
                switch (Input.touchCount)
                {
                    // 단일 터치
                    // 화면 이동, 노드 선택 등이 실행되는 부분
                    // switch문을 사용하여 터치 상태에 바로 대응할 수 있도록 구현
                    case 1:
                        switch (Input.GetTouch(0).phase)
                        {
                            // 터치가 막 시작되었을때
                            case TouchPhase.Began:
                                if (hits.collider != null)   // 무언가를 터치함
                                {
                                    // 현재 공격 명령을 내리는 중인가?
                                    if (bAttackCommand)
                                    {
                                        AttackCommand(hits);
                                    }
                                    // 현재 이동 명령을 내리는 중인가?
                                    else if (bMoveCommand)
                                    {
                                        MoveCommand(hits);
                                    }
                                    // 평상시 맵을 보고있다면 노드가 선택되도 맵에서의 역할이 진행되어야 한다.
                                    else
                                    {
                                        RefreshWindow(hits);
                                    }
                                }
                                else // 빈 공간을 터치함 
                                {
                                    // 빈 공간을 두번 터치하는 경우 열려있는 창이 닫혀야한다
                                    if (Input.GetTouch(0).tapCount == 2)
                                    {
                                        gNodeWindow.SetActive(false);
                                        WActiveWindow = WindowName.Empty;
                                    }
                                }

                                break;

                            // 터치후 유지, 움직이기 시작하였을 때
                            case TouchPhase.Moved:
                                // Touch.deltaPosition : 현재 위치와 이전의 위치의 차이를 저장, 즉 얼마나 이동했는지를 저장한다.
                                Vector2 vDeltaPos = Input.GetTouch(0).deltaPosition;
                                // 저장된 움직인 거리를 이용하여 반대방향으로 카메라의 위치를 이동시켜 준다
                                Camera.main.transform.Translate(-vDeltaPos.x * fMoveSpeed, -vDeltaPos.y * fMoveSpeed, 0);
                                break;

                            default:
                                break;
                        }

                        break;



                    // 멀티 터치
                    // 화면 확대, 축소 등이 실행되는 부분
                    case 2:
                        // 첫, 두번째 터치를 저장할 변수 선언
                        Touch tZero = Input.GetTouch(0);
                        Touch tOne = Input.GetTouch(1);

                        // 원래 위치에서 deltaTime만큼 움직인 위치를 빼서 이전 프레임의 터치 위치를 저장
                        Vector2 vtZeroPrevPos = tZero.position - tZero.deltaPosition;
                        Vector2 vtOnePrevPos = tOne.position - tOne.deltaPosition;

                        // 이전 프레임에서 두 터치의 점이 얼마나 벌어져있는지 확인
                        float fPrevTouchDeltaMag = (vtZeroPrevPos - vtOnePrevPos).magnitude;
                        // 현재 프레임에서 두 터치의 점이 얼마나 벌어져있는지 확인
                        float fTouchDeltaMag = (tZero.position - tOne.position).magnitude;
                        // 이전과 현재의 프레임 사이의 거리를 구한다
                        float fDeltaMagnitudeDiff = fPrevTouchDeltaMag - fTouchDeltaMag;

                        // 화면의 확대 크기를 두 터치의 점이 벌어진 크기 * 확대 속도를 곱하여 확대시킨다
                        Camera.main.orthographicSize += fDeltaMagnitudeDiff * fZoomSpeed;
                        // 그리고 화면의 확대는 0보다 작아지면 이미지가 반전되므로 0보다 작아지지 않도록 한다.
                        // 화면을 확대할 때 0.5 이상으로 확대되지 않게 고정시키고,
                        // 화면을 축소할 때 5 이상으로 축소되지 않게 고정시킨다.
                        if (Camera.main.orthographicSize <= 2.2f) Camera.main.orthographicSize = 2.2f;
                        else if (Camera.main.orthographicSize >= 7) Camera.main.orthographicSize = 7;
                        // 노드창이 현재 확대된 크기에 맞게 변경
                        if (gNodeWindow != null)
                            gNodeWindow.transform.localScale = new Vector3(
                                2.2f / Camera.main.orthographicSize,
                                2.2f / Camera.main.orthographicSize,
                                1);
                        
                        break;




                    default:
                        break;
                }
            }
            // 노드창이 활성화 되있다면, 실시간으로 월드좌표기준으로 변경하여 창이 노드 위에 떠있는 효과를 줌
            if (gNodeWindow.activeSelf)
                gNodeWindow.transform.position = Camera.main.WorldToScreenPoint(gSelectNode.transform.position);
        }
    }
}
