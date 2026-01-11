using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogues : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMPro.TextMeshProUGUI textObject;
    public GameEvent PlayerOutBabypark;

    [Header("Print")]
    [SerializeField] private float timeBetweenChar = 0.05f;

    [Header("Sentences")]
    public List<string> sentences;

    private void Awake()
    {
        // Testing
        PlayerOutBabypark.TriggerEvent();
    }

    // Wrapper obligatoire pour que ce soit affiché dans l'inspecteur
    public void PrintSentence(int sentenceId)
    {
        StartCoroutine(PrintSentenceEnnumerator(sentenceId));
    }

    IEnumerator PrintSentenceEnnumerator(int sentenceId)
    {
        textObject.text = "";

        foreach (char c in sentences[sentenceId])
        {
            textObject.text += c;
            yield return new WaitForSeconds(timeBetweenChar);
        }
    }
}
