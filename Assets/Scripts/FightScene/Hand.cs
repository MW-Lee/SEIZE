////////////////////////////////////////////////
//
// Hand
//
// 카드를 쥐고있는 손에서 작동되는 함수
// 19. 06. 20
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 손패를 사용하기 위한 instance
    /// </summary>
    static public Hand instance;
    /// <summary>
    /// 손패에서 터치포인터가 벗어났는지 확인시켜주는 변수
    /// </summary>
    public bool IsPointerOut;

    public Animator aEffect;

    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    // 포인터의 위치로 안밖을 알려줌
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsPointerOut = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsPointerOut = true;
    }

    public void ActiveCard()
    {
        aEffect.SetInteger("SkillNum", 0);
    }

    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        // 시작할 때에는 들어와있지 않음
        IsPointerOut = false;
    }
}
