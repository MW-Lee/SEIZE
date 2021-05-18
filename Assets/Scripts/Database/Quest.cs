////////////////////////////////////////////////
//
// Quest
//
// 퀘스트 진행을 위한 정보 스크립트
// 
// 19. 09. 09
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    /////////////////////////////////////////////////////////////////////////////////
    // 변수 선언
    //
    public string title;

    public string lu;
    public string ld;
    public string ru;
    public string rd;

    public int[] nextnum = new int[4];

    public bool isitend;

    /////////////////////////////////////////////////////////////////////////////////
    // 함수 선언
    //
    public Quest(Dictionary<string, object> inputQuest)
    {
        title = inputQuest["title"].ToString();

        lu = inputQuest["lu"].ToString();
        ld = inputQuest["ld"].ToString();
        ru = inputQuest["ru"].ToString();
        rd = inputQuest["rd"].ToString();

        if (int.TryParse(inputQuest["lunext"].ToString(), out int final))
            nextnum[0] = final;
        else
            nextnum[0] = 0;

        if (int.TryParse(inputQuest["ldnext"].ToString(), out final))
            nextnum[1] = final;
        else
            nextnum[1] = 0;

        if (int.TryParse(inputQuest["runext"].ToString(), out final))
            nextnum[2] = final;
        else
            nextnum[2] = 0;

        if (int.TryParse(inputQuest["rdnext"].ToString(), out final))
            nextnum[3] = final;
        else
            nextnum[3] = 0;

        isitend = bool.Parse(inputQuest["isitend"].ToString());
    }
}
