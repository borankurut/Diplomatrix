using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : ArmyMemberController
{
    [SerializeField]
    private Transform turret;

    [SerializeField]
    private float turretRotationSpeed;

    protected override bool followTarget()
    {
        if (enemyTarget == null)
            return false;

        float distance = Vector3.Distance(transform.position, enemyTarget.transform.position);

        Vector3 direction = (enemyTarget.transform.position - transform.position).normalized;

        direction.y = 0;

        // Slow body rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Turret rotation
        RotateTurretTowardsTarget(enemyTarget.transform.position);

        Debug.Log("rb.velocity: " + rb.velocity);

        if (distance > range)
        {
            rb.MovePosition(transform.position + direction * movementSpeed * Time.fixedDeltaTime);
            state = State.Moving;
            return true;
        }
        else
        {
            rb.velocity = Vector3.zero;
            return false;
        }
    }

    private void RotateTurretTowardsTarget(Vector3 targetPosition)
    {
        if (turret == null)
            return;

        Vector3 turretDirection = (targetPosition - turret.position).normalized;

        turretDirection.y = 0;

        Quaternion turretTargetRotation = Quaternion.LookRotation(turretDirection);

        Debug.Log(turret.rotation);

        turret.rotation = Quaternion.Slerp(turret.rotation, turretTargetRotation, turretRotationSpeed * Time.deltaTime);
    }

    public override void shootTarget()
    {
        base.shootTarget();
        Debug.Log("Tank is shooting from turret!");
    }
}

