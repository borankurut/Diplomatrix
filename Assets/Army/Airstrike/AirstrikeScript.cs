using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirstrikeScript : TankProjectileScript 
{
    protected override void Start() 
    {
        thisArmy.GetComponent<ArmyScript>().armyInformation.atBattlefield.airStrikeAmount -= 1;
        base.Start();
    }
}
