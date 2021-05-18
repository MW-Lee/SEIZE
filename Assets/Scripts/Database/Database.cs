////////////////////////////////////////////////
//
// Database
//
// 게임에서 제공되는 아이템, 군사 등
// 정보를 모아놓은 스크립트
// 엑셀파일로 정보를 받아올 수 있게 바뀌어야함.
// 19. 03. 19
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전역 상수를 모아놓은 클래스
/// </summary>
public class Constant
{
    public const float fTime = 0.3f;
    public const float fWindowTime = 0.5f;

    public const int iSoldierEmpty = 12;
    public const int iSoldierUnknown = 13;

    /// <summary>
    /// 비어있는 군사 칸
    /// </summary>
    public static readonly Army aEmptyArmy = new Army()
    {
        commander = Database.instance.dCommanderList["Empty"],
        soldier = Database.instance.lSoldierList[iSoldierEmpty],
        count = 0,
        bIsCommander = false
    };
    /// <summary>
    /// 비어있는 전열
    /// </summary>
    public static readonly Army[] aEmptyArmies = new Army[4]
    {
        aEmptyArmy,aEmptyArmy,aEmptyArmy,aEmptyArmy
    };

    /// <summary>
    /// 대표 초상화인 지휘관을 찾는 함수
    /// </summary>
    /// <param name="_inputArmies">지휘관을 찾을 군대</param>
    /// <returns>지휘관이 있는 칸 번호\n없는 경우 -1</returns>
    public static int FindCommander(Army[] _inputArmies)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_inputArmies[i].bIsCommander)
                return i;
        }

        return -1;
    }
    
    /// <summary>
    /// 지휘관이 없을 경우 대표로 쓸 병사를 찾는 함수
    /// </summary>
    /// <param name="_inputArmies">병사를 찾을 군대</param>
    /// <returns>병사가 존재하는 칸 번호</returns>
    public static int FindSoldier(Army[] _inputArmies)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_inputArmies[i].count != 0)
                return i;
        }
        return -1;
    }
}

/// <summary>
/// 전체적인 데이터를 보유하는 클래스
/// </summary>
public class Database : MonoBehaviour
{
    #region 변수
        
    /// <summary>
    /// 데이터베이스를 쓰기 위한 instance
    /// </summary>
    static public Database instance;
    /// <summary>
    /// 군사 정보를 담을 List형 Database
    /// </summary>
    public List<Soldiers> lSoldierList = new List<Soldiers>();
    /// <summary>
    /// 지휘관 정보를 담을 Dictionary형 Database
    /// </summary>
    public Dictionary<string, Commander> dCommanderList = new Dictionary<string, Commander>();
    /// <summary>
    /// 게임 진행에 사용되는 퀘스트들을 담을 Dictionary형 Database
    /// </summary>
    public Dictionary<string, List<Quest>> dQuestList = new Dictionary<string, List<Quest>>();

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    /// <summary>
    /// 병사 Database 생성
    /// </summary>
    public void MakeSoldierList()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Soldier");

        for (int i = 0; i < data.Count; i++)
        {
            Soldiers temp = new Soldiers();
            Stats tempstat = new Stats();

            temp.sSoldierName = data[i]["name"].ToString();

            switch (data[i]["class"].ToString())
            {
                case "Empty":
                    temp.eSoldierType = SoldierType.Empty;
                    break;

                case "Unknown":
                    temp.eSoldierType = SoldierType.Unknown;
                    break;

                case "W":
                    temp.eSoldierType = SoldierType.Warrior;
                    break;

                case "M":
                    temp.eSoldierType = SoldierType.Magician;
                    break;                   

                default:
                    break;
            }

            int.TryParse(data[i]["ad"].ToString(), out tempstat.AD);
            int.TryParse(data[i]["ap"].ToString(), out tempstat.AP);
            float.TryParse(data[i]["hp"].ToString(), out tempstat.HP);
            tempstat.Total = tempstat.AD + tempstat.AP + (int)tempstat.HP;
            temp.sSoldierStat = tempstat;

            temp.sSoldierInfo = data[i]["info"].ToString();

            temp.iSoldierImg = Resources.Load("Char/" + temp.sSoldierName, typeof(Sprite)) as Sprite;
            temp.iSoldierImgBig = Resources.Load("Char_Big/" + temp.sSoldierName, typeof(Sprite)) as Sprite;

            lSoldierList.Add(temp);
        }
    }

    /// <summary>
    /// 지휘관 전용 Dictionary Database 생성
    /// </summary>
    public void MakeCommanderList()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Commander");

        for(int i = 0; i < data.Count; i++)
        {
            Commander temp = new Commander();
            Stats tempStats = new Stats();

            temp.sCommanderName = data[i]["name"].ToString();

            switch (data[i]["type"])
            {
                case "Knight":
                    temp.sCommanderType = CommanderType.Knight;
                    break;

                case "Mercenary":
                    temp.sCommanderType = CommanderType.Mercenary;
                    break;

                case "Empty":
                    temp.sCommanderType = CommanderType.Empty;
                    break;
            }

            switch(data[i]["rate"])
            {
                case "Bronze":
                    temp.sCommanderRate = CommanderRate.bronze;
                    break;

                case "Silver":
                    temp.sCommanderRate = CommanderRate.silver;
                    break;

                case "gold":
                    temp.sCommanderRate = CommanderRate.gold;
                    break;

                case "platinum":
                    temp.sCommanderRate = CommanderRate.platinum;
                    break;

                case "Empty":
                    temp.sCommanderRate = CommanderRate.Empty;
                    break;
            }

            switch (data[i]["class"])
            {
                case "Assassin":
                    temp.sCommanderClass = CommanderClass.Assassin;
                    break;

                case "Magician":
                    temp.sCommanderClass = CommanderClass.Magician;
                    break;

                case "Empty":
                    temp.sCommanderClass = CommanderClass.Empty;
                    break;
            }

            int.TryParse(data[i]["ad"].ToString(), out tempStats.AD);
            int.TryParse(data[i]["ap"].ToString(), out tempStats.AP);
            float.TryParse(data[i]["hp"].ToString(), out tempStats.HP);
            tempStats.Total = (int)tempStats.HP + tempStats.AD + tempStats.AP;
            temp.sCommanderStat = tempStats;

            temp.sCommanderInfo = data[i]["info"].ToString();

            int[] skilltemp = new int[3];
            for(int j = 0; j < 3; j++)
            {
                skilltemp[j] = int.Parse(data[i]["skill" + (j + 1)].ToString());
            }

            temp.iCommanderSkillnum = skilltemp;

            temp.sCommanderImg = Resources.Load("Com/" + temp.sCommanderName, typeof(Sprite)) as Sprite;
            temp.sCommanderImgBig = Resources.Load("Com_Big/" + temp.sCommanderName, typeof(Sprite)) as Sprite;

            dCommanderList.Add(temp.sCommanderName, temp);
        }

        //
        // 지속적인 지휘관 정보 업데이트 필요 ★
        //
        //dCommanderList.Add()
    }
    
    /// <summary>
    /// 이벤트 / 퀘스트를 모아놓는 Database 생성
    /// </summary>
    public void MakeQuestList()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Quest - Lucerne");

        List<Quest> tempQuestList = new List<Quest>();

        for(int i = 0; i < data.Count; i++)
        {
            Quest temp = new Quest(data[i]);

            tempQuestList.Add(temp);
        }

        dQuestList.Add("Test", tempQuestList);        
    }

    /// <summary>
    /// x축으로 움직이는 창 공통 Coroutine
    /// </summary>
    /// <param name="_windowTF">움직이고자 하는 창</param>
    /// <param name="_inputDest">목표지점</param>
    /// <returns>창의 움직임</returns>
    public static IEnumerator StartWindowMove(Transform _windowTF, float _inputDest)
    {
        float fTime = 0;

        if ((_windowTF.position.x - _inputDest) < 0)
        {
            while(true)
            {
                fTime += Time.deltaTime / .5f;

                _windowTF.position = new Vector3(
                    Mathf.Lerp(_windowTF.position.x, _inputDest, fTime),
                    _windowTF.position.y,
                    _windowTF.position.z
                    );
                yield return null;

                if(_windowTF.position.x > (_inputDest - 5))
                {
                    _windowTF.position = new Vector3(
                        _inputDest,
                        _windowTF.position.y,
                        _windowTF.position.z
                        );
                    yield return null;
                    break;
                }
            }
        }
        else
        {
            while (true)
            {
                fTime += Time.deltaTime / Constant.fWindowTime;

                _windowTF.position = new Vector3(
                    Mathf.Lerp(_windowTF.position.x, _inputDest, fTime),
                    _windowTF.position.y,
                    _windowTF.position.z
                    );
                yield return null;

                if (_windowTF.position.x < (_inputDest + 5))
                {
                    _windowTF.position = new Vector3(
                        _inputDest,
                        _windowTF.position.y,
                        _windowTF.position.z
                        );
                    break;
                }
            }
        }

        yield return null;
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Awake()
    {
        // 데이터베이스 instance를 사용하기 위한 싱글톤 제작
        // 현재 게임오브젝트를 싱글톤으로 설정 및 중복생성 방지
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);


        if (lSoldierList.Count == 0) 
        {
            MakeSoldierList();
            MakeCommanderList();
            MakeQuestList();
        }
    }

    #endregion
}
