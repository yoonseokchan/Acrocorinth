using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum STATETYPE : int
    {
        MAP_INIT,
        PLAYER_MOVE,
        MONSTER_GEN,
        BATTLE_INIT,
        BATTLE,
        BATTLE_END_PALYER_WIN,
        BATTLE_END_PALYER_LOSE,
        OPEN_CHEST,
        MAP_END
    }

    public STATETYPE statetype = STATETYPE.MAP_INIT;

    public GameObject playerPrefabs;
    public GameObject monsterPrefabs;

    public Transform playerPivot;
    public Transform monsterPivot;

    private float MonsterGenInterval = 6.0f;
    private float MonsterGenTime = 6.0f;

    public GameObject currentPlayer;
    public GameObject currentMonster;

    public Text uiPlayerHp;
    public Text uiMonsterHp;
    public Text uiCurrentState;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        UIUpdate();

        if (statetype == STATETYPE.PLAYER_MOVE)
        {
            MapMove();

            MonsterGenTime -= Time.deltaTime;

            if(MonsterGenTime <= 0)
            {
                statetype = STATETYPE.MONSTER_GEN;
                MonsterGenTime = MonsterGenInterval;
                MonsterGen();
            }
        }
        else if(statetype == STATETYPE.BATTLE_END_PALYER_LOSE)
        {
            currentPlayer.GetComponent<PlayerController>().Target = null;
            currentMonster.GetComponent<MonsterController>().Target = null;
        }
        else if (statetype == STATETYPE.BATTLE_END_PALYER_WIN)
        {
            currentPlayer.GetComponent<PlayerController>().Target = null;
            currentPlayer.transform.DOMove(playerPivot.transform.position, 0.1f);
            currentMonster.GetComponent<MonsterController>().Target = null;
            Destroy(currentMonster);
            statetype = STATETYPE.OPEN_CHEST; //나중에 아이템 획득 작업 
            statetype = STATETYPE.PLAYER_MOVE; //우선은 다음 몬스터 나오게
        }

    }

    public void MapMove()
    {



    }

    public void Init()
    {
        if(statetype == STATETYPE.MAP_INIT)
        {
            GameObject temp = (GameObject)Instantiate(playerPrefabs);
            temp.transform.position = playerPivot.transform.position;
            currentPlayer = temp;
            currentPlayer.GetComponent<PlayerController>().gameManager = this;
            currentPlayer.GetComponent<PlayerController>().hudTextManager = GetComponent<HUDTextManager>();
            statetype = STATETYPE.PLAYER_MOVE;
        }
    }

    public void MonsterGen()
    {
        statetype = STATETYPE.BATTLE_INIT;
        GameObject temp = (GameObject)Instantiate(monsterPrefabs);
        temp.transform.position = monsterPivot.transform.position + new Vector3(10.0f, 0.0f,0.0f);
        currentMonster = temp;
        currentMonster.GetComponent<MonsterController>().gameManager = this;
        currentMonster.GetComponent<MonsterController>().hudTextManager = GetComponent<HUDTextManager>();
        temp.transform.DOMove(monsterPivot.transform.position, 2.0f);
        StartCoroutine(CoMonsterGen());
    }

    IEnumerator CoMonsterGen()
    {
        yield return new WaitForSeconds(2.0f);
        currentPlayer.GetComponent<PlayerController>().Target = currentMonster;        
        currentMonster.GetComponent<MonsterController>().Target = currentPlayer;
        
        statetype = STATETYPE.BATTLE;
    }

    public void UIUpdate()
    {
        if (currentPlayer != null)
            uiPlayerHp.text = "Player HP : " + currentPlayer.GetComponent<PlayerController>().currentHp.ToString();

        if (currentMonster != null)
        {
            uiMonsterHp.text = "Monster HP : " + currentMonster.GetComponent<MonsterController>().currentHp.ToString();
        }
        else
        {
            uiMonsterHp.text = "Monster Gen Time : " + MonsterGenTime.ToString("00:00");
        }

        string stateString = "";
        if (statetype == STATETYPE.MAP_INIT) stateString = "MAP_INIT";
        if (statetype == STATETYPE.PLAYER_MOVE) stateString = "PLAYER_MOVE";
        if (statetype == STATETYPE.MONSTER_GEN) stateString = "MONSTER_GEN";
        if (statetype == STATETYPE.BATTLE_INIT) stateString = "BATTLE_INIT";
        if (statetype == STATETYPE.BATTLE) stateString = "BATTLE";
        if (statetype == STATETYPE.BATTLE_END_PALYER_WIN) stateString = "BATTLE_END_PALYER_WIN";
        if (statetype == STATETYPE.BATTLE_END_PALYER_LOSE) stateString = "BATTLE_END_PALYER_LOSE";
        if (statetype == STATETYPE.OPEN_CHEST) stateString = "OPEN_CHEST";        
        if (statetype == STATETYPE.MAP_END) stateString = "MAP_END";

        uiCurrentState.text = stateString;
    }
}
