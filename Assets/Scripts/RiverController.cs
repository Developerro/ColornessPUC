using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverController : MonoBehaviour
{
    private float startPos;
    private float length;
    public float moveSpeed = 2f;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float newPosition = startPos - Time.time * moveSpeed;

        if (newPosition < startPos - length) newPosition += length;

        transform.position = new Vector3(newPosition, transform.position.y, transform.position.z);
    }
}
