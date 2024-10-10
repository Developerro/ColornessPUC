using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState: MonoBehaviour
{
    public bool stunned;
    public string color;
    public bool absorbable;
    public int life;
}

public class EnemyPatrolling : EnemyState
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    public PlayerController player;
    private Animator anim;
    private Transform currentPoint;
    public float speed;
    public GameObject tip;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        color = "green";
        stunned = false;
    }

    private void Update()
    {
        LifeLogic();
        
    }

    private void LifeLogic()
    {
        anim.SetInteger("Life", life);
        if(life > 1)
        {
            MoveLogic();
        }
        if(life == 1)
        {
            stunned = true;
        }
        if(life <= 0)
        {
            tip.SetActive(false);
            Destroy(gameObject, 0.6f);
        }
        if (stunned && life > 0)
        {
            tip.SetActive(true);
        }

    }
    private void MoveLogic()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            flip();
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            flip();
            currentPoint = pointB.transform;
        }
    }
    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

            if(transform.position.y + 0.5f < collision.transform.position.y)
            {
                //Temporário
                if (!stunned)
                {
                    life--;
                }
                player.pBody.velocity = new Vector2(0, 15);
                anim.SetTrigger("TakeDamage");
                Debug.Log(life);
            }
            else
            {
                player.kbCount = player.kbTime;
                if (collision.transform.position.x <= transform.position.x)
                {
                    player.isKnockRight = true;
                }
                if (collision.transform.position.x > transform.position.x)
                {
                    player.isKnockRight = false;
                }
                player.life--;
                player.animator.SetTrigger("TakeDamage");
            }

            
           
        }
    }
}
