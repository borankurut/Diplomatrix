using System.Collections;
using System.Collections.Generic;
using Diplomatrix;
using Unity.VisualScripting;
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

    public ArmyAttributes initialArmyInformation;
    public ArmyAttributes atHandArmyInformation;
    public ArmyAttributes atBattlefieldArmyInformation;

    void Awake(){
        atHandArmyInformation = initialArmyInformation;
    }

    public ArmyAttributes currentArmyInformation(){
        return new ArmyAttributes(atHandArmyInformation.soldierAmount + atBattlefieldArmyInformation.soldierAmount, 
            atHandArmyInformation.tankAmount + atBattlefieldArmyInformation.tankAmount);
    }

    public string totalInformation(){
        return "Initial Army: " + initialArmyInformation.ToString() + "\n" +
                "Current Army: " + currentArmyInformation().ToString(); 
    }
}
