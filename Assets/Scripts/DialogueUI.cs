using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private float cutsceneDuration = 43f;
    private bool cutsceneSkipped = false;

    private void Start()
    {
        StartCoroutine(CutsceneTimer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SkipCutscene();
        }
    }

    private IEnumerator CutsceneTimer()
    {
        yield return new WaitForSeconds(cutsceneDuration);
        if (!cutsceneSkipped)
        {
            LoadNextScene();
        }
    }

    private void SkipCutscene()
    {
        cutsceneSkipped = true;
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("LevelGreen");
    }
}