using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActorController : MonoBehaviour
{

    public GameManager gameManager;
    public HUDTextManager hudTextManager;
    public enum ACTORTYPE : int
    {
        PLAYER,
        MONSTER
    }

    public int baseHp;
    public int baseDefense;
    public int baseLuck;
    public int baseAttack;
    
    public int currentHp;
    public int currentDefense;
    public int currentLuck;
    public int currentAttack;
   
    public int maxHp;
    public int maxDefense;
    public int maxLuck;
    public int maxAttack;

    public GameObject Target;

    private float nextAttackTime = 0f;
    public float attackInterval = 2f;

    public ACTORTYPE actortype = ACTORTYPE.PLAYER;
    
    Vector3 targetPos;
    Vector3 OwnPos;
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Target != null && Time.time > nextAttackTime)
        {
            ActorAttack();
            nextAttackTime = Time.time + attackInterval;
        }
    }

    public void DieCheck()
    {
        if(currentHp <= 0)
        {
            ActorDie();
        }
    }

    public int ActorDamaged(int Number)
    {
        currentHp -= Number;
        hudTextManager.UpdateHUDTextSet(Number.ToString(), this.gameObject, Vector3.zero);
        DieCheck();

        return currentHp;
       
    }

    public void ActorAttack()
    {
        StartCoroutine(CoAttackMotion());
    }

    IEnumerator CoAttackMotion()
    {
        targetPos = Target.transform.position;
        OwnPos = transform.position;
        
        Sequence sequence;
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(targetPos, 0.1f));
        sequence.Append(transform.DOMove(OwnPos, 0.1f));
        yield return new WaitForSeconds(0.2f);
        AttackMotionEnd();
    }

    public virtual void AttackMotionEnd()
    {

    }

    public virtual void ActorDie()
    {
       
    }


}
