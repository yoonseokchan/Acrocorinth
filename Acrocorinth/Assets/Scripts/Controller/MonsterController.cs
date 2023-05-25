using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : ActorController
{
    // Start is called before the first frame update
    public override void Start()
    {
        actortype = ACTORTYPE.MONSTER;

        currentHp = 25;
        currentAttack = 2;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void AttackMotionEnd()
    {
        Target.GetComponent<PlayerController>().ActorDamaged(currentAttack);
    }

    public override void ActorDie()
    {
        gameManager.statetype = GameManager.STATETYPE.BATTLE_END_PALYER_WIN;
    }

}
