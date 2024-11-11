using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public PlayerController player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
        Vector2 shootDirection = player.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        rb.velocity = shootDirection * speed;
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyState enemy = collision.GetComponent<EnemyState>();
        if (collision.CompareTag("Enemy"))
        {
            if (enemy != null)
            {
                enemy.TakeDamage(20);
                Destroy(gameObject);
            }
            
        }
 
        if (collision.CompareTag("Ground") || collision.CompareTag("Wall"))
        {

            Destroy(gameObject);
        }
    }
}
