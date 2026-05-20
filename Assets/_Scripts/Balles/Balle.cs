using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Balle : MonoBehaviour
{
    public bool isAlreadyInBasket = false;
    public GameEvent BallPickedUp;

    XRGrabInteractable grabInteractable;
    bool wasSelectedLastFrame = false;

    [Header("Son")]
    private bool soundEnabled = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        // D�clench� UNIQUEMENT la frame du pickup
        if (!wasSelectedLastFrame && grabInteractable.isSelected)
        {
            soundEnabled = true;
            BallPickedUp.TriggerEvent();
        }
        wasSelectedLastFrame = grabInteractable.isSelected;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!soundEnabled)
            return;

        float impactSpeed = collision.relativeVelocity.magnitude;

        if (!grabInteractable.isSelected)
        {
            float volume = 0.7f * (1 - Mathf.Exp(-impactSpeed / 6));
            Debug.Log("Impact r�el � vitesse: " + impactSpeed);
            Debug.Log("Son jou� volume: " + volume);
            AudioSystem.Instance.Play3DSoundRdmPitch("ball hit ground", transform.position, volume);
        }
    }
}