using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPutHandler : PutHandler 
{
    [SerializeField]
    ArmyScript enemyArmy;

    [SerializeField]
    ArmyScript playerArmy;      // maybe look locations of player army to find good positions?
    Diplomatrix.AttackPattern attackPattern;
    void Awake(){

    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
