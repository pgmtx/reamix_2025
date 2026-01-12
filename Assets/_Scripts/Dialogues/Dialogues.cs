using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogues : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMPro.TextMeshProUGUI textObject;

    [Header("Print")]
    [SerializeField] private float timeBetweenChar = 0.05f;

    // Wrapper obligatoire pour que ce soit affiché dans l'inspecteur
    public void PrintSentence(string sentence)
    {
        StopCoroutine("PrintSentenceEnnumerator");
        StartCoroutine(PrintSentenceEnnumerator(sentence));
    }

    IEnumerator PrintSentenceEnnumerator(string sentence)
    {
        textObject.text = "";

        foreach (char c in sentence)
        {
            textObject.text += c;
            yield return new WaitForSeconds(timeBetweenChar);
        }
    }
}
