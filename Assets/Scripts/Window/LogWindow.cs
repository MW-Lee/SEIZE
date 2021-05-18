////////////////////////////////////////////////
//
// LogWindow
//
// 로그창에서 작동하기 위한 스크립트
// 
// 19. 09. 20
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 기록이 남는 로그 형식
/// </summary>
public class Log
{
    #region 변수

    /// <summary>
    /// 로그 내용
    /// </summary>
    public string sContent;
    /// <summary>
    /// 로그 이미지
    /// </summary>
    public Sprite sImg;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수
    /// <summary>
        /// 어떠한 행동이 일어나서 기록을 남기는 함수
        /// </summary>
        /// <param name="_inputOrder">행동</param>
        /// <returns>저장될 로그</returns>
    public static Log AddLog(Order _inputOrder)
    {        
        // 반환될 로그
        Log result = new Log();

        // 입력되는 명령에 따라서 로그 내용이 달라짐
        switch (_inputOrder.order)
        {
            case OrderNum.Move:
                result.sImg = LogWindow.instance.sMove;
                result.sContent =
                    _inputOrder.mStartPoint.sName +
                    " 에서 " +
                    _inputOrder.mDestination.sName +
                    //" 으로 " + 
                    // 부대정보 +
                    " 으로 군대가 이동하였습니다";
                break;

            case OrderNum.Attack:
                result.sImg = LogWindow.instance.sAttack;
                result.sContent =
                    _inputOrder.mStartPoint.iAffiliation.ToString() +
                    "의 " +
                    _inputOrder.mStartPoint.sName +
                    "에서 " +
                    _inputOrder.mDestination.iAffiliation.ToString() +
                    "의 " +
                    _inputOrder.mDestination.sName +
                    // "를 공격하여"
                    // 이겼는지 유무 bool 변수
                    // 하였습니다.
                    "로 공격을 시작하였습니다";
                break;

            case OrderNum.Production:

                break;

            case OrderNum.UpgradeWall:

                break;

            case OrderNum.Scout:
                result.sImg = LogWindow.instance.sScout;
                result.sContent =
                    _inputOrder.mStartPoint.sName +
                    "에서 탐색이 시전되었습니다";
                break;

            default:
                break;
        }


        return result;
    }

    // 퀘스트도 로그에 찍혀야함 ★
    #endregion
}

/// <summary>
/// 로그 창에서 작동되는 클래스
/// </summary>
public class LogWindow : MonoBehaviour
{
    #region 변수
    /// <summary>
    /// LogWindow를 외부에서 사용하기 위한 instance
    /// </summary>
    public static LogWindow instance;

    /// <summary>
    /// 작은 로그창1 활성화 상태
    /// </summary>
    public static bool bSmall1 = false;
    /// <summary>
    /// 작은 로그창2 활성화 상태
    /// </summary>
    public static bool bSmall2 = false;

    /// <summary>
    /// 작은 로그창1
    /// </summary>
    public GameObject gSmall1;
    /// <summary>
    /// 작은 로그창2
    /// </summary>
    public GameObject gSmall2;

    /// <summary>
    /// 작은 로그창을 띄우기 위해 선택한 로그를 저장
    /// </summary>
    public Log lSelectLog;

    //
    // 창에서 변경되어야할 시각적인 ui들
    //
    /// <summary>
    /// 로그가 표시되는 오브젝트의 Transform
    /// </summary>
    public Transform tfLogContent;
    /// <summary>
    /// 날짜가 표시되는 오브젝트의 Transform
    /// </summary>
    public Transform tfDayContent;
    /// <summary>
    /// 맨 위의 글자
    /// </summary>
    public Text      tTitle;

    /// <summary>
    /// 알파값 조정을 위한 로그 표시 오브젝트의 CanvasGroup
    /// </summary>
    public CanvasGroup cgLogContent;
    /// <summary>
    /// 알파값 조정을 위한 날짜 표시 오브젝트의 CanvasGroup
    /// </summary>
    public CanvasGroup cgDayContent;

    /// <summary>
    /// Log의 Prefab
    /// </summary>
    public GameObject gLogPrefab;
    /// <summary>
    /// 날짜의 Prefab
    /// </summary>
    public GameObject gDayPrefab;

    /// <summary>
    /// 이동 명령의 이미지
    /// </summary>
    public Sprite sMove;
    /// <summary>
    /// 공격 명령의 이미지
    /// </summary>
    public Sprite sAttack;
    /// <summary>
    /// 퀘스트 행동의 이미지
    /// </summary>
    public Sprite sQuest;
    /// <summary>
    /// 정찰 행동의 이미지
    /// </summary>
    public Sprite sScout;

    /// <summary>
    /// 해당 날짜를 보기위한 선택한 날짜를 저장
    /// </summary>
    private int iSelectDay;
    /// <summary>
    /// 날짜 <> 로그 실행 코루틴 저장
    /// </summary>
    private Coroutine cChangeCoroutine = null;
    /// <summary>
    /// 날짜 >> Log 변환중인가?
    /// </summary>
    private bool IsChangeToLog = false;
    /// <summary>
    /// 현재 LogWindow가 활성화 되어있는가?
    /// </summary>
    public bool IsLogWindowShow = false;
    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수
    /// <summary>
    /// LogWindow에 날이 진행될 수록 날짜를 추가함
    /// </summary>
    public void AddDayCount()
    {
        GameObject result = Instantiate(gDayPrefab, tfDayContent, false); ;
        string temp = DataManager.instance.iDay.ToString();

        result.transform.GetChild(0).GetComponent<Text>().text = temp + " 일차";
        result.GetComponent<Button>().onClick.AddListener(() => Button_Day());
        result.name = temp;

        DataManager.instance.lLogList.Add(new List<Log>());
        //result.GetComponent<Button>().onClick.AddListener(delegate { Button_Day(); });
    }

    /// <summary>
    /// 날짜가 눌렸을 때 작동하는 함수
    /// </summary>
    public void Button_Day()
    {
        iSelectDay = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

        // 선택된 날짜에 아무 행동도 일어나지 않았을 경우
        if (DataManager.instance.lLogList[iSelectDay].Count == 0)
        {
            InputManager.instance.StaticFadePopup("해당 일에는 아무 일도 일어나지 않았습니다");
        }
        else
        {
            IsChangeToLog = true;
            cChangeCoroutine = StartCoroutine(DayToLogFade());
        }
    }

    /// <summary>
    /// Log가 눌렸을 때 작동하는 함수
    /// </summary>
    public void Button_Log()
    {
        // 선택된 로그를 저장함
        lSelectLog =
            DataManager.instance.lLogList[iSelectDay][int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name)];

        // 활성화 되어있는 작은 로그창에 맞추어 표시됨
        if (!bSmall1)
        {
            gSmall1.SetActive(true);
            bSmall1 = true;
        }
        else if (!bSmall2 && bSmall1)
        {
            gSmall2.SetActive(true);
            bSmall2 = true;
        }
        else if(bSmall1 && bSmall2)
        {

        }
    }

    /// <summary>
    /// LogWindow의 뒤로가기 버튼에서 작동하는 함수
    /// </summary>
    public void Button_LogBack()
    {
        // 현재 Log가 활성화 되어있다면, 날짜로 나와야함
        if (IsChangeToLog)
        {
            IsChangeToLog = false;
            cChangeCoroutine = StartCoroutine(DayToLogFade());
        }
        // 날짜가 활성화 되어있다면, 로그창이 사라져야 함
        else if(!IsChangeToLog && IsLogWindowShow)
        {
            //StartCoroutine(StartLogMove());
            StartCoroutine(Database.StartWindowMove(transform.parent, -650));
            IsLogWindowShow = false;
        }
    }

    /// <summary>
    /// 선택된 날짜의 Log들을 띄워줌
    /// </summary>
    /// <param name="_inputDay">선택된 날짜</param>
    public void RefreshLog(int _inputDay)
    {
        // 자주쓰이는 Datamanager의 instance를 파싱해놓음
        var temp = DataManager.instance;

        // 해당 날짜의 로그 갯수만큼 작동
        for (int i = 0; i < temp.lLogList[_inputDay].Count; i++)
        {
            // 해당 날짜의 로그 갯수보다 만들어져잇는 로그가 많다면
            // 로그를 따로 생성할 필요 없이 기존의 로그를 갱신시켜 사용
            if (tfLogContent.childCount >= (i + 1))
            {
                // 기존의 로그를 가져와 저장하기 위한 변수
                Transform result = null;
                
                if (!tfLogContent.GetChild(i).gameObject.activeInHierarchy)
                    tfLogContent.GetChild(i).gameObject.SetActive(true);

                result = tfLogContent.GetChild(i);

                // 가져온 로그의 이미지로 변경
                result.GetChild(0).GetComponent<Image>().sprite =
                    temp.lLogList[_inputDay][i].sImg;

                // 만약 로그 내용이 대략 2줄을 넘어갈 경우 뒤는 요약한다
                string inputContent = string.Empty;
                if (temp.lLogList[_inputDay][i].sContent.Length > 38)
                {
                    inputContent = temp.lLogList[_inputDay][i].sContent.Substring(0, 35);
                    inputContent += "..";
                }
                else
                {
                    inputContent = temp.lLogList[_inputDay][i].sContent;
                }

                // 가져온 로그의 내용으로 변경
                result.transform.GetChild(1).GetComponent<Text>().text = inputContent;
            }
            // 반대로 만들어져 있는 로그가 더 적어서 새로 만들어줌
            else
            {
                // 새로운 로그를 생성
                GameObject result = Instantiate(gLogPrefab, tfLogContent, false);
                // 쉬운 사용을 위해 이름을 숫자로 변경
                result.name = (tfLogContent.childCount - 1).ToString();
                // 로그가 눌렸을때 작동할 OnClick 함수를 스크립트 상으로 넣어줌
                // Inspector에서는 보이지 않음! ★
                result.GetComponent<Button>().onClick.AddListener(() => Button_Log());
                // 가져온 로그의 이미지로 변경
                result.transform.GetChild(0).GetComponent<Image>().sprite =
                    temp.lLogList[_inputDay][i].sImg;
                // 로그 내용이 2줄을 넘는지 확인
                string inputContent = string.Empty;
                if(temp.lLogList[_inputDay][i].sContent.Length > 38)
                {
                    inputContent = temp.lLogList[_inputDay][i].sContent.Substring(0, 35);
                    inputContent += "..";
                }
                else
                {
                    inputContent = temp.lLogList[_inputDay][i].sContent;
                }
                // 가져온 로그의 내용으로 변경
                result.transform.GetChild(1).GetComponent<Text>().text = inputContent;
            }
        }

        // 해당 날짜에 저장된 로그보다 많은 로그가 생성되어 있을때 남는 로그는 꺼줘야 한다
        if (tfLogContent.childCount != 0
            &&
            tfLogContent.childCount > temp.lLogList[_inputDay].Count)
        {
            for(int i = (tfLogContent.childCount - temp.lLogList[_inputDay].Count); 
                i >= temp.lLogList[_inputDay].Count; 
                i--)
            {
                tfLogContent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ScrollRect 사용을 위해 작동하는 함수
    /// </summary>
    public void SwitchScrollContent()
    {
        // 현재 로그 창에서 보여지는 오브젝트가 드래그 가능하도록 변경해줌
        if (IsChangeToLog)
        {
            transform.GetComponentInChildren<ScrollRect>().content = tfLogContent.GetComponent<RectTransform>();
        }
        else
        {
            transform.GetComponentInChildren<ScrollRect>().content = tfDayContent.GetComponent<RectTransform>();
        }
    }

    /// <summary>
    /// LogAggregate 전체를 움직이는 Coroutine
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartLogMove()
    {
        // 로그 전체를 움직이기 위해 받아놓는 부모(LogAggregate) Transform
        Transform LogTF = transform.parent;
        float fTime = 0;

        if(IsLogWindowShow)
        {
            while(LogTF.position.x > -650)
            {
                fTime += Time.deltaTime / .5f;

                LogTF.position = new Vector3(
                    Mathf.Lerp(LogTF.position.x, -650, fTime),
                    LogTF.position.y,
                    LogTF.position.z
                    );

                yield return null;

                if (LogTF.position.x < -645)
                {
                    LogTF.position = new Vector3(
                    -650,
                    LogTF.position.y,
                    LogTF.position.z
                    );
                    break;
                }
            }
        }
        else
        {            
            while (LogTF.position.x < 0)
            {
                fTime += Time.deltaTime / .5f;

                LogTF.position = new Vector3(
                    Mathf.Lerp(LogTF.position.x, 0, fTime),
                    LogTF.position.y,
                    LogTF.position.z
                    );

                yield return null;

                if (LogTF.position.x > -5)
                {
                    LogTF.position = new Vector3(
                    0,
                    LogTF.position.y,
                    LogTF.position.z
                    );
                    break;
                }
            }
        }

        // 현재 LogWindow창이 보이는지 확인시켜줌
        IsLogWindowShow ^= true;
        yield return null;
    }

    /// <summary>
    /// 날짜 <> Log 변환 Coroutine
    /// </summary>
    /// <returns></returns>
    public IEnumerator DayToLogFade()
    {
        // 날짜와 로그창이 전환되어야 하므로 변경하기 쉬운 알파값만 미리 받아놓음
        float logAlpha = cgLogContent.alpha;
        float dayAlpha = cgDayContent.alpha;

        float fTime = 0;

        // 날짜에서 로그로 전환
        if (IsChangeToLog)
        {
            cgDayContent.alpha = Mathf.Lerp(1, 0, fTime);

            while (cgDayContent.alpha > 0)
            {
                fTime += Time.deltaTime / .3f;

                dayAlpha = Mathf.Lerp(1, 0, fTime);
                cgDayContent.alpha = dayAlpha;

                yield return null;

                if(cgDayContent.alpha < 0.1f)
                {
                    cgDayContent.alpha = 0;
                    break;
                }
            }
            cgDayContent.gameObject.SetActive(false);
            cgLogContent.gameObject.SetActive(true);

            // 로그들을 해당 날짜에 맞게 갱신시킴
            RefreshLog(iSelectDay);
            fTime = 0;

            yield return new WaitForSeconds(.5f);

            cgLogContent.alpha = Mathf.Lerp(0, 1, fTime);

            while (cgLogContent.alpha < 1)
            {
                fTime += Time.deltaTime / .3f;

                logAlpha = Mathf.Lerp(0, 1, fTime);
                cgLogContent.alpha = logAlpha;

                yield return null;

                if(cgLogContent.alpha > 0.94f)
                {
                    cgLogContent.alpha = 1;
                    break;
                }
            }
        }
        // 로그에서 날짜로 전환
        else
        {
            cgLogContent.alpha = Mathf.Lerp(1, 0, fTime);
            

            while (cgLogContent.alpha > 0)
            {
                fTime += Time.deltaTime / .3f;

                logAlpha = Mathf.Lerp(1, 0, fTime);
                cgLogContent.alpha = logAlpha;

                yield return null;

                if(cgLogContent.alpha < 0.1f)
                {
                    cgLogContent.alpha = 0;
                    break;
                }
            }
            cgDayContent.gameObject.SetActive(true);
            cgLogContent.gameObject.SetActive(false);

            fTime = 0;

            yield return new WaitForSeconds(.5f);

            cgDayContent.alpha = Mathf.Lerp(0, 1, fTime);
            while (cgDayContent.alpha < 1)
            {
                fTime += Time.deltaTime / .3f;

                dayAlpha = Mathf.Lerp(0, 1, fTime);
                cgDayContent.alpha = dayAlpha;

                yield return null;

                if(cgDayContent.alpha > 0.94f)
                {
                    cgDayContent.alpha = 1;
                    break;
                }
            }
        }

        // 날짜나 로그중 보이는 것으로 드래그 가능하게 변경
        SwitchScrollContent();
        yield return null;
    }
    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        // 시작하면 우선 0일을 추가
        AddDayCount();
    }

    private void Update()
    {
        
    }


    #endregion
}
