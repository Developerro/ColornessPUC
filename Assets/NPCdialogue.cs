using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NPCdialogue : MonoBehaviour
{
    public DialogueNPC DialogueNPC;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.name == "Player")
        {
            if (!DialogueNPC.talking)
            {
                DialogueNPC.talking = true;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}
