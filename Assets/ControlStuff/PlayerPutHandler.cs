using System;
using System.Collections;
using System.Collections.Generic;
using Diplomatrix;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPutHandler : PutHandler 
{

    [SerializeField]
    ArmyScript playerArmy;
    Selection selection;

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

        if(selection == Selection.Tank && playerArmy.atHandArmyInformation.tankAmount <= 0){
            Debug.Log("Not enough tanks");
            return;
        }
        else if(selection == Selection.Soldier && playerArmy.atHandArmyInformation.soldierAmount <= 0){
            Debug.Log("Not enough soldiers");
            return;
        }
        Put(selection, hit.point, playerArmy);
    }
}
