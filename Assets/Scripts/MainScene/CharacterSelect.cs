////////////////////////////////////////////////
//
// CharacterSelect
//
// 캐릭터를 선택할 때 작동하는 스크립트
// 급조 >> 추후 수정 반드시 필요 ★
// 19. 07. 01
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelect : MonoBehaviour, IPointerDownHandler
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 첫번째 그림
    /// </summary>
    public GameObject gFirstLook;
    /// <summary>
    /// 두번째 그림
    /// </summary>
    public GameObject gSecondLook;
    /// <summary>
    /// 그림이 눌렸는지 확인
    /// </summary>
    public bool bIsClick = false;


    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    public void OnPointerDown(PointerEventData eventData)
    {
        // 토글 형식으로 그림을 바꿔줌
        bIsClick ^= true;

        if (bIsClick)
        {
            gFirstLook.SetActive(false);
            gSecondLook.SetActive(true);
        }
        else
        {
            gFirstLook.SetActive(true);
            gSecondLook.SetActive(false);
        }
    }



    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
}
