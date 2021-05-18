////////////////////////////////////////////////
//
// NewFormationWindow
//
// 개편된 전열창에서 작동하는 스크립트
// 19. 12. 18
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewFormation : MonoBehaviour, 
    IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    #region 변수

    /// <summary>
    /// 전열창을 외부에서 사용하기 위한 instance
    /// </summary>
    public static NewFormation instance;

    //
    // 회전 UI를 위한 변수
    //
    /// <summary>
    /// 선택 슬라이드를 돌리기 위한 Transform
    /// </summary>
    public Transform SlideTF;
    /// <summary>
    /// 현재 슬라이드가 돌아가고 있는지 확인시켜주는 변수
    /// </summary>
    public bool bIsitMove;
    /// <summary>
    /// 얼마나 돌아갔는지 확인시켜주는 변수
    /// </summary>
    public float fTime;

    /// <summary>
    /// 위로 돌림을 알리는 방향
    /// </summary>
    private readonly int iUp   = 1;
    /// <summary>
    /// 아래로 돌림을 알리는 방향
    /// </summary>
    private readonly int iDown = -1;
    /// <summary>
    /// 어디로 돌릴 것인지 기억하는 변수
    /// </summary>
    private int iInputDir = 0;

    //
    // 전열 배치를 위한 변수
    //
    /// <summary>
    /// 소유한 병사, 지휘관, 장비가 나타나는 곳
    /// </summary>
    public Transform ListTF;
    /// <summary>
    /// 드래그시 따라다닐 그림
    /// </summary>
    public Transform ItemCloneTF;
    /// <summary>
    /// 전열 칸이 보이는 곳
    /// </summary>
    public Transform ArmySlotTF;
    /// <summary>
    /// 전열 칸 왼편의 정보창
    /// </summary>
    public Transform InfoTF;
    /// <summary>
    /// 전열 번호 버튼이 모여있는 곳
    /// </summary>
    public Transform FormationBtnTF;
    /// <summary>
    /// 전투력 총합을 표시할 부분이 모여있는 곳
    /// </summary>
    public Transform TotalPowerTF;
    /// <summary>
    /// 수정사항이 발생하는 경우 나오는 확정버튼
    /// </summary>
    public Button btn_Confirm;
    /// <summary>
    /// 드래그시 주변을 어둡게 해주는 이미지
    /// </summary>
    public GameObject gBG_Black;
    /// <summary>
    /// 병사 숫자 입력하는 창
    /// </summary>
    public GameObject gCountWindow;
    /// <summary>
    /// 행동 대기중인 군대는 편집할 수 없게 막아주는 판넬
    /// </summary>
    public GameObject gBlock;
    /// <summary>
    /// 빈 슬룻 이미지
    /// </summary>
    public Sprite sEmptySlot;
    /// <summary>
    /// 뒤에 크게 보여지는 이미지 화면
    /// </summary>
    public Image imgRepresent;
    /// <summary>
    /// 드래그하고 있는 아이템의 번호를 저장하는 변수
    /// </summary>
    public int iSelectItem = -1;
    /// <summary>
    /// 넣고싶은 칸을 저장하는 역할
    /// </summary>
    public int iSelectSlot;
    /// <summary>
    /// 아이템 이미지로부터 포인터가 나갔는지 확인해주는 변수
    /// </summary>
    public bool bIsPointerOut = false;
    /// <summary>
    /// 아이템을 드래그중인지 확인해주는 변수
    /// </summary>
    public bool bSelectItem = false;
    /// <summary>
    /// 전열 칸을 드래그중인지 확인해주는 변수
    /// </summary>
    public bool bSelectSlot = false;
    /// <summary>
    /// 전열에서 수정사항이 있는지 확인해주는 변수
    /// </summary>
    public bool bIsChange = false;
    /// <summary>
    /// 전열 전환시에 창이 호출되는지 확인시켜주는 변수
    /// </summary>
    public bool bCallbyForm = false;
    /// <summary>
    /// 현재 드래그중인지 확인시켜주는 변수
    /// </summary>
    public bool bIsDraging = false;

    /// <summary>
    /// 수정중인 전열을 임시로 저장하는 변수
    /// </summary>
    public Army[] ChangingArmies;
    private Army[] BeforeArmies;
    /// <summary>
    /// 현재 List가 무슨 칸인지 확인해주는 변수
    /// </summary>
    private string sNowList = string.Empty;
    /// <summary>
    /// 현재 수정중인 전열이 몇번째 전열인지 저장하는 변수
    /// </summary>
    private int iSelectFormationNum;
    /// <summary>
    /// 넘어가고 싶은 전열의 번호를 저장해놓는 역할
    /// </summary>
    private int iSelectNextFormationNum;
    /// <summary>
    /// 주변이 어두워지는 효과가 켜졌는지 확인해주는 변수
    /// </summary>
    private bool bIsBlackActive = false;

    //
    // 전열 변경 버튼을 위한 오브젝트를 받아놓음
    //
    /// <summary>
    /// 현재 보고있는 전열이 아닐 때
    /// </summary>
    private Sprite[] sBtnOff;
    /// <summary>
    /// 현재 보고있는 전열일 때
    /// </summary>
    private Sprite[] sBtnOn;

    //
    // 상세 정보창
    //
    /// <summary>
    /// 지휘관용 상세정보창
    /// </summary>
    public GameObject gCommanderInfoWindow;
    /// <summary>
    /// 군사용 상세정보창
    /// </summary>
    public GameObject gSoldierInfoWindow;

    //
    // List 페이지 효과
    //
    /// <summary>
    /// PageUp 버튼
    /// </summary>
    public Button btn_PageUp;
    /// <summary>
    /// PageDown 버튼
    /// </summary>
    public Button btn_PageDown;
    /// <summary>
    /// Page 상태를 보여주는 텍스트
    /// </summary>
    public Text txtPageStatus;
    /// <summary>
    /// 현재 Page
    /// </summary>
    public int iCurrentPage;
    /// <summary>
    /// 전체 Page
    /// </summary>
    private int iTotalPage;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수
    
    //
    // 왼편 슬라이드 및 List 관련 함수
    //
    /// <summary>
    /// 슬라이드 버튼이 눌렸을 때 작동하는 함수
    /// </summary>
    public void Button_Slide()
    {
        if(bIsitMove)
        {
            return;
        }
        else
        {
            bIsitMove = true;

            // Down 터치시 위로 돌려서 터치한 내용을 볼 수 있어야 함
            if (EventSystem.current.currentSelectedGameObject.name == "Down")
            {
                SlideRotate(iUp);
                iInputDir = iUp;
            }
            else
            {
                SlideRotate(iDown);
                iInputDir = iDown;
            }
        }
    }
    
    /// <summary>
    /// 슬라이드를 돌리는 함수
    /// </summary>
    /// <param name="_inputDir">어느 방향으로 돌 것인가?</param>
    public void SlideRotate(int _inputDir)
    {
        int iDir = _inputDir * 35;
        int iDownDest = 360 - 35;

        if (_inputDir == iUp ? (SlideTF.localEulerAngles.z < iDir) : (SlideTF.localEulerAngles.z > iDownDest)
            ||
            SlideTF.localEulerAngles.z == 0)
        {
            fTime += Time.deltaTime;

            SlideTF.localRotation = Quaternion.Euler(
                0f,
                0f,
                Mathf.Lerp(SlideTF.localRotation.z, iDir, fTime / .3f));


            for(int i = 0;i<SlideTF.childCount; i++)
            {
                SlideTF.GetChild(i).localRotation = Quaternion.Euler(
                    0f,
                    0f,
                    Mathf.Lerp(SlideTF.localRotation.z, -iDir, fTime / .3f));
            }

            SlideTF.GetChild(2).localScale = new Vector3(
                Mathf.Lerp(SlideTF.GetChild(2).localScale.x, 1f, fTime / .5f),
                Mathf.Lerp(SlideTF.GetChild(2).localScale.y, 1f, fTime / .5f),
                1);

            int temp = _inputDir == iUp ? 3 : 1;
            SlideTF.GetChild(temp).localScale = new Vector3(
                Mathf.Lerp(SlideTF.GetChild(temp).localScale.x, 1.3f, fTime / .5f),
                Mathf.Lerp(SlideTF.GetChild(temp).localScale.y, 1.3f, fTime / .5f),
                1);
        }
        else if(_inputDir == 1 ? (SlideTF.localEulerAngles.z >= iDir) : (SlideTF.localEulerAngles.z <= iDownDest))
        {
            SlideTF.localRotation = Quaternion.Euler(Vector3.zero);

            if(_inputDir == iUp)
            {
                for (int i = 1; i < 5; i++)
                {
                    SlideTF.GetChild(i - 1).GetComponentInChildren<Text>().text =
                        SlideTF.GetChild(i).GetComponentInChildren<Text>().text;
                }

                SlideTF.GetChild(4).GetComponentInChildren<Text>().text =
                    SlideTF.GetChild(1).GetComponentInChildren<Text>().text;
            }
            else
            {
                for(int i = 4; i > 0; i--)
                {
                    SlideTF.GetChild(i).GetComponentInChildren<Text>().text =
                       SlideTF.GetChild(i - 1).GetComponentInChildren<Text>().text;
                }

                SlideTF.GetChild(0).GetComponentInChildren<Text>().text =
                   SlideTF.GetChild(3).GetComponentInChildren<Text>().text;
            }

            sNowList = SlideTF.Find("Now").GetComponentInChildren<Text>().text;
            iCurrentPage = 1;
            RefreshList();

            Transform temp;
            for(int i = 0; i < SlideTF.childCount; i++)
            {
                temp = SlideTF.GetChild(i);
                temp.localRotation = Quaternion.Euler(Vector3.zero);
                temp.localScale = Vector3.one;
            }

            SlideTF.GetChild(2).localScale = new Vector3(1.3f, 1.3f);

            fTime = 0;
            bIsitMove = false;
        }
    }

    /// <summary>
    /// 슬라이드가 돌아간 후 해당 List를 불러오는 함수
    /// </summary>
    /// <param name="_input"></param>
    public void RefreshList()
    {
        RefreshPageText();

        int Start = (iCurrentPage - 1) * 8;
        int End = iCurrentPage * 8;

        switch (sNowList)
        {
            case "지휘관":
                for (int i = Start; i < End; i++)
                {
                    if (i >= DataManager.instance.lComList.Count)
                    {
                        ListTF.GetChild(i % 8).GetComponentInChildren<Image>().sprite =
                            sEmptySlot;

                        ListTF.GetChild(i % 8).GetChild(1).GetComponentInChildren<Text>().text = "";
                        continue;
                    }

                    ListTF.GetChild(i % 8).GetComponentInChildren<Image>().sprite =
                        DataManager.instance.lComList[i].sCommanderImg;

                    ListTF.GetChild(i % 8).GetChild(1).GetComponentInChildren<Text>().text =
                        DataManager.instance.lComList[i].sCommanderName;
                }
                break;

            case "장비":
                for(int i = Start; i < End; i++)
                {
                    ListTF.GetChild(i % 8).GetComponentInChildren<Image>().sprite =
                            sEmptySlot;

                    ListTF.GetChild(i % 8).GetChild(1).GetComponentInChildren<Text>().text = "";
                }
                break;

            case "병사":
                for(int i = Start; i < End; i++)
                {
                    if (i >= Database.instance.lSoldierList.Count
                        ||
                        i == Constant.iSoldierEmpty
                        ||
                        i == Constant.iSoldierUnknown)
                    {
                        ListTF.GetChild(i % 8).GetComponentInChildren<Image>().sprite =
                            sEmptySlot;

                        ListTF.GetChild(i % 8).GetChild(1).GetComponentInChildren<Text>().text = "";
                        continue;
                    }

                    ListTF.GetChild(i % 8).GetComponentInChildren<Image>().sprite =
                        Database.instance.lSoldierList[i].iSoldierImg;

                    ListTF.GetChild(i % 8).GetChild(1).GetComponentInChildren<Text>().text =
                        Database.instance.lSoldierList[i].sSoldierName;
                }
                break;
        }
    }

    //
    // 전열 배치를 위한 터치, 드래그 관련 함수
    //
    /// <summary>
    /// 전열 번호 버튼이 눌렸을 때 작동하는 함수
    /// </summary>
    public void Button_FormationNum()
    {
        if (iSelectFormationNum == int.Parse(EventSystem.current.currentSelectedGameObject.name))
            return;

        // 눌린 번호를 저장
        iSelectNextFormationNum = int.Parse(EventSystem.current.currentSelectedGameObject.name);

        // 변경 전 전열에서 수정사항이 있으면 저장 할 것인지 확인해주는 창 출력
        if (bIsChange)
        {
            bCallbyForm = true;
            InputManager.instance.UseSimpleWindow(UseSimpleWindow.ApplyChange);
        }
        // 변경점이 없다면 바로 원하는 전열로 화면이 전환된다
        else
            AfterFormationButton();
    }

    /// <summary>
    /// 확정 버튼이 눌렸을 때 작동하는 함수
    /// </summary>
    public void Button_Confirm()
    {
        InputManager.instance.UseSimpleWindow(UseSimpleWindow.ApplyChange);
    }

    /// <summary>
    /// PageUp 버튼이 눌렸을 때 작동하는 함수
    /// </summary>
    public void Button_PageUp()
    {
        iCurrentPage--;

        if(iCurrentPage == 0)
        {
            iCurrentPage = 1;
            btn_PageUp.interactable = false;            
        }
        if (!btn_PageDown.interactable)
            btn_PageDown.interactable = true;

        RefreshList();
    }

    /// <summary>
    /// PageDown 버튼이 눌렸을 때 작동하는 함수
    /// </summary>
    public void Button_PageDown()
    {
        iCurrentPage++;

        if(iCurrentPage == iTotalPage)
        {
            btn_PageDown.interactable = false;
        }
        if (!btn_PageUp.interactable)
            btn_PageUp.interactable = true;

        RefreshList();
    }

    /// <summary>
    /// Page를 나타내는 텍스트를 갱신시켜주는 함수
    /// </summary>
    public void RefreshPageText()
    {
        switch (sNowList)
        {
            case "지휘관":
                iTotalPage = (DataManager.instance.lComList.Count / 8) + 1;
                break;

            case "병사":
                iTotalPage = (Database.instance.lSoldierList.Count / 8) + 1;
                break;

            case "장비":
                iTotalPage = 1;
                break;
        }

        if (iTotalPage == 1)
        {
            btn_PageDown.interactable = false;
            btn_PageUp.interactable = false;
        }
        else if (iTotalPage != 1 && iCurrentPage == 1)
        {
            btn_PageUp.interactable = false;
            btn_PageDown.interactable = true;
        }

        txtPageStatus.text = iCurrentPage.ToString() + " / " + iTotalPage.ToString();
    }

    /// <summary>
    /// 전열 변경 > 저장 유무 확인 후 작동하는 함수
    /// </summary>
    public void AfterFormationButton()
    {
        // 넘어가고자 하는 번호를 현재 번호로 바꿈
        iSelectFormationNum = iSelectNextFormationNum;

        // 해당 번호 전열에 군대가 존재하는지 확인 후 있으면 해당 군대로, 없으면 초기화로 세팅
        if (InputManager.instance.gSelectNode.CheckIsInArmy(iSelectFormationNum))
        {
            //ChangingArmies = new Army[4];
            if (InputManager.instance.gSelectNode.bIsItArmy[iSelectFormationNum] == ArmyState.Ready)
                gBlock.SetActive(true);
            else
                gBlock.SetActive(false);
            ChangingArmies = (Army[])InputManager.instance.gSelectNode.lArmy[iSelectFormationNum].Clone();
            RefreshFormation();
        }
        else
        {
            gBlock.SetActive(false);
            ResetFormation();
        }

        // 시각적으로 보이는 부분을 갱신
        RefreshFormationButton();
        // 변경점을 초기화 시킴
        bIsChange = false;
        btn_Confirm.gameObject.SetActive(false);
    }

    /// <summary>
    /// 현재 선택된 전열의 버튼은 하얗게 칠하고, 나머진 검은색으로 칠함
    /// </summary>
    public void RefreshFormationButton()
    {
        for(int i = 0; i < 4; i++)
        {
            if (i == iSelectFormationNum)
                FormationBtnTF.GetChild(i).GetComponent<Image>().sprite = sBtnOn[i];
            else
                FormationBtnTF.GetChild(i).GetComponent<Image>().sprite = sBtnOff[i];
        }
    }
    
    /// <summary>
    /// UI내에서 터치가 발생했을 때 작동하는 함수
    /// </summary>
    /// <param name="eventData">터치 입력값</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        // List 내의 개체를 선택했을 때만 작동하여야 함
        if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent == ListTF)
        {
            // 현재 장비가 구현되지 않았으므로 강제로 막음 ★
            switch(sNowList)
            {
                case "지휘관":
                    if (int.Parse(eventData.pointerCurrentRaycast.gameObject.transform.parent.name)
                        >= (DataManager.instance.lComList.Count % 8))
                        return;
                    break;

                case "병사":
                    if (iCurrentPage >= 2
                        &&
                        (int.Parse(eventData.pointerCurrentRaycast.gameObject.transform.parent.name) + 8)
                        >= Constant.iSoldierEmpty)
                        return;
                    break;

                case "장비":
                    return;
            }
            // 현재 List에서 드래그 했음을 나타냄
            bSelectItem = true;

            iSelectItem = (int.Parse(eventData.pointerCurrentRaycast.gameObject.transform.parent.name)
                + ((iCurrentPage - 1) * 8));

            //ListTF.GetChild(iSelectItem).GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            //ListTF.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.3f);
            ListTF.GetChild(iSelectItem).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.3f);
        }
        // List가 아니면 Slot을 선택했을 때만 작동하여야 함
        else if (eventData.pointerCurrentRaycast.gameObject.transform.parent == ArmySlotTF
            &&
            ChangingArmies[int.Parse(eventData.pointerCurrentRaycast.gameObject.name)].count != 0)
        {
            // 현재 Slot에서 드래그 했음을 나타냄
            bSelectSlot = true;
            iSelectItem = int.Parse(eventData.pointerCurrentRaycast.gameObject.name);
        }
    }

    /// <summary>
    /// UI내에서 드래그 중일 때 작동하는 함수
    /// </summary>
    /// <param name="eventData">터치 입력값</param>
    public void OnDrag(PointerEventData eventData)
    {
        if ((bSelectItem || bSelectSlot) && bIsPointerOut)
        {
            // 포인터를 따라다니는 Clone 작동
            ItemCloneTF.position = eventData.position;
        }
    }

    /// <summary>
    /// UI내에서 터치를 뗐을 때 작동하는 함수
    /// </summary>
    /// <param name="eventData">터치 입력값</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        // List에서 시작 됐다면
        if (bSelectItem)
        {
            // 끝나는 지점이 Slot이어야 함
            if (eventData.pointerCurrentRaycast.gameObject.transform.parent.name == "FormationSlot")
            {
                // 끝난 지점의 Slot칸을 기억
                iSelectSlot = int.Parse(eventData.pointerCurrentRaycast.gameObject.name);

                // 어떤 Item을 끌었는지에 따라서 작동이 달라져야함
                switch (sNowList)
                {
                    case "지휘관":
                        if (!bIsChange)
                        {
                            ChangesOccur();
                        }
                        ApplyChange(iSelectSlot, 0);
                        break;

                    case "병사":
                        // 숫자를 넣는 칸을 띄움
                        gCountWindow.SetActive(true);
                        break;

                    case "장비":
                        break;
                }

                eventData.pointerCurrentRaycast.gameObject.GetComponent<Outline>().enabled = false;
            }

            //ListTF.GetChild(iSelectItem).GetComponent<Image>().color = Color.white;
            //ListTF.GetComponentInChildren<Image>().color = Color.white;
            ListTF.GetChild(iSelectItem).GetComponentInChildren<Image>().color = Color.white;
        }
        // Slot에서 시작됐다면
        else if (bSelectSlot)
        {
            // 터치가 끝날 때 Object가 Slot 일 경우
            if (eventData.pointerCurrentRaycast.gameObject.transform.parent.name == "FormationSlot")
            {
                // 선택한 Slot과 다른 Slot일 경우
                if (int.Parse(eventData.pointerCurrentRaycast.gameObject.name) != iSelectItem)
                {
                    ApplyExchange(int.Parse(eventData.pointerCurrentRaycast.gameObject.name));
                }
                // 선택한 Slot과 동일한 Slot일 경우 반투명 칸만 복구
                else
                {
                    ArmySlotTF.GetChild(iSelectItem).GetComponent<Image>().color = Color.white;
                }

                // 해당 칸의 Outline 효과를 끔
                eventData.pointerCurrentRaycast.gameObject.GetComponent<Outline>().enabled = false;
            }
            // 빈공간에서 끝났다면 해당 칸을 비움
            else
            {
                RemoveChange();
            }
        }
        // 그냥 빈공간을 터치하였으면 아무일도 일어나지 않음
        else
            return;

        // 드래그가 끝난 후 공통적으로 실행되는 부분들
        RefreshFormation();

        bSelectItem = false;
        bSelectSlot = false;
        bIsDraging = false;
        bIsPointerOut = false;
        iSelectItem = -1;

        gBG_Black.SetActive(false);
        bIsBlackActive = false;
        fTime = 0;

        if (gCommanderInfoWindow.activeSelf)
            gCommanderInfoWindow.SetActive(false);
        else if (gSoldierInfoWindow.activeSelf)
            gSoldierInfoWindow.SetActive(false);

        ItemCloneTF.position = new Vector3(-80, -80);
    }

    //
    // 전열 배치를 위한 함수
    //
    /// <summary>
    /// 전열을 갱신시켜주는 함수
    /// </summary>
    public void RefreshArmies()
    {
        // 변경점이 존재한다면 작동해야 함
        if (bIsChange)
        {
            // 병사가 하나라도 존재한다면 대기상태로 변경해준다
            for (int i = 0; i < 4; i++)
            {                
                if (ChangingArmies[i].count != 0)
                {
                    InputManager.instance.gSelectNode.bIsItArmy[iSelectFormationNum] = ArmyState.Stay;
                    break;
                }
                else if(i == 3)
                    InputManager.instance.gSelectNode.bIsItArmy[iSelectFormationNum] = ArmyState.None;
            }

            // 변경하던 전열을 실제 노드의 군대에 적용시킴
            InputManager.instance.gSelectNode.lArmy[iSelectFormationNum] = (Army[])ChangingArmies.Clone();

            // 변경점이 적용 되었음을 알림
            ChangesApply();
            //ChangingArmies = new Army[4];
            //ChangingArmies = (Army[])Constant.aEmptyArmies.Clone();
        }
    }

    /// <summary>
    /// Main으로 띄워진 이미지를 갱신시켜주는 함수
    /// </summary>
    public void RefreshFormation()
    {
        // 대표 지휘관 초상화를 기본으로 변경
        imgRepresent.sprite = sEmptySlot;

        Stats total = new Stats(0, 0, 0);

        // 해당 칸의 정보에 따라 알맞게 정보를 갱신해준다
        for (int i = 0; i < 4; i++)
        {
            if(ChangingArmies[i].bIsCommander)
            {
                imgRepresent.sprite = ChangingArmies[i].commander.sCommanderImg;
                ArmySlotTF.GetChild(i).GetComponent<Image>().sprite = ChangingArmies[i].commander.sCommanderImg;

                InfoTF.GetChild(i).Find("name").GetComponent<Text>().text = ChangingArmies[i].commander.sCommanderName;
                InfoTF.GetChild(i).Find("ad").Find("ad").GetComponent<Text>().text = ChangingArmies[i].stat.AD.ToString();
                InfoTF.GetChild(i).Find("ap").Find("ap").GetComponent<Text>().text = ChangingArmies[i].stat.AP.ToString();
                InfoTF.GetChild(i).Find("hp").Find("hp").GetComponent<Text>().text = ChangingArmies[i].stat.HP.ToString();
            }
            else
            {
                ArmySlotTF.GetChild(i).GetComponent<Image>().sprite = ChangingArmies[i].soldier.iSoldierImg;

                if(ChangingArmies[i].count == 0)
                {
                    ArmySlotTF.GetChild(i).GetComponent<Image>().sprite = sEmptySlot;

                    InfoTF.GetChild(i).Find("name").GetComponent<Text>().text = "Empty";
                    InfoTF.GetChild(i).Find("ad").Find("ad").GetComponent<Text>().text = "-";
                    InfoTF.GetChild(i).Find("ap").Find("ap").GetComponent<Text>().text = "-";
                    InfoTF.GetChild(i).Find("hp").Find("hp").GetComponent<Text>().text = "-";
                }
                else
                {
                    InfoTF.GetChild(i).Find("name").GetComponent<Text>().text = ChangingArmies[i].soldier.sSoldierName;
                    InfoTF.GetChild(i).Find("ad").Find("ad").GetComponent<Text>().text = ChangingArmies[i].stat.AD.ToString();
                    InfoTF.GetChild(i).Find("ap").Find("ap").GetComponent<Text>().text = ChangingArmies[i].stat.AP.ToString();
                    InfoTF.GetChild(i).Find("hp").Find("hp").GetComponent<Text>().text = ChangingArmies[i].stat.HP.ToString();
                }
            }

            total.HP += ChangingArmies[i].stat.HP;
            total.AD += ChangingArmies[i].stat.AD;
            total.AP += ChangingArmies[i].stat.AP;            
        }

        TotalPowerTF.Find("Stat").Find("hp").GetComponent<Text>().text = total.HP.ToString();
        TotalPowerTF.Find("Stat").Find("ad").GetComponent<Text>().text = total.AD.ToString();
        TotalPowerTF.Find("Stat").Find("ap").GetComponent<Text>().text = total.AP.ToString();

        // 실험중 - 바꾸기 이전과 변경사항이 없다면 확정하지 않아도 됨? ★
        if (ChangingArmies == BeforeArmies)
            ChangesApply();
    }

    /// <summary>
    /// Main으로 띄워진 이미지 및 변경 전열을 초기화 시켜주는 함수
    /// </summary>
    public void ResetFormation()
    {
        ChangingArmies = (Army[])Constant.aEmptyArmies.Clone();
        imgRepresent.sprite = sEmptySlot;

        for (int i = 0; i < 4; i++)
        {
            ArmySlotTF.GetChild(i).GetComponent<Image>().sprite = sEmptySlot;

            InfoTF.GetChild(i).Find("name").GetComponent<Text>().text = "Empty";
            InfoTF.GetChild(i).Find("ad").Find("ad").GetComponent<Text>().text = "-";
            InfoTF.GetChild(i).Find("ap").Find("ap").GetComponent<Text>().text = "-";
            InfoTF.GetChild(i).Find("hp").Find("hp").GetComponent<Text>().text = "-";
        }
    }

    /// <summary>
    /// 변경 전열에 변경점을 적용시키는 함수
    /// </summary>
    /// <param name="_num">어느 칸에 넣을 것인가</param>
    /// <param name="_count">몇 명 넣을 것인가</param>
    public void ApplyChange(int _num, int _count)
    { 
        // 현재 보고있는 List에 따라 작동이 달라야함
        switch (sNowList)
        {
            case "지휘관":
                for(int i = 0; i < 4; i++)
                {
                    if (ChangingArmies[i].bIsCommander)
                    {
                        ChangingArmies[i] = Constant.aEmptyArmy;
                        ArmySlotTF.GetChild(i).GetComponent<Image>().sprite = sEmptySlot;
                        break;
                    }
                }
                ChangingArmies[_num] = new Army(DataManager.instance.lComList[iSelectItem]);
                ArmySlotTF.GetChild(_num).GetComponent<Image>().sprite
                    = ItemCloneTF.GetComponent<Image>().sprite;
                imgRepresent.sprite = ItemCloneTF.GetComponent<Image>().sprite;
                break;

            case "장비":
                // 장비를 착용한 칸을 받아와서 해당 장비를 지휘관에게 착용
                // 지휘관의 Stat이 변경됨 (장비 능력치만큼 +)
                // 지휘관의 소유 장비 목록 갱신
                // ListTF에서 해당 장비는 사용중임을 표시
                break;

            case "병사":
                if (ChangingArmies[_num].bIsCommander)
                    imgRepresent.sprite = sEmptySlot;
                ChangingArmies[_num] = new Army(Database.instance.lSoldierList[iSelectItem], _count);
                ArmySlotTF.GetChild(_num).GetComponent<Image>().sprite
                    = ItemCloneTF.GetComponent<Image>().sprite;
                iSelectItem = -1;
                break;
        }
    }

    /// <summary>
    /// 변경 전열에 자리 변경점을 적용시키는 함수
    /// </summary>
    /// <param name="_input">바꾸고자 하는 칸 번호</param>
    public void ApplyExchange(int _input)
    {
        // 임시로 담을 변수
        Army temp = new Army();

        // 칸 교체
        temp = ChangingArmies[_input];
        ChangingArmies[_input] = ChangingArmies[iSelectItem];
        ChangingArmies[iSelectItem] = temp;

        // 시작으로 반투명된 칸을 원상복구 시킴
        ArmySlotTF.GetChild(iSelectItem).GetComponent<Image>().color = Color.white;

        // 이전까지 변경점이 없었다면 생겼음을 알림
        if (!bIsChange)
        {
            ChangesOccur();
        }
    }

    /// <summary>
    /// 변경 전열에 삭제 변경점을 적용시키는 함수
    /// </summary>
    public void RemoveChange()
    {
        // 삭제하려는 칸이 지휘관이라면 대표 이미지도 초기화 시킴
        if (ChangingArmies[iSelectItem].bIsCommander)
            imgRepresent.sprite = sEmptySlot;

        ChangingArmies[iSelectItem] = Constant.aEmptyArmy;
        ArmySlotTF.GetChild(iSelectItem).GetComponent<Image>().color = Color.white;

        // 이전까지 변경점이 없었다면 생겼음을 알림
        if (!bIsChange)
        {
            ChangesOccur();
        }
    }

    /// <summary>
    /// 상세 정보창을 작동시키는 함수
    /// </summary>
    public void ActiveInfoWindow()
    {
        // 아이템을 선택하였을 때
        if (bSelectItem)
        {
            switch (sNowList)
            {
                case "지휘관":
                    gCommanderInfoWindow.SetActive(true);
                    break;

                case "병사":
                    gSoldierInfoWindow.SetActive(true);
                    break;

                case "장비":
                    break;
            }
        }
        // Slot을 선택하였을 때
        else if (bSelectSlot)
        {
            if (ChangingArmies[iSelectItem].bIsCommander)
                gCommanderInfoWindow.SetActive(true);
            else if (!ChangingArmies[iSelectItem].bIsCommander && ChangingArmies[iSelectItem].count > 0)
                gSoldierInfoWindow.SetActive(true);          
            // 둘 다 아닐경우 장비, 추후 추가 필요 ★
        }
    }

    /// <summary>
    /// 드래그시 어두운 효과를 적용시켜주는 함수
    /// </summary>
    public void ActiveBlack()
    {
        gBG_Black.SetActive(true);
        bIsBlackActive = true;
    }

    /// <summary>
    /// 전열 창이 켜질 때 유저가 소유한 전열 개수 만큼 활성화 시켜주는 함수
    /// </summary>
    public void SettingFormationButton()
    {
        if(InputManager.instance.gSelectNode.iType == NodeType.Capital)
        {
            for(int i = 1; i < 4; i++)
            {
                FormationBtnTF.GetChild(i).GetComponent<Button>().interactable = true;
            }
            return;
        }
        else
        {
            for (int i = 1; i < 4; i++)
            {
                FormationBtnTF.GetChild(i).GetComponent<Button>().interactable = false;
            }
            return;
        }
    }

    /// <summary>
    /// 변경사항이 생겼을 때 작동하는 함수
    /// </summary>
    public void ChangesOccur()
    {
        BeforeArmies = (Army[])InputManager.instance.gSelectNode.lArmy[iSelectFormationNum].Clone();
        bIsChange = true;
        if (!btn_Confirm.gameObject.activeSelf)
            btn_Confirm.gameObject.SetActive(true);
        btn_Confirm.interactable = true;
    }

    /// <summary>
    /// 변경사항을 적용시킨 후 작동하는 함수
    /// </summary>
    public void ChangesApply()
    {
        BeforeArmies = new Army[4];
        bIsChange = false;
        btn_Confirm.interactable = false;
    }

    #endregion


    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);


        //
        // 버튼 이미지를 미리 받아옴
        //
        sBtnOff = new Sprite[6];
        sBtnOn = new Sprite[6];

        for(int i = 0; i < 6; i++)
        {
            sBtnOff[i] = Resources.Load("UI/Formation/" + (i + 1).ToString(), typeof(Sprite)) as Sprite;
            sBtnOn[i]  = Resources.Load("UI/Formation/" + (i + 1).ToString() + "_b", typeof(Sprite)) as Sprite;
        }
    }

    private void OnEnable()
    {
        fTime = 0f;
        bIsitMove = false;
        bIsChange = false;
        iSelectFormationNum = 0;
        iSelectNextFormationNum = -1;
        sNowList = SlideTF.Find("Now").GetComponentInChildren<Text>().text;

        iCurrentPage = 1;
        iTotalPage = 0;

        BeforeArmies = new Army[4];
        ChangingArmies = new Army[4];

        RefreshList();
        SettingFormationButton();
        RefreshFormationButton();

        if (InputManager.instance.gSelectNode.CheckIsInArmy(iSelectFormationNum))
        {
            if (InputManager.instance.gSelectNode.bIsItArmy[iSelectFormationNum] == ArmyState.Ready)
                gBlock.SetActive(true);
            else
                gBlock.SetActive(false);

            ChangingArmies = (Army[])InputManager.instance.gSelectNode.lArmy[iSelectFormationNum].Clone();
            RefreshFormation();
        }
        else
        {
            gBlock.SetActive(false);
            ChangingArmies = (Army[])Constant.aEmptyArmies.Clone();
            ResetFormation();
        }
    }

    private void OnDisable()
    {
        btn_Confirm.gameObject.SetActive(false);
        ChangingArmies.Initialize();
        gBlock.SetActive(false);
    }

    private void Update()
    {
        // 슬라이드 돌리기
        if (bIsitMove)
        {
            SlideRotate(iInputDir);
        }

        // 아이템 선택시
        if ((bSelectSlot || bSelectItem) && !bIsBlackActive)
        {
            if (fTime <= .5f)
                fTime += Time.deltaTime;
            else if (fTime > .5f)
            {
                ActiveBlack();
                ActiveInfoWindow();
            }
        }
    }


    #endregion
}

/// <summary>
/// 화면 검은 효과를 어떻게 사용할지 나타내는 열거형 변수
/// </summary>
public enum HowToUseBlack
{
    Army,
    Equip,
    Confirm,


    MAX
}