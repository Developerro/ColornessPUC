using System.Collections;
using UnityEngine;

public class Beelzebufo : EnemyState
{
    public Vector3 rightSide;
    public Vector3 leftSide;
    private Animator anim;
    private float t = 0.0f;
    public float transitionDuration = 2.0f;
    public float waitTime = 20.0f;
    private bool isMoving = false;
    private bool hasFlipped = false;
    public int previousLife;
    public PlayerController player;

    public Material flashMaterial;
    private Material originalMaterial;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        life = 50;
        previousLife = life;
        anim = GetComponent<Animator>();
        rightSide = new Vector3(184.3f, 0, 0f);
        leftSide = new Vector3(167.51f, 0, 0f);

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;

        rightPosition();
        StartCoroutine(CycleRoutine());
    }

    void Update()
    {
        if (life <= 0)
        {
            Destroy(gameObject);
        }
        Debug.Log(life);
        if (life < previousLife)
        {
            StartCoroutine(FlashEffect());
            previousLife = life; 
        }
        if (isMoving)
        {
            t += Time.deltaTime / transitionDuration;
            t = Mathf.Clamp01(t);

            transform.position = Vector3.Lerp(rightSide, leftSide, t);

            if (!hasFlipped && t >= 0.6f)
            {
                Flip();
                hasFlipped = true;
            }

            if (t >= 1.0f)
            {
                t = 0.0f;
                isMoving = false;
                hasFlipped = false;
                SwapSides();
                StartCoroutine(CycleRoutine());
            }
        }
    }

    IEnumerator CycleRoutine()
    {
        float elapsed = 0.0f;

        while (elapsed < waitTime)
        {
            anim.Play("BeelzebufoSmoke");
            yield return new WaitForSeconds(3f);
            elapsed += 3f;

            anim.Play("BeelzebufoTongue");
            yield return new WaitForSeconds(3f);
            elapsed += 3f;
        }

        StartCoroutine(StartJump());
    }

    IEnumerator StartJump()
    {
        anim.Play("BeelzebufoJump");
        yield return new WaitForSeconds(0.5f);
        isMoving = true;
    }


    IEnumerator FlashEffect()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);

        spriteRenderer.material = originalMaterial;
    }

    void rightPosition()
    {
        transform.position = rightSide;
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void leftPosition()
    {
        transform.position = leftSide;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void SwapSides()
    {
        Vector3 temp = rightSide;
        rightSide = leftSide;
        leftSide = temp;
    }

    void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.TakeDamage(transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.TakeDamage(transform.position);
        }
    }






}
