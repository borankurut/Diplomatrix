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
    public ArmyAttributes currentArmyInformation;

    void Awake(){
        currentArmyInformation = initialArmyInformation;
    }

    public string totalInformation(){
        return "Initial Army: " + initialArmyInformation.ToString() + "\n" +
                "Current Army: " + currentArmyInformation.ToString(); 
    }
    
}
