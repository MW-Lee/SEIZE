////////////////////////////////////////////////
//
// ProductionSelectWindow
//
// ProductionSelectWindow에서 작동하는 스크립트
// 19. 05. 23
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionSelectWindow : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 활성화된 선택 제작 창을 활용하기 위한 instance
    /// </summary>
    static public ProductionSelectWindow instance;
    /// <summary>
    /// 선택된 병사의 이미지를 표현할 자리
    /// </summary>
    public Image iSoldierImage;
    /// <summary>
    /// 선택된 병사의 이름을 표현할 자리
    /// </summary>
    public Text txtSoldierName;
    /// <summary>
    /// 병사 숫자를 입력하는 UI들의 Transform
    /// </summary>
    public Transform CountTF;
    /// <summary>
    /// 예상 소모 골드를 나타내는 Text
    /// </summary>
    public Text GoldTXT;
    /// <summary>
    /// 어떤 병사를 만들지 저장
    /// </summary>
    public int iToMake;
    /// <summary>
    /// 만들고 싶은 군사를 미리 저장해놓음
    /// </summary>
    private Soldiers solToMake;

    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// 확인 버튼을 누르면 실행
    /// </summary>
    public void Button_OK()
    {
        int result;
        NewFormation temp = NewFormation.instance;

        result = int.Parse(CountTF.GetChild(0).GetComponent<Text>().text) * 100;
        result += int.Parse(CountTF.GetChild(1).GetComponent<Text>().text) * 10;
        result += int.Parse(CountTF.GetChild(2).GetComponent<Text>().text);

        if (result == 0)
        {
            InputManager.instance.StaticFadePopup("1 이상의 숫자가 필요합니다");
            return;
        }

        temp.iSelectItem = iToMake;
        temp.ChangesOccur();
        temp.ApplyChange(temp.iSelectSlot, result);
        temp.RefreshFormation();

        Button_Cancel();
    }

    /// <summary>
    /// 취소 버튼을 누르면 실행
    /// </summary>
    public void Button_Cancel()
    {
        // 열려있는 창을 모두 지운다.
        //InputManager.instance.gNodeWindow.SetActive(false);
        //if (IsitFormation) InputManager.instance.WActiveWindow = WindowName.FormationWindow;
        //else InputManager.instance.WActiveWindow = WindowName.Empty;
        //InputManager.instance.BG_Black.SetActive(false);
        //NodeWindow.instance.gPWindow.SetActive(false);
        //this.gameObject.SetActive(false);

        InputManager.instance.WActiveWindow = WindowName.FormationWindow;

        this.gameObject.SetActive(false);
    }

    public void Button_Up()
    {
        int temp;
        temp = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInParent<Text>().text);
        temp++;
        if (temp == 10) temp = 0;
        UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInParent<Text>().text = temp.ToString();

        RefreshGold();
    }

    public void Button_Down()
    {
        int temp;
        temp = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInParent<Text>().text);
        temp--;
        if (temp == -1) temp = 9;
        UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInParent<Text>().text = temp.ToString();

        RefreshGold();
    }

    public void RefreshGold()
    {
        int result;
        result = int.Parse(CountTF.GetChild(0).GetComponent<Text>().text) * 100;
        result += int.Parse(CountTF.GetChild(1).GetComponent<Text>().text) * 10;
        result += int.Parse(CountTF.GetChild(2).GetComponent<Text>().text);

        // 병사의 골드 값이 예측되고 해당 값이 적용되어야함
        GoldTXT.text = (result.ToString() + " G");
    }


    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        // 싱글톤 제작
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        iToMake = NewFormation.instance.iSelectItem;
        solToMake = Database.instance.lSoldierList[iToMake];

        iSoldierImage.sprite = solToMake.iSoldierImg;
        txtSoldierName.text = solToMake.sSoldierName;
        InputManager.instance.WActiveWindow = WindowName.ProductSelectWindow;       
    }

    private void OnDisable()
    {
        for(int i = 0; i < 3; i++)
        {
            CountTF.GetChild(i).GetComponent<Text>().text = "0";
        }

        GoldTXT.text = "0 G";
    }
}
