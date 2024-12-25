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

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    float projectileVelocity = 10f;

    [SerializeField]
    Transform projectileStartTransform;

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

        //Debug.Log("rb.velocity: " + rb.velocity);

        if (distance > range)
        {
            rb.MovePosition(transform.position + direction * movementSpeed * Time.fixedDeltaTime);
            state = State.Moving;
            return true;
        }
        else
        {
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

        //Debug.Log(turret.rotation);

        turret.rotation = Quaternion.Slerp(turret.rotation, turretTargetRotation, turretRotationSpeed * Time.deltaTime);
    }

    public override void shootTarget()
    {
        Debug.Log("Tank shoot target is called.");

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, projectileStartTransform.position, projectileStartTransform.rotation);
        projectile.GetComponent<TankProjectileScript>().damage = thisArmyMember.Damage;
        projectile.GetComponent<TankProjectileScript>().enemyArmy = thisArmy.enemyArmy;
        
        // Apply initial velocity to the projectile based on the tank's forward direction
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply velocity in the forward direction of the tank's rotation
            rb.velocity = transform.forward * projectileVelocity;
        }
    }
}

