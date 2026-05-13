using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ActionOnExternalPlate : MonoBehaviour
{
    [SerializeField]
    private Animator elevatorDoorAnimator;

    public void StartTransition()
    {
        elevatorDoorAnimator.SetTrigger("openDoors");
        GetComponent<AudioSource>().Play();
    }
}
