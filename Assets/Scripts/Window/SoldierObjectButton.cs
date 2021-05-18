////////////////////////////////////////////////
//
// SoldierObjectButton
//
// 전열 창에서 사용되는 군사 버튼에 들어가는 스크립트
// 각 버튼의 군사 정보를 가진다
// 19. 03. 19
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierObjectButton : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    public Soldiers SoldierInfo;

    public Image ButtonImg;

    public Transform InfoTF;
    public GameObject InfoGO;


    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    public void Button_SoldierObject()
    {
        InfoTF.Find("Name").GetChild(0).GetComponent<Text>().text = SoldierInfo.sSoldierName;

        InfoTF.Find("Stat").GetChild(0).GetComponent<Text>().text = SoldierInfo.sSoldierStat.AD.ToString();
        InfoTF.Find("Stat").GetChild(1).GetComponent<Text>().text = SoldierInfo.sSoldierStat.AP.ToString();
        InfoTF.Find("Stat").GetChild(2).GetComponent<Text>().text = SoldierInfo.sSoldierStat.HP.ToString();

        InfoTF.Find("Img").GetComponent<Image>().sprite = SoldierInfo.iSoldierImg;

        FormationWindow.instance.SelectSoldier = int.Parse(name);
        FormationWindow.instance.IsSelectSoldier = true;
    }

    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        ButtonImg = GetComponent<Image>();
    }

    private void Start()
    {
        // 현재 병사 3마리까지 구현 추후 변경 필요 ★
        if (int.Parse(name) < 3)
        {
            SoldierInfo = Database.instance.lSoldierList[int.Parse(name)];
        }

        ButtonImg.sprite = SoldierInfo.iSoldierImg;
    }

    private void Update()
    {

    }
}
