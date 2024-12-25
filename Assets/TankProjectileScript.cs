using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProjectileScript : MonoBehaviour
{
    public Transform enemyArmy;
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

    void Start()
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
        if (other.transform.IsChildOf(enemyArmy))
        {
            explosionCollider.enabled = true;
            explosionParticle.Play();
            impactParticle.Play();
            modelMeshRenderer.enabled = false;

            Collider mainCollider = GetComponent<Collider>();
            if (mainCollider != null)
            {
                mainCollider.enabled = false;
            }
            rb.isKinematic = true;
            Destroy(gameObject, 8.0f);
        }

        if (explosionCollider.enabled && other.transform.IsChildOf(enemyArmy))
        {
            ArmyMember armyMember = other.GetComponent<ArmyMember>();
            if (armyMember != null)
            {
                armyMember.getDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
