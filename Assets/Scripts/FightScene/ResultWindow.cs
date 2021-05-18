////////////////////////////////////////////////
//
// ResultWindow
//
// 전투 후 결과창에서 나타나는 창에서 작동하는 스크립트
// 
// 19. 09. 10
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResultWindow : MonoBehaviour, IPointerDownHandler
{
    #region 변수

    

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    public void OnPointerDown(PointerEventData eventData)
    {
        BattleTurn.instance.SetQuest();

        BattleTurn.instance.ShowQuest();
        gameObject.SetActive(false);
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    #endregion

}
