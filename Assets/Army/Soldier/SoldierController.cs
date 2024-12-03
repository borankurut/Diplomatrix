using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SoldierArmyMember))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class SoldierController : MonoBehaviour
{

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float range;

    [SerializeField]
    ArmyMember enemyTarget;

    ArmyMember thisArmyMember;

    Rigidbody rb;

    Animator animator;

    private string runningStr = "running";
    private string shootingStr = "shooting"; 
    private string deadStr= "dead"; 

    enum State{Unknown, Idle, Running, Shooting, Dead};

    [SerializeField]
    private State state;

    void Start(){
        thisArmyMember = GetComponent<ArmyMember>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        state = State.Idle;
    }

    void Update(){
        Debug.Log("health: " + thisArmyMember.Health + ", state" + state);

        if(thisArmyMember.Health <= 0){
            state = State.Dead;
        }

        else if(enemyTarget == null){
            enemyTarget = closestTarget();

            state = State.Idle;
        }

        else if(enemyTarget){
            bool isFollowing = followTarget();

            if(!isFollowing){ // close enough, shoot.
                state = State.Shooting;
            }
        }

        handleState();
    }

    void handleState(){
        makeAllStateFalse();
        if(state == State.Idle){
        }

        else if(state == State.Shooting){
            animator.SetBool(shootingStr, true);
        }

        else if(state == State.Running){
            animator.SetBool(runningStr, true);
        }

        else if(state == State.Dead){
            animator.SetBool(deadStr, true);
        }
    }

    void makeAllStateFalse(){
        animator.SetBool(runningStr, false);
        animator.SetBool(shootingStr, false);
        animator.SetBool(deadStr, false);
    }

    bool followTarget(){
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
            state = State.Running;
            return true;
        }

        else{
            rb.velocity = Vector3.zero;
            return false;
        }

    }

    ArmyMember closestTarget(){
        // get the closest target
        return null;
    }

    public void shootTarget(){
        enemyTarget.getDamage(thisArmyMember.Damage);
    }

    public void die(){
        this.enabled = false;
    }

}
