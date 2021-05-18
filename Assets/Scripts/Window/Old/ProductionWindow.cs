////////////////////////////////////////////////
//
// ProductionWindow
//
// ProductionWindow에서 작동하는 스크립트
// 19. 05. 20
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionWindow : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 선택 제작창
    /// </summary>
    public GameObject gPSWindow;
    /// <summary>
    /// DataBase의 Soldier 정보를 받아서 임시저장
    /// </summary>
    public List<Soldiers> temp = null;

    public List<Army> lNodeArmy = new List<Army>();

    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// 만들고 싶은 병사를 눌렀을 때
    /// </summary>
    public void Button_Production_Select()
    {
        // Database를 참고하여
        // 만들고자 하는 병사가 무엇인지 찾는다
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].sSoldierName == this.name)
            {
                gPSWindow.SetActive(true);
                //ProductionSelectWindow.instance.sToMake = temp[i];
                ProductionSelectWindow.instance.iSoldierImage.sprite = temp[i].iSoldierImg;
                return;
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        temp = Database.instance.lSoldierList;
        
    }

    private void Start()
    {

    }
}
