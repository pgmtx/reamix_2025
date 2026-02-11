using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVDialogues : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMPro.TextMeshProUGUI textObject;
    [SerializeField] private AudioSource typingAudio;
    private float typingPitch;
    private float typingVolume;

    [Header("Print")]
    [SerializeField] private float timeBetweenChar = 0.05f;

    Coroutine currentCoroutine;

    private void Awake()
    {
        typingPitch = typingAudio.pitch;
        typingVolume = typingAudio.volume;
    }

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
            // Play sound
            typingAudio.Play();
            typingAudio.pitch = typingPitch + UnityEngine.Random.Range(-0.007f, 0.007f);
            typingAudio.volume = typingVolume + UnityEngine.Random.Range(-0.01f, 0.01f);

            yield return new WaitForSeconds(timeBetweenChar);
        }

        // Fin de la coroutine : on réinitialise la référence
        currentCoroutine = null;
    }
}
