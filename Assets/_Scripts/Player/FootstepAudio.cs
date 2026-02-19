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
    [SerializeField] private Transform playerPos;

    [SerializeField] private CharacterController characterController;
    private Vector2 lastPosition2D;        // position XZ
    private float distanceTraveled = 0f;

    void Awake()
    {
        footstepAudio = GetComponent<AudioSource>();
        footstepPitch = footstepAudio.pitch;
        footstepVolume = footstepAudio.volume;
        Vector3 startPos = playerPos.position;
        lastPosition2D = new Vector2(startPos.x, startPos.z);
    }

    void Update()
    {
        // Position actuelle en XZ
        Vector3 currentPos3D = playerPos.position;
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
        Debug.Log("Play step sound");
        // Randomize
        footstepAudio.clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        footstepAudio.pitch = footstepPitch + Random.Range(-0.005f, 0.005f);
        footstepAudio.volume = footstepVolume + Random.Range(-0.005f, 0.005f);
        footstepAudio.Play();
    }
}


