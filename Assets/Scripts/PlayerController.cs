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
    public FollowCamera PlayerCamera;
    public bool falling;
    public Vector3 savePoint;
    public Vector3 initialSavePoint;
    //BUFFS

    //Stick
    public bool stick;

    //Shoot
    public bool shoot;
    public GameObject ShootPrefab;
    public Transform ShootingPoint;
    public float shootCooldown = 0.5f; 
    private float lastShootTime;

    // Start is called before the first frame update
    void Start()
    {
        initialSavePoint = new Vector3(-27.2f, 530.13f, 0f);
        savePoint  = new Vector3(-27.2f, 530.13f, 0f);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pBody = GetComponent<Rigidbody2D>();
        pCollider = GetComponent<BoxCollider2D>();
        //kbCount = -1;
        //if(SceneManager.GetActiveScene().name == "LevelGreen")
        //{
        //    pBody.AddForce(-Vector2.up * 10); 
        //}


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
        if (collision.gameObject.tag == "Wall")
        {
            onWall = true;
        }
        if (collision.gameObject.tag == "DamageArea")
        {
            TakeDamage(collision);
        }
        if (collision.gameObject.tag == "DeathArea")
        {
            life = 0;
        }
        //if (collision.gameObject.tag == "FallArea")
        //{
        //    pBody.AddForce(-Vector2.up * 10);
        //}

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
        if (collision.gameObject.tag == "FallArea")
        {
            falling = true;
            animator.SetBool("Falling", true);
            pBody.gravityScale = 0.1f;
            PlayerCamera.FollowSpeed = 10;
        }
        if (collision.gameObject.tag == "SavePoint")
        {
            savePoint = collision.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        try { enemy.absorbable = false; }
        catch { }
        if (collision.gameObject.tag == "FallArea")
        {
            falling = false;
            animator.SetBool("Falling", false);
            pBody.gravityScale = 5;
            PlayerCamera.FollowSpeed = 4;
        }

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
        else
        {
            animator.SetInteger("Mode", 0);
        }
    }

    void BuffsLogic()
    {
        //StickBuff
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.localScale.x > 0 ? Vector2.right : Vector2.left, 2);
        if (stick)
        {
            if (onWall && !isGrounded)
            {
                animator.SetBool("onWall", true);
                StickOnWall = true;

                if (hit.collider != null)
                {
                    if (hit.point.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }
                    else 
                    {
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }
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
            if (Input.GetKeyDown(KeyCode.F) && Time.time >= lastShootTime + shootCooldown)
            {
                Instantiate(ShootPrefab, ShootingPoint.position, transform.rotation);
                lastShootTime = Time.time;
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
        if (!StickOnWall && !falling)
        {
            if (moveHorizontal < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (moveHorizontal > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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
            if(savePoint == initialSavePoint)
            {
                Destroy(gameObject);
                SceneManager.LoadScene("LevelGreen");              
            }
            else
            {
                transform.position = savePoint;
            }
            //mode = "";
            //stick = false;
            //shoot = false;
            life = 7;     
        }
    }

    void KnockLogic()
    {
        if (kbCount <= 0)
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

    public void TakeDamage(Collision2D collision)
    {
        kbCount = kbTime;
        if (collision.transform.position.x <= transform.position.x)
        {
            isKnockRight = true;
        }
        if (collision.transform.position.x > transform.position.x)
        {
            isKnockRight = false;
        }
        life--;
        animator.SetTrigger("TakeDamage");
    }

}
