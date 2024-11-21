using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : EnemyState
{
    public Vector2[] waypoints;  
    public float speed = 2f;    
    private int currentWaypointIndex = 0;
    public PlayerController player;
    public int previousLife;

    public Material flashMaterial;
    private Material originalMaterial;
    private SpriteRenderer spriteRenderer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.TakeDamage(transform.position);
        }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;

        life = 3;
        previousLife = life;
        
    }
    void Update()
    {
        MoveToWaypoint();
        if (life < previousLife)
        {
            StartCoroutine(FlashEffect());
            previousLife = life;
        }
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    void MoveToWaypoint()
    {
        if (waypoints.Length == 0) return;

        Vector2 targetWaypoint = waypoints[currentWaypointIndex];

        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWaypoint) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    IEnumerator FlashEffect()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.material = originalMaterial;
    }
}
