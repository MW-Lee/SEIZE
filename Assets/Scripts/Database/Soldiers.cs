////////////////////////////////////////////////
//
// Soldiers
//
// 맵에서 이용되는 군사 정보를 위한 스크립트
// 19. 03. 11
// MWLee
////////////////////////////////////////////////
using UnityEngine;

[System.Serializable]
public class Soldiers
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 군사 고유 식별 ID
    /// </summary>
    public SoldierType eSoldierType;
    /// <summary>
    /// 군사 이름
    /// </summary>
    public string sSoldierName;
    /// <summary>
    /// 군사 설명
    /// </summary>
    public string sSoldierInfo;
    /// <summary>
    /// 군사 스탯
    /// </summary>
    public Stats sSoldierStat;
    /// <summary>
    /// 군사 사진
    /// </summary>
    public Sprite iSoldierImg;
    /// <summary>
    /// 군사 사진 큰 버전
    /// </summary>
    public Sprite iSoldierImgBig;

    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
}

[System.Serializable]
public class Commander
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 지휘관 이름
    /// </summary>
    public string sCommanderName;
    /// <summary>
    /// 지휘관 정보
    /// </summary>
    public string sCommanderInfo;
    /// <summary>
    /// 용병 / 기사 정보
    /// </summary>
    public CommanderType sCommanderType;
    /// <summary>
    /// 지휘관 등급
    /// </summary>
    public CommanderRate sCommanderRate;
    /// <summary>
    /// 지휘관 직업
    /// </summary>
    public CommanderClass sCommanderClass;
    /// <summary>
    /// 지휘관 스탯
    /// </summary>
    public Stats sCommanderStat;
    /// <summary>
    /// 지휘관 사진
    /// </summary>
    public Sprite sCommanderImg;
    /// <summary>
    /// 지휘관 사진 큰버전
    /// </summary>
    public Sprite sCommanderImgBig;
    /// <summary>
    /// 스킬 함수 번호 저장
    /// </summary>
    public int[] iCommanderSkillnum;


    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
}

/// <summary>
/// 병사가 모인 군대의 정보
/// </summary>
[System.Serializable]
public struct Army
{
    /// <summary>
    /// 어떤 지휘관
    /// </summary>
    public Commander commander;
    /// <summary>
    /// 어느 병사가
    /// </summary>
    public Soldiers soldier;
    /// <summary>
    /// 해당 칸의 전투력
    /// </summary>
    public Stats stat;
    /// <summary>
    /// 몇 명 있는가
    /// </summary>
    public int count;
    /// <summary>
    /// 이 칸은 지휘관인가?
    /// </summary>
    public bool bIsCommander;

    //
    // 생성자
    //
    /// <summary>
    /// 지휘관용 생성자
    /// </summary>
    /// <param name="_com">넣을 지휘관</param>
    public Army(Commander _com)
    {
        commander = _com;
        soldier = Database.instance.lSoldierList[Constant.iSoldierEmpty];
        count = 1;
        bIsCommander = true;

        // 추후 장비의 스탯을 더하여 계산되어야 함
        // commander 자체에서 아이템을 착용하여 계산된다면? ★
        stat = commander.sCommanderStat;        
    }

    /// <summary>
    /// 병사용 생성자
    /// </summary>
    /// <param name="_soldiers">넣을 병사</param>
    /// <param name="_count">병사의 수</param>
    public Army(Soldiers _soldiers, int _count)
    {
        commander = Database.instance.dCommanderList["Empty"];
        soldier = _soldiers;
        count = _count;
        bIsCommander = false;

        stat = new Stats(
            soldier.sSoldierStat.AD * count,
            soldier.sSoldierStat.AP * count,
            soldier.sSoldierStat.HP * count);
    }

    /// <summary>
    /// 복사용 생성자
    /// </summary>
    /// <param name="_input">복사할 군대</param>
    public Army(Army _input)
    {
        commander = _input.commander;
        soldier = _input.soldier;
        stat = _input.stat;
        count = _input.count;
        bIsCommander = _input.bIsCommander;
    }
}

/// <summary>
/// 전투에서 사용하기 위한 스탯 구조체
/// </summary>
[System.Serializable]
public struct Stats
{
    public int AD;
    public int AP;
    public float HP;
    public int Total;

    public Stats(int _ad, int _ap, float _hp)
    {
        AD = _ad;
        AP = _ap;
        HP = _hp;

        Total = _ad + _ap + (int)_hp;
    }
}

/// <summary>
/// 군사 종류를 나타내기 위한 열거형 변수
/// </summary>
public enum SoldierType
{
    Empty,
    Unknown,
    Warrior,
    Magician,

    Max
}

/// <summary>
/// 지휘관 타입 구분
/// </summary>
public enum CommanderType
{
    Knight,
    Mercenary,

    Empty
}

/// <summary>
/// 지휘관 전직 구분
/// </summary>
public enum CommanderClass
{
    SwordMan,
    Assassin,
    Archer,
    Gunner,
    Magician,


    Empty,
    Max
}

/// <summary>
/// 지휘관 등급 구분
/// </summary>
public enum CommanderRate
{
    bronze,
    silver,
    gold,
    platinum,


    Empty
}

/// <summary>
/// 군대의 상태를 나타내주는 열거형 변수
/// </summary>
public enum ArmyState
{
    Stay,
    Ready,
    None
}