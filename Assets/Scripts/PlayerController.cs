using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public float speed;
    public string mode;
    public Image[] heart;
    public int life;
    public float jumpForce;
    public Rigidbody2D pBody;
    private BoxCollider2D pCollider;
    public bool isGrounded;
    private SpriteRenderer spriteRenderer;
    public Animator animator;
    public float kbForce;
    public float kbCount;
    public float kbTime;
    public bool isKnockRight;
    public EnemyState enemy;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pBody = GetComponent<Rigidbody2D>();
        pCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HealtLogic();
        DeadState();
        KnockLogic();
        AbsorbLogic();
    }

   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemy = collision.gameObject.GetComponent<EnemyState>();
            if (enemy.stunned)
            {
                enemy.absorbable = true;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemy.absorbable = false;
    }

    void AbsorbLogic()
    {
        
        if (Input.GetKeyDown(KeyCode.R) && enemy.absorbable)
        {
            mode = enemy.color;
            enemy.life--;
        }
        if (mode == "green")
        {
            animator.SetInteger("Mode", 1);
            jumpForce = 30;
            speed = 15;
        }
    }
    void MoveLogic()
    {
        isGrounded = Physics2D.BoxCast(pCollider.bounds.center, pCollider.bounds.size, 0f, Vector2.down, 1f);

        //Walk
        float moveHorizontal = Input.GetAxis("Horizontal");
        pBody.velocity = new Vector2(moveHorizontal * speed, pBody.velocity.y);
        animator.SetFloat("xVelocity", Math.Abs(pBody.velocity.x));

        //Jump
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            pBody.velocity = new Vector2(pBody.velocity.x, jumpForce);
        }
        animator.SetBool("IsJumping", !isGrounded);

        //Flip
        if (moveHorizontal != 0f)
        {
            spriteRenderer.flipX = moveHorizontal < 0;
        }
    }

    void HealtLogic()
    {
        if (life > 7)
        {
            life = 7;
        }
        for(int i = 0; i < heart.Length; i++)
        {
            if(i < life)
            {
                heart[i].enabled = true;
            }
            else
            {
                heart[i].enabled = false;
            }
        }
    }

    void DeadState()
    {
        if(life <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("LevelGreen");
        }
    }

    void KnockLogic()
    {
        if (kbCount < 0)
        {
            MoveLogic();
        }
        else
        {
            if (isKnockRight)
            {
                pBody.velocity = new Vector2(-kbForce, kbForce);
            }
            else
            {
                pBody.velocity = new Vector2(kbForce, kbForce);
            }
        }
        kbCount -= Time.deltaTime;
    }

}
