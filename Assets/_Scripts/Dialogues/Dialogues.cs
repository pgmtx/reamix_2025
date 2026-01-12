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

    Coroutine currentCoroutine;

    // Wrapper obligatoire pour que ce soit affiché dans l'inspecteur
    public void PrintSentence(string sentence)
    {
        // Stop la coroutine en cours si elle existe
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Lance la nouvelle coroutine et garde la référence
        currentCoroutine = StartCoroutine(PrintSentenceEnumerator(sentence));
    }

    IEnumerator PrintSentenceEnumerator(string sentence)
    {
        textObject.text = "";

        foreach (char c in sentence)
        {
            textObject.text += c;
            yield return new WaitForSeconds(timeBetweenChar);
        }

        // Fin de la coroutine : on réinitialise la référence
        currentCoroutine = null;
    }
}
