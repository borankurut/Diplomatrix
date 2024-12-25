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
        thisArmy.currentArmyInformation.soldierAmount -= 1;
        base.die();
    }

    public override void shootTarget(){

        if(fireParticle.isPlaying)
            fireParticle.Stop();
        fireParticle.Play();

        base.shootTarget();
    }
}
