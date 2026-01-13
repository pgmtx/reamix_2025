using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WhackAMoleManager : MonoBehaviour
{
    private Taupe[] taupes;
    private Taupe currentTaupe;
    public static int Score = 0;
    private bool win = false;

    public GameEvent TaupeFrappe;

    void Start()
    {
        taupes = FindObjectsOfType<Taupe>();
        StartCoroutine(MoleLoop());
    }

    private void EndGame()
    {
        win = true;
        StopAllCoroutines();

        for (int i = 0; i < taupes.Length; i++)
        {
            taupes[i].ShowUpFinal();
            taupes[i].DisableTaupe();
        }
    }


    IEnumerator MoleLoop()
    {
        while (!win)
        {
            // Choisir une taupe
            currentTaupe = taupes[Random.Range(0, taupes.Length)];
            
            float timeUp = Random.Range(1f, 2f);
            yield return new WaitForSeconds(timeUp);

            currentTaupe.GoUp();

            timeUp = Random.Range(1f, 3f);
            yield return new WaitForSeconds(timeUp);

            currentTaupe.GoDown();

            yield return new WaitForSeconds(3f);
        }

    }

    public void OnTaupeHit(Taupe taupe)
    {
        if (taupe != currentTaupe || Score >= 5 || win) return;
        
        Score++;
        Debug.Log("Score : " + Score);
        
        StopAllCoroutines();
        
        TaupeFrappe.TriggerEvent();
        if (Score >= 5)
        {
            EndGame();
            return;
        }
        currentTaupe.GoDown();
        StartCoroutine(MoleLoop());
    }
}
