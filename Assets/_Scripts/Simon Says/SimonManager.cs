using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SimonManager : MonoBehaviour
{
    [Header("UI Setup")]
    public GameObject startButton; // Drag your Start Button here in the Inspector

    [Header("Buttons Setup")]
    public GameObject[] gameButtons; // Assign your 4 buttons here
    public Material[] buttonMaterials; // Original materials
    public Color flashColor = Color.white; // The color when it glows

    private List<int> gameSequence = new List<int>();
    private int playerStep = 0;
    private bool isInputEnabled = false;

    [Header("Audio")]
    public AudioSource[] buttonSounds; // Assign unique beeps for each button

    public void StartGame()
    {
        startButton.SetActive(false); // This hides the button immediately
        gameSequence.Clear();
        playerStep = 0;
        AddNextStep();
    }

    void AddNextStep()
    {
        gameSequence.Add(Random.Range(0, gameButtons.Length));
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        isInputEnabled = false;
        yield return new WaitForSeconds(1f);

        foreach (int index in gameSequence)
        {
            FlashButton(index);
            yield return new WaitForSeconds(0.7f);
        }

        isInputEnabled = true;
        playerStep = 0;
    }

    public void PlayerPressedButton(int buttonID)
    {
        if (!isInputEnabled) return;

        // Visual and audio feedback for the player press
        FlashButton(buttonID);

        if (buttonID == gameSequence[playerStep])
        {
            playerStep++;
            if (playerStep == gameSequence.Count)
            {
                Invoke("AddNextStep", 0.5f);
            }
        }
        else
        {
            Debug.Log("Game Over!");
            // Optional: Play a "buzz" sound here
            gameSequence.Clear();
            isInputEnabled = false;
        }
    }

    void FlashButton(int index)
    {
        StartCoroutine(FlashEffect(index));
        if (buttonSounds[index] != null) buttonSounds[index].Play();
    }

    IEnumerator FlashEffect(int index)
    {
        Renderer rend = gameButtons[index].GetComponent<Renderer>();
        rend.material.EnableKeyword("_EMISSION");
        rend.material.SetColor("_EmissionColor", flashColor * 2f); // Bright glow

        yield return new WaitForSeconds(0.4f);

        rend.material.SetColor("_EmissionColor", Color.black); // Turn off glow
    }
}
