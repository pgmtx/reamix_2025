using System.Collections;
using UnityEngine;

public class Whack_a_mole : MonoBehaviour
{
    private Animator m_Animator;

    void Start()
    {
        m_Animator = GetComponent<Animator>();

        if (m_Animator != null)
        {
            StartCoroutine(MoleLoop());
        }
    }

    IEnumerator MoleLoop()
    {
        while (true)
        {
            // Monter la taupe
            m_Animator.SetTrigger("up");

            // Temps visible (1 à 3 secondes)
            float waitTime = Random.Range(1f, 6f);
            yield return new WaitForSeconds(waitTime);

            // Descendre la taupe
            m_Animator.SetTrigger("down");

            // Temps caché avant de remonter
            yield return new WaitForSeconds(3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
