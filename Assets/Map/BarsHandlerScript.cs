using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarsHandlerScript : MonoBehaviour
{
    [SerializeField]
    PlayerPutHandler playerPutHandler;

    [SerializeField]
    Slider soldierSlider;

    [SerializeField]
    Slider tankSlider;

    [SerializeField]
    Slider airstrikeSlider;

    [SerializeField]
    TMP_Text soldierStackCount;

    [SerializeField]
    TMP_Text tankStackCount;

    [SerializeField]
    TMP_Text airstrikeStackCount;

    [SerializeField]
    ArmyScript playerArmy;

    [SerializeField]
    ArmyScript enemyArmy;

    [SerializeField] 
    TMP_Text textValuesPlayer; 

    [SerializeField] 
    TMP_Text textValuesEnemy; 



    void Update()
    {
        textValuesPlayer.text = ValuesAtHand(playerArmy);
        textValuesEnemy.text = ValuesAtHand(enemyArmy);

        // bars were counting when there is no units preparing, this is bad bad so temp fix.
        soldierSlider.value = playerArmy.armyInformation.atHand.soldierAmount == playerPutHandler.getSoldierStack() ? 0 : Mathf.Clamp01(playerPutHandler.SoldierReadyRatio());
        tankSlider.value = playerArmy.armyInformation.atHand.tankAmount == playerPutHandler.getTankStack() ? 0 : Mathf.Clamp01(playerPutHandler.TankReadyRatio());
        airstrikeSlider.value = playerArmy.armyInformation.atHand.airStrikeAmount == playerPutHandler.getAirstrikeStack() ? 0 : Mathf.Clamp01(playerPutHandler.AirstrikeReadyRatio());

        if(playerPutHandler.getSoldierStack() <= 0){
            soldierStackCount.text = "";
        }
        else{
            soldierStackCount.text = "x" + playerPutHandler.getSoldierStack().ToString();
        }

        if(playerPutHandler.getTankStack() <= 0){
            tankStackCount.text = "";
        }
        else{
            tankStackCount.text = "x" + playerPutHandler.getTankStack().ToString();
        }

        if(playerPutHandler.getAirstrikeStack() <= 0){
            airstrikeStackCount.text = "";
        }
        else{
            airstrikeStackCount.text = "x" + playerPutHandler.getAirstrikeStack().ToString();
        }
    }
    private string ValuesAtHand(ArmyScript army){
        string toReturn = "";
        if(army.GetArmyType() == ArmyScript.ArmyType.NPCArmy){
            toReturn += "Enemy";
        }
        else if(army.GetArmyType() == ArmyScript.ArmyType.playerArmy){
            toReturn += "Player";
        }
        else{
            toReturn += "Null";
        }
        toReturn += "\n  Soldiers:" + army.armyInformation.atHand.soldierAmount.ToString();
        toReturn += "\n  Tanks:" + army.armyInformation.atHand.tankAmount.ToString();
        toReturn += "\n  Airstrikes:" + army.armyInformation.atHand.airStrikeAmount.ToString();
        return toReturn;
    }
}
