using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
public class FootstepAudio : MonoBehaviour
{
    [Header("Footstep Settings")]
    private AudioSource footstepAudio;
    private float footstepPitch;
    private float footstepVolume;
    public AudioClip[] footstepSounds;
    public float distanceThreshold = 1.5f; // Distance avant de jouer le bruit de pas

    private CharacterController characterController;
    private Vector2 lastPosition2D;        // position XZ
    private float distanceTraveled = 0f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        footstepAudio = GetComponent<AudioSource>();
        footstepPitch = footstepAudio.pitch;
        footstepVolume = footstepAudio.volume;
        Vector3 startPos = transform.position;
        lastPosition2D = new Vector2(startPos.x, startPos.z);
    }

    void FixedUpdate()
    {
        // Position actuelle en XZ
        Vector3 currentPos3D = transform.position;
        Vector2 currentPos2D = new Vector2(currentPos3D.x, currentPos3D.z);

        // Calcul du déplacement horizontal
        Vector2 displacement2D = currentPos2D - lastPosition2D;

        // Si le joueur ne bouge pas, reset distance
        if (displacement2D.sqrMagnitude < 0.0001f)
        {
            distanceTraveled = 0f;
            lastPosition2D = currentPos2D;
            return;
        }

        // Accumule la distance
        distanceTraveled += displacement2D.magnitude;

        // Jouer le bruit de pas si seuil atteint
        if (distanceTraveled >= distanceThreshold)
        {
            PlayFootstepSound();

            distanceTraveled = 0f; // reset
        }

        lastPosition2D = currentPos2D;
    }

    void PlayFootstepSound()
    {
        // Randomize
        footstepAudio.clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        AudioSystem.Instance.PlayRdmPitchVol(footstepAudio);
    }
}


