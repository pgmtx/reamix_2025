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

    private void Update()
    {
        if (Score == 5 && !win)
        {
            for (int i = 0; i < taupes.Length; i++)
            {
                taupes[i].GoUp();
            }
            Debug.Log("Bien joué tu as gagné !");
            win = true;
        }
    }

    IEnumerator MoleLoop()
    {
        while (!win)
        {
            // Choisir une taupe
            currentTaupe = taupes[Random.Range(0, taupes.Length)];

            currentTaupe.GoUp();

            float timeUp = Random.Range(1f, 6f);
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

        currentTaupe.GoDown();
        TaupeFrappe.TriggerEvent();
    }
}
