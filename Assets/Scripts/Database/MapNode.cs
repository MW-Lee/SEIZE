////////////////////////////////////////////////
//
// MapNode
//
// 맵에서 이용되는 노드를 위한 스크립트
// 19. 02. 18
// MWLee
////////////////////////////////////////////////
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 해당 노드의 종류가 무엇인지 나타낼 열거형 변수
/// </summary>
public enum NodeType
{
    /// <summary>
    /// 수도
    /// </summary>
    Capital,

    /// <summary>
    /// 도시
    /// </summary>
    City,

    /// <summary>
    /// 마을
    /// </summary>
    Village
}

/// <summary>
/// 맵의 각 노드들이 갖고 있는 정보
/// </summary>
public class MapNode : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 노드 이름
    /// </summary>
    public string sName;
    /// <summary>
    /// 노드 소속 국가
    /// </summary>
    public Country iAffiliation;
    /// <summary>
    /// 노드 유형
    /// </summary>
    public NodeType iType;
    /// <summary>
    /// 노드 전투력 (성벽 방어력 + 병사 전투력)
    /// </summary>
    public int iPower;
    /// <summary>
    /// 노드 성벽 레벨
    /// </summary>
    public short sWallLV;
    /// <summary>
    /// 노드 성벽 방어력
    /// </summary>
    public int iDefensive;
    /// <summary>
    /// 노드 병사 전투력
    /// </summary>
    public int iSoldiers;
    /// <summary>
    /// 노드 우호도
    /// </summary>
    public int iSatisfaction;
    /// <summary>
    /// 노드 병사 제한수
    /// </summary>
    public int iMaximumSoldier;
    /// <summary>
    /// 근처 노드들의 정보
    /// </summary>
    public List<MapNode> lNearNodes;
    /// <summary>
    /// 선을 그리기 위해 바로 근처의 노드들의 Transform만 저장한 List
    /// </summary>
    [HideInInspector]
    public List<Transform> TFNearNodes;
    /// <summary>
    /// 근처 노드 중 같은 소속인 노드들만 저장한 List
    /// </summary>
    [HideInInspector]
    public List<MapNode> lNearMyNodes;
    /// <summary>
    /// 근처 노드 중 다른 소속인 노드들만 저장한 List
    /// </summary>
    [HideInInspector]
    public List<MapNode> lNearEnemyNodes;
    /// <summary>
    /// 마을에 주둔중인 군대 정보
    /// </summary>
    [SerializeField]
    public Army[][] lArmy;
    //public Armies[] lArmy;
    /// <summary>
    /// 현재 마을에 군대가 주둔중인지 확인시켜주는 변수
    /// </summary>
    [HideInInspector]
    public bool bIsArmy;
    /// <summary>
    /// 현재 마을에 군대가 각 어떤 상태인지 저장하는 변수
    /// </summary>
    public ArmyState[] bIsItArmy = new ArmyState[4]
    {
        ArmyState.None,
        ArmyState.None,
        ArmyState.None,
        ArmyState.None
    };
    /// <summary>
    /// 해당 노드가 탐색이 되었는지 확인시켜주는 변수
    /// </summary>
    public bool bIsScout;
    /// <summary>
    /// 노드 아이콘 옆에 보이는 UI들을 사용하기 위한 Transform 파싱
    /// </summary>
    private Transform WorldUI;
    public int iFrindship;
    public int iHostility;


    //
    // 기타 변수
    //
    /// <summary>
    /// Outline을 그리기 위한 SpriteRenderer
    /// </summary>
    private SpriteRenderer sr;
    /// <summary>
    /// Outline을 그리기 위한 Color
    /// </summary>
    private Color cNodeColor;
    /// <summary>
    /// 길을 그리기 위한 Material
    /// </summary>
    public Material MTLine;

    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// 근처의 노드들을 정리해주는 함수
    /// </summary>
    public void SettingNearNodes()
    {
        for(int i = 0; i < lNearNodes.Count; i++)
        {
            if (lNearNodes[i].iAffiliation != this.iAffiliation)
                lNearEnemyNodes.Add(lNearNodes[i]);
            else
                lNearMyNodes.Add(lNearNodes[i]);
        }
    }

    public void RefreshWorldUI()
    {
        iFrindship = 10;
        iHostility = 1;

        WorldUI.Find("Friendship").GetChild(1).GetComponent<UnityEngine.UI.Text>().text
            = iFrindship.ToString() + " / 20";

        WorldUI.Find("Hostility").GetChild(1).GetComponent<UnityEngine.UI.Text>().text
            = iHostility.ToString() + " / 20";


        WorldUI.Find("World_Name").GetChild(0).GetComponent<UnityEngine.UI.Text>().text
            = sName;
    }

    /// <summary>
    /// 노드의 전투력을 바로 계산함
    /// 해당 노드의 성벽 방어력 + 병사 전투력
    /// </summary>
    public void SumPower()
    {
        for(int i=0;i<lArmy.Length;i++)
        {
            //iSoldiers += lArmy[i].iSoldierPower * lArmy[i].iSoldierCount;
            //iSoldiers += lArmy[i].soldier.iSoldierPower * lArmy[i].count;
        }

        iPower = iDefensive + iSoldiers;
    }
    
    /// <summary>
    /// 노드 방어력 계산
    /// </summary>
    public void SumDefensive()
    {
        iDefensive = 1000 * sWallLV;
    }

    /// <summary>
    /// 해당 노드가 정찰됐는지 확인해주는 함수
    /// </summary>
    public void CheckIsScouted()
    {
        if (iAffiliation == TurnManager.instance.currentPlayer.playerCountry)
            bIsScout = true;
        else
            bIsScout = false;
    }

    /// <summary>
    /// 얕은 복사로 인해 정보가 날아가므로, 깊은 복사 함수 생성
    /// 19.06.30 >> 왜 작동하지 않는 것인가.
    /// </summary>
    /// <returns>복사된 정보</returns>
    public MapNode DeepCopy()
    {
        MapNode temp = new MapNode
        {
            sName = this.sName,
            iAffiliation = this.iAffiliation,
            iType = this.iType,
            iPower = this.iPower,
            iDefensive = this.iDefensive,
            iSoldiers = this.iSoldiers,
            iSatisfaction = this.iSatisfaction,
            iMaximumSoldier = this.iMaximumSoldier,
            lNearNodes = this.lNearNodes,
            lArmy = this.lArmy,
            //bIsArmy = this.bIsArmy,
            bIsItArmy = this.bIsItArmy            
        };

        return temp;
    }

    /// <summary>
    /// 각 노드들 사이에 선을 동적으로 생성해주는 함수
    /// </summary>
    public void DrawLine()
    {
        if (lNearNodes.Count == 0) return;

        LineRenderer lr;

        for(int i = 0; i < lNearNodes.Count; i++)
        {
            TFNearNodes.Add(lNearNodes[i].gameObject.transform);

            if (TFNearNodes[i].Find(this.name)) continue;

            GameObject temp = new GameObject(TFNearNodes[i].name);
            temp.transform.SetParent(this.transform, false);
            temp.transform.SetAsLastSibling();

            lr = temp.AddComponent<LineRenderer>();
            lr.material = MTLine;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, TFNearNodes[i].position);
            lr.sortingLayerName = "BG";
            lr.startWidth = .3f;
            lr.endWidth = .3f;            
        }

        return;
    }

    /// <summary>
    /// 해당 노드에 군사가 주둔중인지 확인하는 함수
    /// </summary>
    /// <returns>군사의 유무</returns>
    public bool CheckIsInArmy()
    {
        for (int i = 0; i < 4; i++)
        {
            if (bIsItArmy[i] != ArmyState.None)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 해당 노드의 해당 칸에 군사가 주둔중인지 확인하는 함수
    /// </summary>
    /// <param name="_index">확인하고싶은 칸</param>
    /// <returns>군사의 유무</returns>
    public bool CheckIsInArmy(int _index)
    {
        if (bIsItArmy[_index] != ArmyState.None)
            return true;
        else
        {
            for(int i = 0; i < 4; i++)
            {
                if (lArmy[_index][i].soldier != Database.instance.lSoldierList[Constant.iSoldierEmpty]
                    ||
                    lArmy[_index][i].commander != Database.instance.dCommanderList["Empty"])
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 게임이 시작될 때 빈 군대를 배치해놓고 시작
    /// </summary>
    public void MakeEmptyArmy()
    {
        Army[] EmptyArmy = Constant.aEmptyArmies;

        if (iType == NodeType.Capital)
        {
            // 플레이어가 결제한 배열 갯수만큼 생성되어야함 ★
            lArmy = new Army[4][]
            {
                EmptyArmy,EmptyArmy,EmptyArmy,EmptyArmy
            };
        }
        else
        {
            lArmy = new Army[1][]
            {
                EmptyArmy
            };
        }
    }

    /// <summary>
    /// 처음에 노드의 외곽선을 그리기 위한 함수
    /// </summary>
    public void StartOutline()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        sr.GetPropertyBlock(mpb);

        switch (iAffiliation)
        {
            case Country.ED:
                cNodeColor = Color.green;
                break;

            case Country.CA:
                cNodeColor = Color.red;
                break;

                //case Country.QU:
                //case Country.EE:
                //case Country.FF:
        }

        mpb.SetFloat("_Outline", 1f);
        mpb.SetColor("_OutlineColor", cNodeColor);
        mpb.SetFloat("_OutlineSize", 3);

        sr.SetPropertyBlock(mpb);
    }

    /// <summary>
    /// 노드의 외곽선을 갱신하기 위한 함수
    /// </summary>
    public void UpdateOutline()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        sr.GetPropertyBlock(mpb);

        switch (iAffiliation)
        {
            case Country.ED:
                cNodeColor = Color.green;
                break;

            case Country.CA:
                cNodeColor = Color.red;
                break;

            //case Country.QU:
            //case Country.EE:
            //case Country.FF:
        }

        mpb.SetColor("_OutlineColor", cNodeColor);

        sr.SetPropertyBlock(mpb);
    }

    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        WorldUI = transform.GetChild(0);

        StartOutline();
    }

    private void Start()
    {
        MakeEmptyArmy();
        SettingNearNodes();
        RefreshWorldUI();

        // 테스트를 위한 적 노드에 병사 생성 ★
        if(transform.parent.name == "CA_Node")
        {
            Army[] temp = new Army[4];

            for(int i = 0; i < 2; i++)
            {
                temp[i] = new Army(Database.instance.lSoldierList[i], 10);
                temp[i + 2] = Constant.aEmptyArmy;
            }
            temp[3] = new Army(Database.instance.lSoldierList[4], 1);

            lArmy[0] = temp;

            //bIsArmy = true;
            bIsItArmy[0] = ArmyState.Stay;
        }

        sWallLV = 1;

        SumDefensive();
        //SumPower();

        CheckIsScouted();

        DrawLine();
    }

    private void Update()
    {
        
    }
}
