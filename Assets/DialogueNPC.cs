using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueNPC : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private TypewriterEffect typewriterEffect;
    [SerializeField] private float delayBetweenTexts = 1f;
    public bool talking;

    private bool isCoroutineRunning = false;

    private List<string> texts = new List<string>
    {
        "Opa, tudo bem a�?!",
        "Claramente voc� n�o � daqui",
        "Mas n�o tem problema",
        "Desde que o chefe n�o te veja",
        "Pois a calma que a gente tem",
        "S� acontece se as cores n�o se veem",
        "Ha Ha Ha, bem-vindo, por�m seja r�pido!"
    };

    private void Update()
    {
        if (talking && !isCoroutineRunning)
        {
            StartCoroutine(DisplayTextsSequentially());
        }
    }

    private IEnumerator DisplayTextsSequentially()
    {
        isCoroutineRunning = true;

        foreach (string text in texts)
        {
            textLabel.text = "";
            yield return typewriterEffect.Run(text, textLabel);
            yield return new WaitForSeconds(delayBetweenTexts);
        }

        talking = false; // Opcional: Evita repetir os textos
        isCoroutineRunning = false;
    }
}
