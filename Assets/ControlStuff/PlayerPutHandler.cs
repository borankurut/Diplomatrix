using System;
using System.Collections;
using System.Collections.Generic;
using Diplomatrix;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPutHandler : MonoBehaviour
{
    [SerializeField]
    private TerrainGrids terrainGrids;

    [SerializeField]
    enum Selection {Unknown, Soldier, Tank};

    [SerializeField]
    ArmyScript playerArmy;
    Selection selection;

    [SerializeField]
    GameObject soldierPrefab;

    [SerializeField]
    GameObject tankPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1))
        {
            HandleClick();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            Debug.Log("ONE IS PRESSED, Soldier selected");
            selection = Selection.Soldier;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)){
            Debug.Log("TWO IS PRESSED, Tank is selected.");
            selection = Selection.Tank;
        }
    }

    void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool rayHit = Physics.Raycast(ray, out RaycastHit hit);
        bool hitToTerrain = false;
        if(rayHit)
            hitToTerrain = hit.collider.gameObject == terrainGrids.gameObject;     

        if(!hitToTerrain){
            return;
        }

        if(!terrainGrids.IsValidCoordinate(hit.point)){
            Debug.Log("Trying to put somewhere outside the map.");
            return;
        }

        if(selection == Selection.Soldier){
            if(playerArmy.atHandArmyInformation.soldierAmount <= 0){ // TODO: make another army informaiton for remaining, this is wrong.
                Debug.Log("Not enough soldiers");
            }

            else{
                GameObject addedSoldier = Instantiate(soldierPrefab);
                addedSoldier.transform.position = hit.point;
                addedSoldier.transform.SetParent(playerArmy.transform, true);
                playerArmy.atHandArmyInformation.soldierAmount -= 1;
                playerArmy.atBattlefieldArmyInformation.soldierAmount += 1;
            }
        }

        if(selection == Selection.Tank){
            if(playerArmy.atHandArmyInformation.tankAmount <= 0){
                Debug.Log("Not enough tanks");
            }

            else{
                GameObject addedTank = Instantiate(tankPrefab);
                addedTank.transform.position = hit.point;
                addedTank.transform.SetParent(playerArmy.transform, true);
                playerArmy.atHandArmyInformation.tankAmount -= 1;
                playerArmy.atBattlefieldArmyInformation.tankAmount += 1;
            }
        }

        Debug.Log(hit.point);
        Debug.Log("Trying to put " + SelectionToString(selection) + " here");
        Debug.Log("");
    }

    string SelectionToString(Selection selection){
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

}
