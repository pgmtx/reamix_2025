using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ActionOnExternalPlate : MonoBehaviour
{
    [SerializeField]
    private Animator elevatorDoorAnimator;

    public void StartTransition()
    {
<<<<<<< HEAD
        AudioSystem.Instance.Play3DSoundRdmPitchVol("bell", transform.position);
        elevatorDoorAnimator.SetTrigger("openDoors");
=======
        elevatorDoorAnimator.SetTrigger("openDoors");
        GetComponent<AudioSource>().Play();
>>>>>>> e6cafb3cb7e2ec410d01009f934d0fe55c8114bc
    }
}
