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

    [SerializeField] 
    TMP_Text textValues; 
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

    void Update(){
        textValues.text = ValuesAtHand();
    }

    private string ValuesAtHand(){
        string toReturn = "";
        if(armyType == ArmyType.NPCArmy){
            toReturn += "Enemy";
        }
        else if(armyType == ArmyType.playerArmy){
            toReturn += "Player";
        }
        else{
            toReturn += "Null";
        }
        toReturn += "\n  Soldiers:" + armyInformation.atHand.soldierAmount.ToString();
        toReturn += "\n  Tanks:" + armyInformation.atHand.tankAmount.ToString();
        toReturn += "\n  Airstrikes:" + armyInformation.atHand.airStrikeAmount.ToString();
        return toReturn;
    }


}
