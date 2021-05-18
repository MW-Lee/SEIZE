////////////////////////////////////////////////
//
// FindRoadAI
//
// 턴마다 자동으로 움직이게 하는 AI 연습
// 19. 02. 20
// MWLee
////////////////////////////////////////////////
using UnityEngine;
using System.Collections.Generic;

public class FindRoadAI : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 공격 하고자 하는 노드 
    /// </summary>
    private MapNode mAttack;

    /// <summary>
    /// 플레이어 턴이 끝났는지 확인
    /// </summary>
    private bool bIsPlayerEnd;
    
    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// 해당 국가에 소속된 노드를 전부 받아오는 함수
    /// </summary>
    /// <param name="country"> 갖고 오고싶은 국가의 번호</param>
    public List<MapNode> GetCountryNode(int country)
    {
        /// <summary>
        /// 반환하기 위한 임시 리스트
        /// </summary>
        List<MapNode> lmResult = new List<MapNode>();

        /// <summary>
        /// 모든 노드들을 검색하기 위해 모든 노드의 부모인 "Node"를 가져온다
        /// </summary>
        GameObject gNode = GameObject.Find("Node");

        /// <summary>
        /// Node의 자식을 임시로 담기위한 변수
        /// </summary>
        MapNode mTemp = new MapNode();
        
        /// <summary>
        /// 노드 전체의 소속을 확인하여 해당 소속과 맞는 노드들을 저장함
        /// </summary>
        for (int i = 0; i < gNode.transform.childCount; i++)
        {
            mTemp = gNode.transform.GetChild(i).GetComponent<MapNode>();

            if((int)mTemp.iAffiliation == country)
            {
                lmResult.Add(mTemp);
            }   
        }

        return lmResult;
    }

    /// <summary>
    /// 근처의 노드와 전투력을 비교하는 함수
    /// </summary>
    public void CompareNearNode()
    {
        
    }


    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Start()
    {
        bIsPlayerEnd = false;
    }

    private void Update()
    { 
        /// 플레이어의 턴이 끝났을 때 나머지 AI들이 작동한다
        if(bIsPlayerEnd)
        {
            for (int i = 1; i < 5; i++)
            {

            }
        }
    }
}