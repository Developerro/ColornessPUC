using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMonkTrigger : MonoBehaviour
{
    public OldMonkFrog OldMonkFrog;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OldMonkFrog.InitDialogue();
        }
    }
}
