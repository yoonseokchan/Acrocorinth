using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ActorController
{
    // Start is called before the first frame update
    public override void Start()
    {
        actortype = ACTORTYPE.PLAYER;

        //¿”Ω√∑Œ Ω∫≈› ¡‹

        currentHp = 50;
        currentAttack = 10;
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void AttackMotionEnd()
    {
        Target.GetComponent<MonsterController>().ActorDamaged(currentAttack);
    }

    public override void ActorDie()
    {
        gameManager.statetype = GameManager.STATETYPE.BATTLE_END_PALYER_LOSE;
    }
}
