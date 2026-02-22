using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BlackjackManager : MonoBehaviour
{
    [Header("Card Setup")]
    // Drag all 52 card prefabs into this list in the Inspector
    public List<GameObject> deckPrefabs; 
    
    // This list will hold the cards currently sitting on the table
    private List<GameObject> cardsOnTable = new List<GameObject>();
    private List<GameObject> shuffledDeck = new List<GameObject>();

    [Header("Table Spawn Points")]
    public Transform playerSlot1;
    public Transform playerSlot2;
    public Transform dealerSlot1;
    public Transform dealerSlot2;

    [Header("Game Settings")]
    public float dealSpeed = 0.5f;     // Time between each card appearing
    public float waitBeforeFlip = 1.0f; // Pause before cards reveal themselves

    // --- CALL THIS FUNCTION FROM YOUR VR BUTTON ---
    public void StartNewGame()
    {
        StopAllCoroutines(); // Stop any current dealing
        ClearTable();
        ShuffleDeck();
        StartCoroutine(DealSequence());
    }

    private void ClearTable()
    {
        foreach (GameObject card in cardsOnTable)
        {
            Destroy(card);
        }
        cardsOnTable.Clear();
    }

    private void ShuffleDeck()
    {
        shuffledDeck = new List<GameObject>(deckPrefabs);
        for (int i = 0; i < shuffledDeck.Count; i++)
        {
            GameObject temp = shuffledDeck[i];
            int randomIndex = Random.Range(i, shuffledDeck.Count);
            shuffledDeck[i] = shuffledDeck[randomIndex];
            shuffledDeck[randomIndex] = temp;
        }
    }

    IEnumerator DealSequence()
    {
        // 1. Deal cards in order (Player, Dealer, Player, Dealer)
        yield return StartCoroutine(SpawnCardAtSlot(playerSlot1));
        yield return new WaitForSeconds(dealSpeed);
        
        yield return StartCoroutine(SpawnCardAtSlot(dealerSlot1));
        yield return new WaitForSeconds(dealSpeed);
        
        yield return StartCoroutine(SpawnCardAtSlot(playerSlot2));
        yield return new WaitForSeconds(dealSpeed);
        
        yield return StartCoroutine(SpawnCardAtSlot(dealerSlot2));

        // 2. Wait for a dramatic pause
        yield return new WaitForSeconds(waitBeforeFlip);

        // 3. Flip all cards over
        foreach (GameObject card in cardsOnTable)
        {
            if (card.TryGetComponent(out BlackjackCard flipScript))
            {
                flipScript.Reveal();
            }
        }
    }

    IEnumerator SpawnCardAtSlot(Transform slot)
    {
        if (shuffledDeck.Count > 0)
        {
            GameObject cardToSpawn = shuffledDeck[0];
            shuffledDeck.RemoveAt(0);

            // Spawn the card using the Slot's position and rotation
            GameObject newCard = Instantiate(cardToSpawn, slot.position, slot.rotation);
            cardsOnTable.Add(newCard);
        }
        yield return null;
    }
}