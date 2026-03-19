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
            AudioSystem.Instance.Play3DSoundRdmPitchVol("ball hit ground", transform.position, Mathf.Clamp(impactSpeed / 10, 0.15f, 0.85f), Random.Range(-0.005f, 0.005f));
        }
    }
}