using UnityEngine;

public class TableTrigger : MonoBehaviour
{
    // Drag your GameManager object into this slot in the Inspector
    public BlackjackManager gameManagerr;

    // This runs automatically when something enters the cube
    private void OnTriggerEnter(Collider other)
    {
        // Check if the thing that entered is the Player (usually tagged "MainCamera" or "Player")
        if (other.CompareTag("MainCamera") || other.CompareTag("Player"))
        {
            Debug.Log("Player at table! Starting game...");
            gameManagerr.StartNewGame();
        }
    }
}