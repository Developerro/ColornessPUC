using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.Controls;


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
    public bool onWall;
    public bool StickOnWall;

    //Buffs
    public bool stick;

    public bool shoot; 
    public GameObject shootPrefab;
    public float shootSpeed = 10f;

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
        BuffsLogic();
        AbsorbLogic();
        KnockLogic();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            onWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            onWall = false;
        }
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
            if(enemy.buff == "stick")
            {
                stick = true;
            }
            if(enemy.buff == "shoot")
            {
                shoot = true;
            }
        }
    }

    void BuffsLogic()
    {
        //StickBuff
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.left, 2);
        if (stick)
        {
            if (onWall && !isGrounded)
            {
                animator.SetBool("onWall", true);
                StickOnWall = true;
                if (hit) 
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }

                pBody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                if (Input.GetKeyDown(KeyCode.W))
                {
                    onWall = false;
                    pBody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                    pBody.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                    pBody.velocity = new Vector2(pBody.velocity.x, jumpForce + 3);
                }
            }

            else
            {
                animator.SetBool("onWall", false);
                StickOnWall = false;
                KnockLogic();
            }
        }

        //ShootBuff
        if (shoot)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                float direction = spriteRenderer.flipX ? -1f : 1f;
                GameObject shootInstance = Instantiate(shootPrefab, transform.position, Quaternion.identity);
                Rigidbody2D shootRb = shootInstance.GetComponent<Rigidbody2D>();
                shootRb.velocity = new Vector2(direction * shootSpeed, 0);
            }
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
            if (!StickOnWall)
            {
                spriteRenderer.flipX = moveHorizontal < 0;
            }
            
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
