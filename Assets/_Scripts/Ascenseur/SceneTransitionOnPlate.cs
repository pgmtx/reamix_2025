using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransitionOnPlate : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 1f;

    [SerializeField]
    private string nextSceneName = "Salle2";

    public void StartTransition()
    {
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        // Fade out
        var elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            //fadeCanvas.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
