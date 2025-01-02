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

    [SerializeField]
    private ChatScript chatScript;

    [SerializeField]
    private float informIntervalSeconds = 5f;

    private string informMessageBefore= "";

    void Start()
    {
        StartCoroutine(InformRoutine());
    }

    private IEnumerator InformRoutine()
    {
        while (true)
        {
            InformGPT();
            yield return new WaitForSeconds(informIntervalSeconds);
        }
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

    private void InformGPT()
    {
        string informMessage = InformMessageArmy(npcArmy, playerArmy);

        if(informMessage != informMessageBefore){
            chatScript.giveSecretPrompt("tatata", informMessage);
            // Debug.Log($"GPT IS INFORMED WITH {informMessage}");
        }
        else{
            // Debug.Log("GPT NOT INFORMED BECAUSE INFORMATION IS SAME.");
        }
    }
}
