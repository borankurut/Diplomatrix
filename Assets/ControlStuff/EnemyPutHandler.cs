using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPutHandler : PutHandler 
{
    [SerializeField]
    ArmyScript enemyArmy;

    [SerializeField]
    ArmyScript playerArmy;      // maybe look locations of player army to find good positions?

    [SerializeField]
    ChatScript chatScript;
    
    void Awake(){

    }
    void Start()
    {
        StartCoroutine(PutSoldierRoutine());
        StartCoroutine(PutTankRoutine());
        StartCoroutine(PutAirAttackRoutine());
    }
    void Update()
    {
        if(chatScript != null){
            attackPattern.setAggressiveness(chatScript.GetCharacteristics().anger);
        }

    }

    private IEnumerator PutSoldierRoutine(){
        while(true){
            Put(Selection.Soldier, terrainGrids.GetRandomValidPointForEnemy(), enemyArmy);
            yield return new WaitForSeconds(attackPattern.getSoldierAttackPeriod());
        }
    }

    private IEnumerator PutTankRoutine(){
        while(true){
            Put(Selection.Tank, terrainGrids.GetRandomValidPointForEnemy(), enemyArmy);
            yield return new WaitForSeconds(attackPattern.getTankAttackPeriod());
        }
    }

    private IEnumerator PutAirAttackRoutine(){
        while(true){
            while(playerArmy.transform.childCount < 5 && (playerArmy.armyInformation.atHand.tankAmount > 0 || playerArmy.armyInformation.atHand.soldierAmount > 0)){
                yield return new WaitForSeconds(1.0f);      // don't throw because it is unnecessary.
            }

            int enemyIx = Random.Range(0, playerArmy.transform.childCount); // get a random index
            Vector3 randomPlayerMemberPosition = playerArmy.transform.GetChild(enemyIx).position;
            randomPlayerMemberPosition.y = 0;
            Put(Selection.Airstrike, randomPlayerMemberPosition, enemyArmy);

            Debug.Log("PUT AIR ATTACK CALLED.");
            yield return new WaitForSeconds(attackPattern.getAirAttackPeriod());
        }
    }
}
