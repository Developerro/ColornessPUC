using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Added to handle scene transitions

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private TypewriterEffect typewriterEffect;
    [SerializeField] private float delayBetweenTexts = 1f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("LevelGreen");
        }
    }
    private List<string> texts = new List<string>
    {
    "Ha muito tempo, as cores brilhavam e os reinos viviam em paz. Culturas unicas coexistiam, compartilhando arte, musica e tradicoes.",
    "Entao... ele chegou.",
    "Sua chegada trouxe o fim. As cores sumiram, levando suas respectivas culturas. Mas um jovem do Reino Vermelho se manteve esperancoso.",
    "Ele sonhava restaurar as cores e a riqueza das culturas perdidas. Um dia, descobriu que podia tentar.",
    "Mas seu pecado o lancou ao abismo…",
    "..."
    };


    private void Start()
    {
        StartCoroutine(DisplayTextsSequentially());
    }

    private IEnumerator DisplayTextsSequentially()
    {
        foreach (string text in texts)
        {
            textLabel.text = ""; 
            yield return typewriterEffect.Run(text, textLabel); 
            yield return new WaitForSeconds(delayBetweenTexts);
        }


        SceneManager.LoadScene("LevelGreen");
    }
}
