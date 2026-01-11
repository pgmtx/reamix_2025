using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogues : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMPro.TextMeshProUGUI textObject;
    public GameEvent playerMoved;

    // Private reference for the Audio Source component
    private AudioSource m_AudioSource;

    [Header("Print")]
    [SerializeField] private float timeBetweenChar = 0.05f;
    [SerializeField] private float timeBetweenSentences = 2f;
    private float timer;
    int sentenceIndex = 0;
    int letterIndex = 0;

    [Header("Sentences")]
    public List<string> sentences;
    public List<ListInt> sequences;

    private void Awake()
    {
        // Get the Audio Source attached to this same GameObject
        m_AudioSource = GetComponent<AudioSource>();
        if (m_AudioSource == null)
        {
            Debug.LogWarning("TravisDialogue is missing an AudioSource component on the GameObject!");
        }

        textObject.text = "";
        playerMoved.TriggerEvent();
    }

    // id de la sequence
    public void PrintSequence(int i)
    {
        StartCoroutine(PrintSequence(sequences[i].row));
    }

    // Coroutine to stop the sound after a specified delay
    private IEnumerator StopSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Stop the sound if it is still playing
        if (m_AudioSource != null && m_AudioSource.isPlaying)
        {
            m_AudioSource.Stop();
        }
    }

    // id de chaque phrase de la sequence
    IEnumerator PrintSequence(List<int> sentenceIds)
    {
        // Define the maximum duration for the sound effect (5 seconds)
        const float maxSoundDuration = 5f;

        foreach (int id in sentenceIds)
        {
            // 1. PLAY THE SOUND EFFECT
            if (m_AudioSource != null)
            {
                m_AudioSource.Play();
            }

            // 2. START THE SOUND STOP TIMER (Runs in parallel)
            Coroutine soundStopCoroutine = StartCoroutine(StopSoundAfterDelay(maxSoundDuration));

            // 3. WAIT FOR THE SENTENCE TO FINISH PRINTING
            yield return StartCoroutine(PrintSentence(id));

            // 4. STOP THE SOUND TIMER and the sound if the sentence finished first
            StopCoroutine(soundStopCoroutine);

            if (m_AudioSource != null && m_AudioSource.isPlaying)
            {
                m_AudioSource.Stop();
            }

            // 5. THEN WAIT FOR THE INTER-SENTENCE PAUSE
            yield return new WaitForSeconds(timeBetweenSentences);
        }
    }

    IEnumerator PrintSentence(int sentenceId)
    {
        textObject.text = "";

        foreach (char c in sentences[sentenceId])
        {
            textObject.text += c;
            yield return new WaitForSeconds(timeBetweenChar);
        }
    }
}
