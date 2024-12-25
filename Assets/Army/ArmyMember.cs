using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyMember : MonoBehaviour
{
    [SerializeField]
    private int _health;

    [SerializeField]
    private int _damage;

    public int Health{
        get{ return _health; }
        private set{ _health = Mathf.Max(value, 0); }
    }

    public int Damage{
        get{ return _damage; }
        private set{ _damage = value; }
    }

    public bool IsDead{
        get{ return _health <= 0; }
    }
    
    public virtual void getDamage(int damage){
        Health -= damage;
    }
}
