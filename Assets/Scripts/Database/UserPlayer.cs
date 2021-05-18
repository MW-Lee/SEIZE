﻿////////////////////////////////////////////////
//
// UserPlayer
//
// 직접 플레이하는 유저의 정보
// 19. 05. 30
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayer : Player
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //


    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    /// <summary>
    /// Player 스크립트를 상속받고, 이를 재선언하여 새로운 기능을 하도록 지정
    /// </summary>
    public override void TurnUpdate()
    {

        base.TurnUpdate();
    }

    /////////////////////////////////////////////////////////////////////////////////
    // 실행
    //
    private void Awake()
    {
        // 게임이 실행되면 저장된 데이터를 불러와서 적용시켜야함
        // Json파일 연동 후 설정할 것. ★
    }
}