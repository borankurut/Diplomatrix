using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerScript : MonoBehaviour
{
    [SerializeField]
    ChatScript chatScript;

    [SerializeField]
    ArmyScript playerArmy;

    [SerializeField]
    ArmyScript npcArmy;

    [SerializeField]
    GameObject inGameStuff;

    [SerializeField]
    GameObject outGameStuff;

    [SerializeField]
    TMP_Text winLostText;
    
    void Start()
    {
        StartCoroutine(checkGameEndRoutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            StopCoroutine(checkGameEndRoutine());
            LoadLevelDesign();
            // user pressed escape, close.
        }
    }

    IEnumerator checkGameEndRoutine()
    {
        while (!gameOver())
        {
            yield return new WaitForSeconds(3.0f);
        }

        if (PlayerWon())
        {
            winLostText.text = "YOU WIN";
        }
        else
        {
            winLostText.text = "YOU LOST";
        }

        inGameStuff.SetActive(false);
        outGameStuff.SetActive(true);

        yield return new WaitForSeconds(5.0f);
        LoadLevelDesign();
    }

    bool PlayerWon(){
        return chatScript.GetCharacteristics().surrenderLikelihood >= 8 || npcArmy.IsDoomed();
    }

    bool NpcWon(){
        return playerArmy.IsDoomed();
    }

    bool gameOver(){
        return PlayerWon() || NpcWon();
    }

    void LoadLevelDesign(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelDesignWindow");
    }
}
