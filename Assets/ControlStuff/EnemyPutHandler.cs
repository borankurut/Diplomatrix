using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPutHandler : PutHandler 
{
    [SerializeField]
    ArmyScript enemyArmy;

    [SerializeField]
    ArmyScript playerArmy;      // maybe look locations of player army to find good positions?

    public Diplomatrix.AttackPattern attackPattern = new Diplomatrix.AttackPattern(1, 10, 10);
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
        
    }

    private IEnumerator PutSoldierRoutine(){
        while(true){
            Put(Selection.Soldier, terrainGrids.GetRandomValidPoint(), enemyArmy);
            Debug.Log("attackperiod soldier: " + attackPattern.getSoldierAttackPeriod());
            yield return new WaitForSeconds(attackPattern.getSoldierAttackPeriod());
        }
    }

    private IEnumerator PutTankRoutine(){
        while(true){
            Put(Selection.Tank, terrainGrids.GetRandomValidPoint(), enemyArmy);
            yield return new WaitForSeconds(attackPattern.getTankAttackPeriod());
        }
    }

    private IEnumerator PutAirAttackRoutine(){
        while(true){
            Debug.Log("PUT AIR ATTACK CALLED.");
            yield return new WaitForSeconds(attackPattern.getAirAttackPeriod());
        }
    }
}
