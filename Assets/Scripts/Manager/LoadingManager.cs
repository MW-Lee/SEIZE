////////////////////////////////////////////////
//
// LoadingManager
//
// 로딩화면에서 작동되는 스크립트
// 19. 06. 24
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    /// <summary>
    /// 다음 Scene을 동기식으로 호출하기 위한 AsyncManager
    /// </summary>
    private AsyncOperation async;

    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Start()
    {
        // DataManager에 저장된 불러올 다음 Scene을 호출
        async = SceneManager.LoadSceneAsync(DataManager.instance.sSceneName);
        // 로딩이 완료되어도 바로 전환하진 않는다
        async.allowSceneActivation = false;
    }

    private void Update()
    {
        // 로딩의 90%가 되면 화면을 전환한다.
        if (async.progress >= 0.9f)
            async.allowSceneActivation = true;
    }

}
