////////////////////////////////////////////////
//
// SmallLogWindow
//
// 추가로그창에서 작동하기 위한 스크립트
// 
// 19. 10. 04
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallLogWindow : MonoBehaviour
{
    #region 변수
    /// <summary>
    /// 작은 로그창에 나타나는 이미지
    /// </summary>
    public Image iImg;
    /// <summary>
    /// 작은 로그창에 나타나는 행동 유형
    /// </summary>
    public Text tType;
    /// <summary>
    /// 작은 로그창에 나타나는 내용
    /// </summary>
    public Text tContent;
    /// <summary>
    /// 현재 작은 로그창이 나타나 있는가?
    /// </summary>
    public bool bIsShow = false;
    /// <summary>
    /// 현재 작은 로그창의 Transform을 미리 파싱해놓음
    /// </summary>
    private Transform TF;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수
    /// <summary>
    /// 안의 내용을 갱신
    /// </summary>
    public void RefreshLog(Log _input)
    { 
        iImg.sprite = _input.sImg;
        tContent.text = _input.sContent;
    }

    /// <summary>
    /// 닫는 버튼이 눌리면 닫는 애니메이션 실행
    /// </summary>
    public void Button_Close()
    {
        StartCoroutine(WindowMove());
    }

    /// <summary>
    /// 작은 로그창이 움직일 때 작동하는 Coroutine
    /// </summary>
    /// <returns></returns>
    public IEnumerator WindowMove()
    {
        float fTime = 0;

        // 작은 로그창이 활성화 되어 있을 때 다시 넣는 역할
        if (bIsShow)
        {
            TF.position = new Vector3(
                Mathf.Lerp(TF.position.x, TF.parent.position.x, fTime),
                TF.position.y,
                TF.position.z
                );
            yield return null;

            while(TF.position.x > TF.parent.position.x)
            {
                fTime += Time.deltaTime / .5f;

                TF.position = new Vector3(
                Mathf.Lerp(TF.position.x, TF.parent.position.x, fTime),
                TF.position.y,
                TF.position.z
                );
                yield return null;

                if(TF.position.x <= (TF.parent.position.x + 10))
                {
                    TF.position = new Vector3(
                        TF.parent.position.x,
                        TF.position.y,
                        TF.position.z
                        );
                    bIsShow = false;
                    break;
                }
            }
        }
        // 작은 로그창이 비활성화 되어있어 밖으로 꺼내는 역할
        else
        {
            TF.position = new Vector3(
                    Mathf.Lerp(TF.position.x, 650, fTime),
                    TF.position.y,
                    TF.position.z
                    );
            yield return null;

            while (TF.position.x < 650)
            {
                fTime += Time.deltaTime / .5f;

                TF.position = new Vector3(
                    Mathf.Lerp(TF.position.x, 650, fTime),
                    TF.position.y,
                    TF.position.z
                    );
                yield return null;

                if (TF.position.x >= 640)
                {
                    TF.position = new Vector3(
                        650,
                        TF.position.y,
                        TF.position.z
                        );

                    bIsShow = true;
                    break;
                }
            }
        }

        // 작은 로그창이 비활성화 되면 LogWindow에 알려준다
        if (!bIsShow)
        {
            if (this.gameObject.name == "SmallLog1")
                LogWindow.bSmall1 = false;
            else
                LogWindow.bSmall2 = false;

            this.gameObject.SetActive(false);
        }
        yield return null;
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Awake()
    {
        TF = this.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        RefreshLog(LogWindow.instance.lSelectLog);
        StartCoroutine(WindowMove());
    }

    private void Update()
    {

    }

    private void OnDisable()
    {
        
    }

    #endregion
}
