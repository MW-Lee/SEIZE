////////////////////////////////////////////////
//
// NewStart
//
// 메인화면의 새로시작 버튼에 작동하는 스크립트
// 급조 >> 추후 수정 반드시 필요 ★
// 19. 07. 01
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewStart : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //



    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    public void OnNewStartclick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterSelect");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnCharacterSelect()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapPractice");
    }


    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {

    }
}
