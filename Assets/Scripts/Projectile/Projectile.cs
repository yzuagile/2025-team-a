using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D collider;

    private Transform target;
    public GameObject hitEffect;
    private float moveSpeed;
    private float damage;
    private float lifetime = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        Destroy(gameObject, lifetime);
    }

    public void Setup(Transform target, float moveSpeed, float damage)
    {
        this.target = target;
        this.moveSpeed = moveSpeed;
        this.damage = damage;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            Vector2 currentMovement = transform.right * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + currentMovement);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
         
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Vector2 displacement = direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + displacement);
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyStats enemyStats = other.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(damage);
            GameObject hit = Instantiate(hitEffect, enemyStats.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(hit, 0.5f);

        }
    }
}
