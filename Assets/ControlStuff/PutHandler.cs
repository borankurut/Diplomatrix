using System;
using System.Collections;
using System.Collections.Generic;
using Diplomatrix;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PutHandler : MonoBehaviour
{
    [SerializeField]
    protected TerrainGrids terrainGrids;

    protected enum Selection {Unknown, Soldier, Tank, Airstrike};

    protected struct Stacks{
        public int soldier;
        public int tank;
        public int airStrike;
    }
    protected struct LastPrepareTimes{
        public float soldier;
        public float tank;
        public float airStrike;
    }

    public AttackPattern attackPattern = new AttackPattern(5, 10, 20);

    [SerializeField]
    GameObject soldierPrefab;

    [SerializeField]
    GameObject tankPrefab;

    [SerializeField]
    GameObject airstrikePrefab;
    protected Stacks stacks = new Stacks();
    protected LastPrepareTimes lastPrepareTimes = new LastPrepareTimes();

    protected virtual void Start()
    {
        lastPrepareTimes.soldier = float.NegativeInfinity;
        lastPrepareTimes.tank = float.NegativeInfinity;
        lastPrepareTimes.airStrike = float.NegativeInfinity;

        StartCoroutine(PrepareSoldierRoutine());
        StartCoroutine(PreapareTankRoutine());
        StartCoroutine(PrepareAirAttackRoutine());
    }

    protected virtual void Update()
    {
        //Debug.Log($"stacks: {stacks.soldier}, {stacks.tank}, {stacks.airStrike}");
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
        else if(selection == Selection.Airstrike){
            return "Airstrike";
        }

        return "NULL";
    }

    protected void Put(Selection selection, Vector3 position, ArmyScript army){
        bool isValidPlace = selection == Selection.Airstrike && terrainGrids.IsValidCoordinate(position)                            ||
                            army.GetArmyType() == ArmyScript.ArmyType.playerArmy && terrainGrids.IsValidInsidePlayerSide(position)  ||
                            army.GetArmyType() == ArmyScript.ArmyType.NPCArmy && terrainGrids.IsValidInsideEnemySide(position);

        
        if(!isValidPlace){
            Debug.Log($"army type {(army.GetArmyType() == ArmyScript.ArmyType.playerArmy ? "player": "npc")} is trying to put something on invalid place");
            return;
        }

        if(selection == Selection.Soldier){

            if(army.armyInformation.atHand.soldierAmount <= 0)
                return;

            GameObject addedSoldier = Instantiate(soldierPrefab);
            addedSoldier.transform.position = position;
            addedSoldier.transform.SetParent(army.transform, true);
            army.armyInformation.atHand.soldierAmount -= 1;
            army.armyInformation.atBattlefield.soldierAmount += 1;
        }

        else if(selection == Selection.Tank){

            if(army.armyInformation.atHand.tankAmount <= 0)
                return;

            GameObject addedTank = Instantiate(tankPrefab);
            addedTank.transform.position = position;
            addedTank.transform.SetParent(army.transform, true);
            army.armyInformation.atHand.tankAmount-= 1;
            army.armyInformation.atBattlefield.tankAmount+= 1;
        }

        else if(selection == Selection.Airstrike){

            if(army.armyInformation.atHand.airStrikeAmount <= 0)
                return;
            // Debug.Log("AIRSTRIKE PUT CALLED, TO BE IMPLEMENTED.");


            // GameObject projectile = Instantiate(projectilePrefab, projectileStartTransform.position, projectileStartTransform.rotation);
            GameObject airstrike = Instantiate(airstrikePrefab, position + Vector3.up * 30.0f, quaternion.RotateX((float)Math.PI));
            airstrike.GetComponent<AirstrikeScript>().enemyArmy = army.enemyArmy;
            airstrike.GetComponent<AirstrikeScript>().thisArmy = army.transform;
            airstrike.GetComponent<Rigidbody>().velocity = Vector3.down * 40.0f;
            airstrike.transform.SetParent(army.transform, true);
            army.armyInformation.atHand.airStrikeAmount -= 1;
            army.armyInformation.atBattlefield.airStrikeAmount += 1;
        }

        // checks are valid, send unit.
        if(selection == Selection.Soldier){
            stacks.soldier -= 1;
        }
        else if(selection == Selection.Tank){
            stacks.tank -= 1;
        }
        else if(selection == Selection.Airstrike){
            stacks.airStrike -= 1;
        }

        Debug.Log("Trying to put " + SelectionToString(selection) + " here: " + position);
    }

    protected bool AreThereSoldiersReady(){
        return stacks.soldier > 0;
    }

    protected bool AreThereTanksReady(){
        return stacks.tank > 0;
    }

    protected bool AreThereAirstrikeReady(){
        return stacks.airStrike > 0;
    }

    public float SoldierReadyRatio(){
        return (Time.time - lastPrepareTimes.soldier) / attackPattern.getSoldierAttackPeriod();
    }

    public float TankReadyRatio(){
        return (Time.time - lastPrepareTimes.tank) / attackPattern.getTankAttackPeriod();
    }

    public float AirstrikeReadyRatio(){
        return (Time.time - lastPrepareTimes.airStrike) / attackPattern.getAirAttackPeriod();
    }

    private IEnumerator PrepareSoldierRoutine(){
        while(true){
            stacks.soldier++;
            lastPrepareTimes.soldier = Time.time;
            yield return new WaitForSeconds(attackPattern.getSoldierAttackPeriod());
        }
    }

    private IEnumerator PreapareTankRoutine(){
        while(true){
            stacks.tank++;
            lastPrepareTimes.tank = Time.time;
            yield return new WaitForSeconds(attackPattern.getTankAttackPeriod());
        }
    }

    private IEnumerator PrepareAirAttackRoutine(){
        while(true){
            stacks.airStrike++;
            lastPrepareTimes.airStrike = Time.time;
            yield return new WaitForSeconds(attackPattern.getAirAttackPeriod());
        }
    }

}
