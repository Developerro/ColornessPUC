using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneTimer : MonoBehaviour
{
    [SerializeField] private float cutsceneDuration = 21f;
    private bool cutsceneSkipped = false;

    private void Start()
    {
        StartCoroutine(CutsceneTimer());
    }

    private void Update()
    {

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
        SceneManager.LoadScene("Main Menu");
    }
}