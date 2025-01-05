using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoldierArmyMember))]

public class SoldierController : ArmyMemberController
{
    [SerializeField]
    private ParticleSystem fireParticle;
    protected override void die()
    {
        thisArmy.armyInformation.atBattlefield.soldierAmount -= 1;
        transform.position = new Vector3(transform.position.x, -0.13f, transform.position.z);    // soldiers dying on air fix.
        base.die();
    }

    public override void shootTarget(){

        if(fireParticle.isPlaying)
            fireParticle.Stop();
        fireParticle.Play();

        base.shootTarget();
    }
}
