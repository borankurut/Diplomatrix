using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyScript : MonoBehaviour
{
    public Transform enemyArmy;

    [SerializeField]

    public enum ArmyType{
        playerArmy, NPCArmy
    };
    [SerializeField]
    ArmyType armyType;

    public ArmyType GetArmyType(){return armyType;}
    
}
