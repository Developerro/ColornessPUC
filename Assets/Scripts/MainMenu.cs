using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public PlayerController player;
    void Update()
    {
        string nameButton = EventSystem.current.currentSelectedGameObject.name;
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (nameButton == "Play")
            {
                PlayGame();
            }
            if (nameButton == "Quit")
            {
                QuitGame();
            }
        }
       
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        //Application.Quit();
    }
}
