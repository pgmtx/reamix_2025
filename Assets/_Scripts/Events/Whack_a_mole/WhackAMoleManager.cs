using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WhackAMoleManager : MonoBehaviour
{
    private Taupe[] taupes;
    private Taupe currentTaupe;
    public int score = 0;
    private bool win = false;

    void Start()
    {
        taupes = FindObjectsOfType<Taupe>();
        StartCoroutine(MoleLoop());
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

    private void Update()
    {
        if (score == 5 && !win)
        {
            for (int i = 0; i < taupes.Length; i++)
            {
                taupes[i].GoUp();
            }
            Debug.Log("Bien joué tu as gagné !");
            win = true;
            return;
        }
    }
    public void OnTaupeHit(Taupe taupe)
    {
        if (taupe != currentTaupe || score >= 5 || win) return;

        score++;
        Debug.Log("Score : " + score);

        currentTaupe.GoDown();
    }
}
