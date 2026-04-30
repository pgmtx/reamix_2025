using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BlackjackManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startButton; // This will now appear in your Inspector

    [Header("Card Setup")]
    public List<GameObject> deckPrefabs;
    private List<GameObject> cardsOnTable = new List<GameObject>();
    private List<GameObject> shuffledDeck = new List<GameObject>();

    private List<int> dealerHandValues = new List<int>();
    private List<int> playerHandValues = new List<int>();

    [Header("Table Spawn Points")]
    public Transform playerSlot1; public Transform playerSlot2;
    public Transform playerSlot3; public Transform playerSlot4;
    public Transform dealerSlot1; public Transform dealerSlot2;
    public Transform dealerSlot3; public Transform dealerSlot4;

    [Header("Game Settings")]
    public float dealSpeed = 0.4f;
    public float waitBeforeFlip = 1.0f;

    public void StartNewGame()
    {
        Debug.Log("StartNewGame called");

        // Safety Check: Make sure we have cards and slots assigned
        if (deckPrefabs.Count == 0 || playerSlot1 == null)
        {
            Debug.LogError("Missing Prefabs or Slots in the GameManager Inspector!");
            return;
        }

        // 1. Hide the Start Button
        if (startButton != null)
        {
            startButton.SetActive(false);
        }

        // 2. Reset the table
        StopAllCoroutines();
        foreach (GameObject card in cardsOnTable) Destroy(card);
        cardsOnTable.Clear();
        dealerHandValues.Clear();
        playerHandValues.Clear();

        // 3. Shuffle and Deal
        ShuffleDeck();
        StartCoroutine(DealSequence());
    }

    IEnumerator DealSequence()
    {
        // Deal 2 to player, 2 to dealer
        yield return Spawn(playerSlot1, false); yield return new WaitForSeconds(dealSpeed);
        yield return Spawn(dealerSlot1, true); yield return new WaitForSeconds(dealSpeed);
        yield return Spawn(playerSlot2, false); yield return new WaitForSeconds(dealSpeed);
        yield return Spawn(dealerSlot2, true); yield return new WaitForSeconds(dealSpeed);

        yield return new WaitForSeconds(waitBeforeFlip);

        // Reveal initial cards
        foreach (GameObject c in cardsOnTable)
        {
            if (c.TryGetComponent(out BlackjackCard cardScript))
                cardScript.Reveal();
        }

        // Dealer Turn: Hit until 17
        while (GetHandTotal(dealerHandValues) < 17 && dealerHandValues.Count < 4)
        {
            Transform nextSlot = (dealerHandValues.Count == 2) ? dealerSlot3 : dealerSlot4;
            yield return Spawn(nextSlot, true);
            cardsOnTable[cardsOnTable.Count - 1].GetComponent<BlackjackCard>().Reveal();
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator Spawn(Transform slot, bool isDealer)
    {
        if (shuffledDeck.Count > 0)
        {
            GameObject prefab = shuffledDeck[0];
            shuffledDeck.RemoveAt(0);

            GameObject newCard = Instantiate(prefab, slot.position, slot.rotation);
            cardsOnTable.Add(newCard);

            if (isDealer) dealerHandValues.Add(newCard.GetComponent<BlackjackCard>().cardValue);
            else playerHandValues.Add(newCard.GetComponent<BlackjackCard>().cardValue);
        }
        yield return null;
    }

    void ShuffleDeck()
    {
        shuffledDeck = new List<GameObject>(deckPrefabs);
        for (int i = 0; i < shuffledDeck.Count; i++)
        {
            GameObject temp = shuffledDeck[i];
            int r = Random.Range(i, shuffledDeck.Count);
            shuffledDeck[i] = shuffledDeck[r];
            shuffledDeck[r] = temp;
        }
    }

    int GetHandTotal(List<int> values)
    {
        int total = 0;
        int aces = 0;
        foreach (int v in values)
        {
            if (v == 11) aces++;
            total += v;
        }
        while (total > 21 && aces > 0)
        {
            total -= 10;
            aces--;
        }
        return total;
    }
}