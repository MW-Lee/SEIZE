////////////////////////////////////////////////
//
// SelectScene
//
// 전투 시작전 선택 장면에서 발동되는 스크립트
// 급조 >> 추후 수정 반드시 필요 ★
// 19. 06. 28
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectScene : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 간단한 확인창 저장
    /// </summary>
    public GameObject gSimpleWindow;
    /// <summary>
    /// 텍스트가 출력될 창
    /// </summary>
    public GameObject gTextWindow;
    /// <summary>
    /// 텍스트에 적을 내용을 저장
    /// </summary>
    public string sNextWord;
    /// <summary>
    /// 선택지 창을 총괄하는 CanvasGroup
    /// </summary>
    public CanvasGroup MainCanvasGroup;


    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// 공격 버튼을 눌렀을 때 작동함
    /// </summary>
    public void OnAttackClick()
    {
        gSimpleWindow.transform.GetChild(0).GetComponent<Text>().text = "진격 하겠습니까?";
        gSimpleWindow.SetActive(true);
        sNextWord = "군사들이 전진합니다";
    }

    public void OnRunClick()
    {
        gSimpleWindow.transform.GetChild(0).GetComponent<Text>().text = "후퇴 하겠습니까?";
        gSimpleWindow.SetActive(true);
        sNextWord = "도망치다 걸렸습니다";
    }

    public void OnSimpleOKClick()
    {
        gSimpleWindow.SetActive(false);
        gTextWindow.transform.GetChild(0).GetComponent<Text>().text = sNextWord;
        gTextWindow.SetActive(true);

        StartCoroutine(StartFadeOut());
    }

    public void OnSimpleCancelClick()
    {
        gSimpleWindow.SetActive(false);
    }

    public IEnumerator StartFadeOut()
    {
        float fTime = 0;

        BattleTurn.instance.bAction = true;

        CanvasGroup temp = MainCanvasGroup;

        while(MainCanvasGroup.alpha > 0f)
        {
            fTime += Time.deltaTime / 1.5f;

            temp.alpha = Mathf.Lerp(1f, 0f, fTime);

            MainCanvasGroup.alpha = temp.alpha;

            yield return null;
        }

        BattleTurn.instance.bAction = false;
        Destroy(gameObject);
        yield return null;
    }

    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        MainCanvasGroup = GetComponent<CanvasGroup>();
    }
}
