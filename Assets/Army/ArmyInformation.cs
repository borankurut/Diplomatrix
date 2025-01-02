using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Diplomatrix;

public class ArmyInformation
{
    public ArmyAttributes initial;
    public ArmyAttributes atHand;
    public ArmyAttributes atBattlefield;

    public string totalInformation(){
        return "Initial Army: " + initial.ToString() + "\n" +
                "At Hand: " + atHand.ToString() + "\n" +
                "At Battlefield: " + atBattlefield.ToString() + "\n" + 
                "Current Total: " + currentArmyInformation().ToString();
    }
    public ArmyAttributes currentArmyInformation(){
        return new ArmyAttributes(
            atHand.soldierAmount + atBattlefield.soldierAmount, 
            atHand.tankAmount + atBattlefield.tankAmount,
            atHand.airStrikeAmount);      // airstrikes are just projectile, so they are one time use.
    }
    
}
