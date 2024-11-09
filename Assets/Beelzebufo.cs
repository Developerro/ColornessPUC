using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beelzebufo : MonoBehaviour
{
    public Vector3 rightSide;
    public Vector3 leftSide;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        rightSide = new Vector3(184.3f, -0.74f, 0f);
        leftSide = new Vector3(167.51f, -0.74f, 0f);
        rightPosition();
    }

 
    void Update()
    {
        
               
    }

    void rightPosition()
    {
        transform.position = rightSide;
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, 3, 0);
        anim.Play("BeelzebufoJump");
    }
    void leftPosition()
    {
        transform.position = leftSide;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
