using System.Collections;
using System.Collections.Generic;
using Diplomatrix;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ArmyScript : MonoBehaviour
{
    public GameSettings gameSettings;
    public Transform enemyArmy;

    [SerializeField]

    public enum ArmyType{
        playerArmy, NPCArmy
    };
    [SerializeField]
    ArmyType armyType;

    public ArmyInformation armyInformation;

    public ArmyType GetArmyType(){return armyType;}

    void Awake(){
        armyInformation = new ArmyInformation();
        if(armyType == ArmyType.playerArmy){
            armyInformation.initial.soldierAmount = gameSettings.playerSoldiers;
            armyInformation.initial.tankAmount = gameSettings.playerTanks;
            armyInformation.initial.airStrikeAmount = gameSettings.playerAirStrikes;

        }

        else if(armyType == ArmyType.NPCArmy){
            armyInformation.initial.soldierAmount = gameSettings.enemySoldiers;
            armyInformation.initial.tankAmount = gameSettings.enemyTanks;
            armyInformation.initial.airStrikeAmount = gameSettings.enemyAirStrikes;
        }

        armyInformation.atHand = armyInformation.initial;
    }

    public bool IsDoomed(){
        return armyInformation.currentArmyInformation().airStrikeAmount <= 0 &&
                armyInformation.currentArmyInformation().tankAmount <= 0 &&
                armyInformation.currentArmyInformation().soldierAmount <=0;
    }

}
