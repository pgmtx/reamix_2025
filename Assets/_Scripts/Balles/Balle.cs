using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Balle : MonoBehaviour
{
    public bool isAlreadyInBasket = false;
    public GameEvent BallPickedUp;

    XRGrabInteractable grabInteractable;
    bool wasSelectedLastFrame = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        bool isSelectedNow = grabInteractable.isSelected;

        // Déclenché UNIQUEMENT la frame du pickup
        if (!wasSelectedLastFrame && isSelectedNow)
        {
            BallPickedUp.TriggerEvent();
        }

        wasSelectedLastFrame = isSelectedNow;
    }
}
