using System.Collections;
using System.Collections.Generic;
using Diplomatrix;
using UnityEngine;

public class GPTInformer : MonoBehaviour
{
    [SerializeField]
    private ArmyScript playerArmy;

    [SerializeField]
    private ArmyScript npcArmy;

    void Start()
    {
    }

    static public string InformMessageArmy(ArmyScript npcArmy, ArmyScript playerArmy){
        string informMessage = "Your army information: " + npcArmy.armyInformation.totalInformation()+
                        "\n\n" + "Enemy army information: " + playerArmy.armyInformation.totalInformation();
        return informMessage;
    }
    static public string InformMessageArmy(ArmyInformation npcArmyInformation, ArmyInformation playerArmyInformation){
        string informMessage = "Your army information: " + npcArmyInformation.totalInformation()+
                        "\n\n" + "Enemy army information: " + playerArmyInformation.totalInformation();
        return informMessage;
    }

    static public string InformMessageCharacteristics(Characteristics characteristics){
        return "Your characteristics information: " + characteristics.ToString();
    }

    
    public string CurrentInformationMessage(){
        return InformMessageArmy(npcArmy, playerArmy);
    }
}
