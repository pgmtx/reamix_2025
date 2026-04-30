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
    [SerializeField] private float minImpactSpeedToPlaySound;
    private bool soundEnabled = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        // Déclenché UNIQUEMENT la frame du pickup
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

        if (!grabInteractable.isSelected && impactSpeed > minImpactSpeedToPlaySound)
        {
            Debug.Log("Impact réel à vitesse: " + impactSpeed);
            Debug.Log("Son joué volume: " + Mathf.Clamp(impactSpeed / 6, 0.05f, 0.95f));
            AudioSystem.Instance.Play3DSoundRdmPitch("ball hit ground", transform.position, Mathf.Clamp(impactSpeed / 6, 0.05f, 0.95f));
        }
    }
}