using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BlackjackManager : MonoBehaviour
{
    public List<GameObject> deckPrefabs; 
    private List<GameObject> cardsOnTable = new List<GameObject>();
    private List<GameObject> shuffledDeck = new List<GameObject>();
    
    // Dealer's internal score tracking
    private List<int> dealerHandValues = new List<int>();

    [Header("Spawn Points")]
    public Transform playerSlot1; public Transform playerSlot2;
    public Transform playerSlot3; public Transform playerSlot4;
    public Transform dealerSlot1; public Transform dealerSlot2;
    public Transform dealerSlot3; public Transform dealerSlot4;

    public void StartNewGame()
    {
        StopAllCoroutines();
        foreach (GameObject card in cardsOnTable) Destroy(card);
        cardsOnTable.Clear();
        dealerHandValues.Clear();
        
        shuffledDeck = new List<GameObject>(deckPrefabs);
        for (int i = 0; i < shuffledDeck.Count; i++) {
            GameObject temp = shuffledDeck[i];
            int r = Random.Range(i, shuffledDeck.Count);
            shuffledDeck[i] = shuffledDeck[r];
            shuffledDeck[r] = temp;
        }
        StartCoroutine(DealSequence());
    }

    IEnumerator DealSequence()
    {
        // 1. Initial Deal (2 each)
        yield return Spawn(playerSlot1, false); yield return new WaitForSeconds(0.4f);
        yield return Spawn(dealerSlot1, true);  yield return new WaitForSeconds(0.4f);
        yield return Spawn(playerSlot2, false); yield return new WaitForSeconds(0.4f);
        yield return Spawn(dealerSlot2, true);  yield return new WaitForSeconds(0.4f);
        
        yield return new WaitForSeconds(0.5f);
        
        // 2. Reveal all initial cards
        foreach (GameObject c in cardsOnTable) c.GetComponent<BlackjackCard>().Reveal();
        yield return new WaitForSeconds(1.0f);

        // 3. Dealer Logic: Hit until 17
        // Check slot 3
        if (GetHandTotal(dealerHandValues) < 17) {
            yield return Spawn(dealerSlot3, true);
            cardsOnTable[cardsOnTable.Count-1].GetComponent<BlackjackCard>().Reveal();
            yield return new WaitForSeconds(1.0f);
        }

        // Check slot 4
        if (GetHandTotal(dealerHandValues) < 17) {
            yield return Spawn(dealerSlot4, true);
            cardsOnTable[cardsOnTable.Count-1].GetComponent<BlackjackCard>().Reveal();
        }

        Debug.Log("Dealer Final Score: " + GetHandTotal(dealerHandValues));
    }

    IEnumerator Spawn(Transform slot, bool isDealer)
    {
        GameObject prefab = shuffledDeck[0];
        shuffledDeck.RemoveAt(0);
        
        GameObject newCard = Instantiate(prefab, slot.position, slot.rotation);
        cardsOnTable.Add(newCard);

        if (isDealer) {
            dealerHandValues.Add(newCard.GetComponent<BlackjackCard>().cardValue);
        }
        yield return null;
    }

    int GetHandTotal(List<int> values)
    {
        int total = 0;
        int aces = 0;
        foreach (int v in values) {
            if (v == 11) aces++;
            total += v;
        }
        while (total > 21 && aces > 0) {
            total -= 10;
            aces--;
        }
        return total;
    }
}