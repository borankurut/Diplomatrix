using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelDesignWindowScript : MonoBehaviour
{
    public GameSettings gameSettings;

    [SerializeField] 
    Slider aggressivenessSlider;

    [SerializeField]
    TextMeshProUGUI aggressivenessText;

    [SerializeField]
    TMP_InputField playerSoldiersInput;

    [SerializeField]
    TMP_InputField playerTanksInput;

    [SerializeField]
    TMP_InputField playerAirStrikesInput;

    [SerializeField]
    TMP_InputField enemySoldiersInput;

    [SerializeField]
    TMP_InputField enemyTanksInput;

    [SerializeField]
    TMP_InputField enemyAirStrikesInput;

    [SerializeField]
    Button startButton;

    void Start(){
        aggressivenessSlider.onValueChanged.AddListener(UpdateAggressivenesText);
        startButton.onClick.AddListener(OnStartGame);
    }

    void UpdateAggressivenesText(float value){
        aggressivenessText.text = value.ToString();
    }
    void OnStartGame()
    {
        gameSettings.playerSoldiers = int.Parse(playerSoldiersInput.text);
        gameSettings.playerTanks = int.Parse(playerTanksInput.text);
        gameSettings.playerAirStrikes = int.Parse(playerAirStrikesInput.text);

        gameSettings.enemySoldiers = int.Parse(enemySoldiersInput.text);
        gameSettings.enemyTanks = int.Parse(enemyTanksInput.text);
        gameSettings.enemyAirStrikes = int.Parse(enemyAirStrikesInput.text);
        gameSettings.enemyCharacteristics.anger = (int) aggressivenessSlider.value;

        Debug.Log("Game settings Send:");
        Debug.Log(gameSettings);

        // Load the game scene TODO
        UnityEngine.SceneManagement.SceneManager.LoadScene("PutTestMap");
    }
}
