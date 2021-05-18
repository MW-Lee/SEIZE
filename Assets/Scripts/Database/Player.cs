////////////////////////////////////////////////
//
// Player
//
// 턴을 진행할 때 필요한 플레이어의 정보
// 19. 05. 30
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당 노드의 소속 국가를 나타내주는 열거형 변수
/// </summary>
public enum Country
{
    /// <summary>
    /// 에란드
    /// </summary>
    ED,

    /// <summary>
    /// 카드레아
    /// </summary>
    CA,

    /// <summary>
    /// 쿼?
    /// </summary>
    QU,

    /// <summary>
    /// 미정
    /// </summary>
    EE,

    /// <summary>
    /// 미정
    /// </summary>
    FF
}

[System.Serializable]
public class Player
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 유저 이름
    /// </summary>
    public string userName;
    /// <summary>
    /// 유저가 고른 진영
    /// </summary>
    public Country playerCountry;
    /// <summary>
    /// 유저가 가지고 있는 거점 리스트
    /// </summary>
    public List<MapNode> playerMapnodeList;
    /// <summary>
    /// 유저가 소유하고 있는 자원
    /// </summary>
    public int coast;
    /// <summary>
    /// 현재 턴이 플레이어인지 AI인지 구분해주는 Bool 변수
    /// </summary>
    public bool bIsuser;
    /// <summary>
    /// 보유 전열 갯수
    /// </summary>
    public int iPossessFormCount;

    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    public Player()
    {

    }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="name">이름</param>
    /// <param name="country">진영</param>
    /// <param name="mapNodes">리스트</param>
    /// <param name="isuser">유저인가</param>
    public Player(string name, Country country, List<MapNode> mapNodes, bool isuser, int FormCount)
    {
        userName = name;
        playerCountry = country;
        playerMapnodeList = mapNodes;
        bIsuser = isuser;
        iPossessFormCount = FormCount;
    }

    public virtual void TurnUpdate()
    {
        // 턴을 넘기는 함수
        // 플레이어와 AI에서 다르게 작동해야 한다
    }

}
