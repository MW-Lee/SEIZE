////////////////////////////////////////////////
//
// Effect
//
// 카드를 사용할 때 발동되는 스크립트
// 19. 11. 08
// MWLee
////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    #region 변수

    public static Effect instance;

    public Animator aEffect;

    private BattleTurn BT;

    private UnityEngine.UI.Image iShow;

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 함수

    public void ActivateCard(int num)
    {
        aEffect.SetInteger("SkillNum", num);

        iShow.raycastTarget = true;
    }

    #region 스킬 모음
    
    //
    // Aron
    //
    public void Skill_0()
    {
        // 테스트 ★
        // 얼음창 >> 1,2,3번 칸의 적을 삭제
        for (int i = 1; i < 4; i++)
        {
            //BattleTurn.instance.EnemyTF.GetChild(i).GetComponent<BattleTile>().army.count = 0;
            //BattleTurn.instance.EnemyTF.GetChild(i).GetComponent<BattleTile>().IsDie = true;

            BT.EnemyTF.GetChild(i).GetComponent<BattleTile>().army.stat.HP -=
                Database.instance.dCommanderList["Aron"].sCommanderStat.AD * 1.5f;

            if (BT.EnemyTF.GetChild(i).GetComponent<BattleTile>().army.stat.HP < 0)
            {
                BT.EnemyTF.GetChild(i).GetComponent<BattleTile>().SetDie();
            }
        }
    }

    public void Skill_1()
    {
        BT.EnemyTF.GetChild(3).GetComponent<BattleTile>().army.stat.HP -=
            Database.instance.dCommanderList["Aron"].sCommanderStat.AD * 3;

        if (BT.EnemyTF.GetChild(3).GetComponent<BattleTile>().army.stat.HP < 0)
        {
            BT.EnemyTF.GetChild(3).GetComponent<BattleTile>().SetDie();
        }
    }

    public void Skill_2()
    {

    }

    //
    // Lina
    //

    #endregion

    public void EndEffect()
    {
        aEffect.SetInteger("SkillNum", -1);

        iShow.raycastTarget = false;
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region 실행

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        aEffect = GetComponent<Animator>();
        iShow = GetComponent<UnityEngine.UI.Image>();
        BT = BattleTurn.instance;

        iShow.raycastTarget = false;
    }

    #endregion
}
