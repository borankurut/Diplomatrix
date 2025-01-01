using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutHandler : MonoBehaviour
{
    [SerializeField]
    protected TerrainGrids terrainGrids;

    protected enum Selection {Unknown, Soldier, Tank};

    [SerializeField]
    GameObject soldierPrefab;

    [SerializeField]
    GameObject tankPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected string SelectionToString(Selection selection){
        if(selection == Selection.Unknown){
            return "Unknown";
        }
        else if(selection == Selection.Soldier){
            
            return "Soldier";
        }
        else if(selection == Selection.Tank){
            return "Tank";
        }

        return "NULL";
    }

    protected void Put(Selection selection, Vector3 position, ArmyScript army){
        if(army.GetArmyType() == ArmyScript.ArmyType.playerArmy && 
            !terrainGrids.IsValidInsidePlayerSide(position))
        {
            Debug.Log("Player is trying to put somewhere outside the map.");
            return;
        }

        if(army.GetArmyType() == ArmyScript.ArmyType.NPCArmy && 
            !terrainGrids.IsValidInsideEnemySide(position))
        {
            Debug.Log("Enemy is trying to put somewhere outside the map.");
            return;
        }

        if(selection == Selection.Soldier){

            if(army.atHandArmyInformation.soldierAmount <= 0)
                return;

            GameObject addedSoldier = Instantiate(soldierPrefab);
            addedSoldier.transform.position = position;
            addedSoldier.transform.SetParent(army.transform, true);
            army.atHandArmyInformation.soldierAmount -= 1;
            army.atBattlefieldArmyInformation.soldierAmount += 1;
        }

        else if(selection == Selection.Tank){

            if(army.atHandArmyInformation.tankAmount <= 0)
                return;

            GameObject addedTank = Instantiate(tankPrefab);
            addedTank.transform.position = position;
            addedTank.transform.SetParent(army.transform, true);
            army.atHandArmyInformation.tankAmount-= 1;
            army.atBattlefieldArmyInformation.tankAmount+= 1;
        }

        Debug.Log("Trying to put " + SelectionToString(selection) + " here: " + position);
    }

}
