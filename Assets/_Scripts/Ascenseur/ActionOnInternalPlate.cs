using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ActionOnInternalPlate : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 1f;

    [SerializeField]
    private string nextSceneName = "Salle2";

    [SerializeField]
    private Animator elevatorDoorAnimator;

    public void StartTransition()
    {
<<<<<<< HEAD
        AudioSystem.Instance.Play3DSoundRdmPitchVol("bell", transform.position);
=======
>>>>>>> e6cafb3cb7e2ec410d01009f934d0fe55c8114bc
        elevatorDoorAnimator.ResetTrigger("openDoors");
        elevatorDoorAnimator.SetFloat("speed", -1);
        elevatorDoorAnimator.SetTrigger("openDoors");

<<<<<<< HEAD
=======
        GetComponent<AudioSource>().Play();
>>>>>>> e6cafb3cb7e2ec410d01009f934d0fe55c8114bc
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(nextSceneName);
    }
}
