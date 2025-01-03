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

    [SerializeField] 
    EnemyPutHandler enemyPutHandler;
    protected override void Start(){
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        this.attackPattern = enemyPutHandler.attackPattern;
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
        //Debug.Log($"Ready ratios: soldier{SoldierReadyRatio()}, tank:{TankReadyRatio()}, air:{AirstrikeReadyRatio()}");
        //Debug.Log($"Stacks: soldier:{stacks.soldier}, tank:{stacks.tank}, air:{stacks.airStrike}");
    }

    void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        RaycastHit hit = new RaycastHit();

        bool hitToTerrain = false;

        foreach(var h in hits){
            if(h.collider.gameObject == terrainGrids.gameObject){
                hitToTerrain = true;
                hit = h;
                break;
            }
        }    

        if(!hitToTerrain){
            return;
        }

        else if(selection == Selection.Soldier && playerArmy.armyInformation.atHand.soldierAmount <= 0){
            Debug.Log("Not enough soldiers");
            return;
        }

        else if(selection == Selection.Tank && playerArmy.armyInformation.atHand.tankAmount <= 0){
            Debug.Log("Not enough tanks");
            return;
        }
        
        else if(selection == Selection.Airstrike && playerArmy.armyInformation.atHand.airStrikeAmount <= 0){
            Debug.Log("Not enough airstrikes");
            return;
        }

        else if(selection == Selection.Soldier && !AreThereSoldiersReady()){
            Debug.Log("Soldier is not ready");
            return;
        }
        
        else if(selection == Selection.Tank && !AreThereTanksReady()){
            Debug.Log("Tank is not ready");
            return;
        }

        else if(selection == Selection.Airstrike && !AreThereAirstrikeReady()){
            Debug.Log("Airstrike is not ready");
            return;
        }

        Put(selection, hit.point, playerArmy);
    }

    public int getSoldierStack(){
        return stacks.soldier;
    }
    
    public int getTankStack(){
        return stacks.tank;
    }
    
    public int getAirstrikeStack(){
        return stacks.airStrike;
    }

}
