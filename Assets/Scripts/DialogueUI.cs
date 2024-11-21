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

    private List<string> texts = new List<string>
    {
        "H� muito tempo, quando as cores ainda brilhavam vivas, os reinos eram unidos, e as pessoas viviam em paz. Culturas diferentes coexistiam em uma harmonia rara e preciosa. Cada reino celebrava sua pr�pria arte, m�sica e sabedoria, compartilhando entre si suas tradi��es e aprendizados.",
        "Mas ent�o... ele veio.",
        "Desde sua chegada, o mundo nunca mais foi o mesmo. As cores desapareceram, e a uni�o entre os reinos se desfez. Por�m, entre os escombros de um antigo mundo, um pequeno garoto do extinto Reino Vermelho se manteve esperan�oso.",
        "Ele desejava mudar as coisas. Ele sonhava em fazer a diferen�a. E um dia, descobriu que... podia. Ele tinha uma chance de restaurar aquilo que foi perdido.",
        "Mas, devido ao seu pecado, ele foi lan�ado ao abismo�",
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
