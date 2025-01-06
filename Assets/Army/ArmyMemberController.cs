using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ArmyMember))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class ArmyMemberController: MonoBehaviour
{

    [SerializeField]
    protected float movementSpeed;

    [SerializeField]
    protected float rotationSpeed;

    [SerializeField]
    protected float range;

    [SerializeField]
    protected ArmyMember enemyTarget;

    protected ArmyScript thisArmy;
    protected Transform enemies;

    protected ArmyMember thisArmyMember;

    protected Rigidbody rb;

    protected Animator animator;

    protected string movingStr = "moving";
    protected string shootingStr = "shooting"; 
    protected string deadStr= "dead"; 

    protected enum State{Unknown, Idle, Moving, Shooting, Dead};

    [SerializeField]
    protected State state;

    void Start(){
        thisArmyMember = GetComponent<ArmyMember>();
        thisArmy = GetComponentInParent<ArmyScript>();
        enemies = thisArmy.enemyArmy;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        state = State.Idle;
        enemyTarget = closestTarget();
    }

    void Update(){
        //Debug.Log("health: " + thisArmyMember.Health + ", state" + state);

        if(state == State.Dead)
            return;

        handleState();

        if(enemyTarget && enemyTarget.IsDead){
            enemyTarget = null;
        }

        if(thisArmyMember.IsDead){
            state = State.Dead;
            handleState();
        }

        else if(enemyTarget == null || enemyTarget != closestTarget()){
            enemyTarget = closestTarget();

            state = State.Idle;
        }

        else if(enemyTarget){
            bool isFollowing = followTarget();

            if(!isFollowing){ // close enough, shoot.
                state = State.Shooting;
            }
        }
    }

    void handleState(){
        makeAllStateFalse();
        if(state == State.Idle || state == State.Dead){
            rb.isKinematic = true;
        }

        else{
            rb.isKinematic = false;
        }

        if(state == State.Shooting){
            animator.SetBool(shootingStr, true);
        }

        else if(state == State.Moving){
            animator.SetBool(movingStr, true);
        }

        else if(state == State.Dead){
            animator.SetBool(deadStr, true);
            rb.isKinematic = true;
            die();
        }
    }

    void makeAllStateFalse(){
        animator.SetBool(movingStr, false);
        animator.SetBool(shootingStr, false);
        animator.SetBool(deadStr, false);
    }

    protected virtual bool followTarget(){
        if(enemyTarget == null)
            return false;

        float distance = Vector3.Distance(transform.position, enemyTarget.transform.position);

        Vector3 direction = (enemyTarget.transform.position - transform.position).normalized;

        direction.y = 0;

        //rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        if(distance > range){
            rb.MovePosition(transform.position + direction * movementSpeed * Time.fixedDeltaTime);
            state = State.Moving;
            return true;
        }

        else{
            return false;
        }

    }

    ArmyMember closestTarget(){
        if (enemies == null || enemies.childCount == 0)
            return null;

        float minDistance = Mathf.Infinity;
        ArmyMember closest = null;

        foreach (Transform enemyTransform in enemies)
        {
            ArmyMember enemy = enemyTransform.GetComponent<ArmyMember>();
            if (enemy == null || enemy.IsDead)
                continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    public virtual void shootTarget(){
        if(enemyTarget)
            enemyTarget.getDamage(thisArmyMember.Damage);
    }

    protected virtual void die(){
        //Debug.Log("die is called. I HOPE WORKS PLEAS");
        
        Destroy(gameObject, 5f);
    }

}

