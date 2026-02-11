using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
public class Balle : MonoBehaviour
{
    public bool isAlreadyInBasket = false;
    public GameEvent BallPickedUp;

    XRGrabInteractable grabInteractable;
    bool wasSelectedLastFrame = false;

    [Header("Son")]
    [SerializeField] private AudioSource ballHitSound;
    private float ballHitSoundVolume;
    private float ballHitSoundPitch;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        ballHitSound = GetComponent<AudioSource>();
        ballHitSoundVolume = ballHitSound.volume;
        ballHitSoundPitch = ballHitSound.pitch;
        // Disable on spawn pour pas avoir de bruit dans le tuyau
        ballHitSound.enabled = false;
    }

    void Update()
    {
        // Déclenché UNIQUEMENT la frame du pickup
        if (!wasSelectedLastFrame && grabInteractable.isSelected)
        {
            ballHitSound.enabled = true;
            BallPickedUp.TriggerEvent();
        }
        wasSelectedLastFrame = grabInteractable.isSelected;
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactSpeed = collision.relativeVelocity.magnitude;

        if (!grabInteractable.isSelected && impactSpeed > 1.6f)
        {
            Debug.Log("Impact réel à vitesse: " + impactSpeed);
            ballHitSound.pitch = ballHitSoundPitch + Random.Range(-0.005f, 0.005f);
            ballHitSound.volume = Mathf.Clamp(impactSpeed / 8, 0.15f, 0.85f);
            ballHitSound.Play();
        }
    }

    
}
