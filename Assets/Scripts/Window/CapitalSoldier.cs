////////////////////////////////////////////////
//
// CapitalSoldier
//
// 수도 창의 병사들이 가지는 스크립트
// 19. 02. 22
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CapitalSoldier : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 선택 되었던 노드
    /// </summary>
    public MapNode SelectedNode;
    /// <summary>
    /// 선택 된 노드의 종휴
    /// </summary>
    public NodeType SelectedNodeType;
    /// <summary>
    /// Transform 미리 파싱해놓음
    /// </summary>
    private Transform TF;

    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 터치한 노드에 군사가 없을경우 작동되지 않아야함
        if (!SelectedNode.CheckIsInArmy(int.Parse(name)))
        {
            InputManager.instance.StaticFadePopup("이동할 군대가 존재하지 않습니다.");
            return;
        }
        // 터치한 군대가 행동 대기 중인 군대이면
        else if(SelectedNode.bIsItArmy[int.Parse(name)] == ArmyState.Ready)
        {
            //행동 취소 창
            InputManager.instance.UseSimpleWindow(UseSimpleWindow.CancelOrder);
            InputManager.instance.iSelectSpace = int.Parse(name);
            return;
        }

        InputManager.instance.gSelectMyNode = InputManager.instance.gSelectNode;

        // 터치한 노드의 종류에 따라서 행동이 달라져야 함
        // 현재 수도는 이동만, 도시는 공격만 가능하게 설정함 ★
        switch (SelectedNodeType)
        {
            case NodeType.Capital:
                Touch_Capital();
                break;

            case NodeType.City:
            case NodeType.Village:
                Touch_City();
                break;

            default:
                break;
        }

    }

    /// <summary>
    /// 터치한 노드가 수도일 경우
    /// </summary>
    private void Touch_Capital()
    {
        Player CurrentTurn = TurnManager.instance.currentPlayer;
        
        // 이동 명령을 위해 자신의 노드에만 하이라이트 효과를 준다
        for (int i = 0; i < CurrentTurn.playerMapnodeList.Count; i++)
        {
            CurrentTurn.playerMapnodeList[i].GetComponent<SpriteRenderer>().sortingLayerName = "Highlight";
        }

        // 이동 명령을 실행중 임을 알림
        InputManager.instance.bMoveCommand = true;

        Touch_After();
    }

    /// <summary>
    /// 터치한 노드가 도시일 경우
    /// </summary>
    private void Touch_City()
    {
        // 주변 노드중에 적인 노드가 없다면 문구를 띄우고 아무 행동도 하지 않음
        if(InputManager.instance.gSelectNode.lNearEnemyNodes.Count == 0)
        {
            InputManager.instance.StaticFadePopup("공격 가능한 노드가 없습니다");
            return;
        }

        // 공격 명령을 위해 적의 노드만 하이라이트 효과를 준다
        for (int i = 0; i < InputManager.instance.gSelectNode.lNearEnemyNodes.Count; i++)
        {
            InputManager.instance.gSelectNode.lNearEnemyNodes[i].GetComponent<SpriteRenderer>().sortingLayerName = "Highlight";
        }

        // 공격 명령을 실행중 임을 알림
        InputManager.instance.bAttackCommand = true;

        Touch_After();
        return;
    }

    private void Touch_Villiage()
    {

    }

    private void Touch_After()
    {
        // 열려있던 맵 노드 명령창을 닫음
        InputManager.instance.gNodeWindow.SetActive(false);
        InputManager.instance.WActiveWindow = WindowName.AttackMove;
        // 이동 명령 하이라이트를 위하여 주변 배경을 어둡게
        InputManager.instance.BG_Black.SetActive(true);

        // 몇번째 전열을 선택했는지 알림
        InputManager.instance.iSelectSpace = int.Parse(name);
        // 정보창 활성화
        Touch_Soldier();
    }

    /// <summary>
    /// 군사를 선택했을 때 작동하는 함수
    /// </summary>
    private void Touch_Soldier()
    {
        if (InputManager.instance.gMyArmyInfoWindow.GetComponent<ArmyInfoWindow>().IsActive)
        {
            InputManager.instance.gMyArmyInfoWindow.GetComponent<ArmyInfoWindow>().
                RefreshWindow(
                InputManager.instance.gSelectNode.lArmy[int.Parse(name)],
                InputManager.instance.gSelectNode.bIsScout
                );

        }
        else
        {
            InputManager.instance.gMyArmyInfoWindow.GetComponent<ArmyInfoWindow>().Button_Active();
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        TF = this.transform;
    }


    private void OnEnable()
    {
        SelectedNode = InputManager.instance.gSelectNode;
        SelectedNodeType = SelectedNode.iType;

        TF.localScale = Vector3.one;
    }

}
