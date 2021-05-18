////////////////////////////////////////////////
//
// QuestWindow
//
// 퀘스트 창에서 작동하는 스크립트
// 19. 09. 06
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour, UnityEngine.EventSystems.IPointerDownHandler
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    public static QuestWindow instance;

    public List<Quest> lQuestList;

    public int iCurrentQuestnum;

    public Text TitleTXT;
    public Text InfoTXT;

    public GameObject LU;
    public GameObject LD;
    public GameObject RU;
    public GameObject RD;

    public Image iCenterImg;

    public AudioSource asBGM;
    public AudioClip acQuest;

    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    public void RefreshQuest(Quest _quest)
    {
        TitleTXT.text = _quest.title;

        if (_quest.lu == "")
            LU.GetComponent<Button>().interactable = false;
        else
            LU.GetComponent<Button>().interactable = true;
        LU.GetComponentInChildren<Text>().text = _quest.lu;

        if (_quest.ld == "")
            LD.GetComponent<Button>().interactable = false;
        else
            LD.GetComponent<Button>().interactable = true;
        LD.GetComponentInChildren<Text>().text = _quest.ld;

        if (_quest.ru == "")
            RU.GetComponent<Button>().interactable = false;
        else
            RU.GetComponent<Button>().interactable = true;
        RU.GetComponentInChildren<Text>().text = _quest.ru;

        if (_quest.rd == "")
            RD.GetComponent<Button>().interactable = false;
        else
            RD.GetComponent<Button>().interactable = true;
        RD.GetComponentInChildren<Text>().text = _quest.rd;

        if (_quest.isitend)
            InfoTXT.text = "이 곳을 선택하여 나가기";

    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if(lQuestList[iCurrentQuestnum].isitend)
        {
            //gameObject.SetActive(false);
            BattleTurn.instance.BackToMap();
        }
    }

    public void Button_Select()
    {
        Quest _quest = lQuestList[iCurrentQuestnum];

        // 현재 선택된 오브젝트를 가져오는 법 ★
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "LU":
                iCurrentQuestnum = _quest.nextnum[0];
                break;

            case "LD":
                iCurrentQuestnum = _quest.nextnum[1];
                break;

            case "RU":
                iCurrentQuestnum = _quest.nextnum[2];
                break;

            case "RD":
                iCurrentQuestnum = _quest.nextnum[3];
                break;
        }

        // 선택지를 저장하고 다음 퀘스트가 진행되어야함
        RefreshQuest(lQuestList[iCurrentQuestnum]);
        return;
    }



    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        iCurrentQuestnum = 0;
        InfoTXT.text = "선택을 하십시오";
        RefreshQuest(lQuestList[iCurrentQuestnum]);

        asBGM.clip = acQuest;
        asBGM.Play();

        iCenterImg.sprite =
            DataManager.instance.lUserArmy[Constant.FindCommander(DataManager.instance.lUserArmy)].commander.sCommanderImg;
    }
}
