using UnityEngine;
using System.Text;
using Diplomatrix;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/GameSettings")]
public class GameSettings : ScriptableObject
{
    public int playerSoldiers;
    public int playerTanks;
    public int playerAirStrikes;
    public int enemySoldiers;
    public int enemyTanks;
    public int enemyAirStrikes;
    public Characteristics enemyCharacteristics;
    public string language;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Game Settings:");

        sb.AppendLine($"Player Soldiers: {playerSoldiers}");
        sb.AppendLine($"Player Tanks: {playerTanks}");
        sb.AppendLine($"Player Air Strikes: {playerAirStrikes}");

        sb.AppendLine($"Enemy Soldiers: {enemySoldiers}");
        sb.AppendLine($"Enemy Tanks: {enemyTanks}");
        sb.AppendLine($"Enemy Air Strikes: {enemyAirStrikes}");
        sb.AppendLine($"Enemy Aggressiveness: {enemyCharacteristics}");

        sb.AppendLine($"Enemy Talk Language: {language}");

        return sb.ToString();
    }
}
