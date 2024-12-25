using System.Collections;
using System.Collections.Generic;
using Diplomatrix;
using UnityEngine;

public class GPTInformer : MonoBehaviour
{
    [SerializeField]
    private ArmyScript playerArmy;
    private string playerTotalBefore = "";

    [SerializeField]
    private ArmyScript npcArmy;
    private string npcTotalBefore = "";

    [SerializeField]
    private ChatScript chatScript;

    [SerializeField]
    private float informIntervalSeconds = 5f;

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

    public void InformGPT()
    {
        string npcTotal = npcArmy.totalInformation();
        string playerTotal = playerArmy.totalInformation();
        if(npcTotal == npcTotalBefore && playerTotal == playerTotalBefore){
            Debug.Log("GPT IS NOT INFORMED BECAUSE BATTLE STATE IS STABLE");
            return;
        }
        string informMessage = "Your army information: " + npcTotal +
                        "\n\n" + "Enemy army information: " + playerTotal;

        chatScript.giveSecretPrompt("tatata", informMessage);

        playerTotalBefore = playerTotal;
        npcTotalBefore = npcTotal;

        Debug.Log($"GPT IS INFORMED WITH {informMessage}");

    }
}
