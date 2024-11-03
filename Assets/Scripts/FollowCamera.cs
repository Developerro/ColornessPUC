using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollowCamera : MonoBehaviour
{
    public PlayerController player;
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Menu();
        }
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);

        if(player.falling == true)
        {
            float offsetX = Random.Range(-0.05f, 0.05f);
            float offsetY = Random.Range(-0.05f, 0.05f);
            transform.position += new Vector3(offsetX, offsetY, 0);
        }
    }

    public void Menu()
    {
        SceneManager.LoadSceneAsync(0);
        
    }
}
