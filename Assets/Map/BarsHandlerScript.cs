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
    void Update()
    {
        soldierSlider.value = Mathf.Clamp01(playerPutHandler.SoldierReadyRatio());
        tankSlider.value = Mathf.Clamp01(playerPutHandler.TankReadyRatio());
        airstrikeSlider.value = Mathf.Clamp01(playerPutHandler.AirstrikeReadyRatio());

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
}
