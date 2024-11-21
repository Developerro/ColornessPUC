using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  
    private BoxCollider2D spawnArea;

    private float spawnTimer;

    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag != "Enemy")
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
          
}