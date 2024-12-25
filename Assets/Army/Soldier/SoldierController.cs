using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoldierArmyMember))]

public class SoldierController : ArmyMemberController
{
    protected override void die()
    {
        thisArmy.currentArmyInformation.soldierAmount -= 1;
        base.die();
    }
}
