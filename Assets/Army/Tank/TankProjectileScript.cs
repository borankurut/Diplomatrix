using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProjectileScript : MonoBehaviour
{
    public Transform enemyArmy;
    public Transform thisArmy;
    public int damage;

    [SerializeField]
    private float radius;

    [SerializeField]
    ParticleSystem explosionParticle;

    [SerializeField]
    ParticleSystem impactParticle;

    [SerializeField]
    MeshRenderer modelMeshRenderer;

    private SphereCollider explosionCollider;
    private Rigidbody rb;

    private float timeAlive = 0f;
    [SerializeField]
    private float maxLifeTime = 30f;

    private HashSet<Collider> alreadyDamaged = new HashSet<Collider>();

    protected virtual void Start()
    {
        explosionCollider = gameObject.AddComponent<SphereCollider>();
        explosionCollider.isTrigger = true;
        explosionCollider.radius = radius;
        explosionCollider.enabled = false;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        timeAlive += Time.deltaTime;

        if (timeAlive > maxLifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        bool isFriend = other.transform.IsChildOf(thisArmy);

        if (!isFriend && !explosionCollider.enabled)
        {
            Debug.Log(other.name);
            explosionCollider.enabled = true;
            explosionParticle.Play();

            if(impactParticle){
                impactParticle.Play();
            }

            modelMeshRenderer.enabled = false;

            Collider mainCollider = GetComponent<Collider>();
            if (mainCollider != null)
            {
                mainCollider.enabled = false;
            }
            rb.isKinematic = true;
            Invoke("disableExplosion", 0.1f);
            Destroy(gameObject, 1.5f);
        }

        ArmyMember armyMember = other.GetComponent<ArmyMember>();
        if (explosionCollider.enabled && other.transform.IsChildOf(enemyArmy) && !alreadyDamaged.Contains(other))
        {
            if (armyMember != null)
            {
                armyMember.getDamage(damage);
                //Debug.Log("tank dealt damage.");
                alreadyDamaged.Add(other);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void disableExplosion(){
        if (explosionCollider != null)
            explosionCollider.enabled = false;
    }
}
