using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Balle : MonoBehaviour
{
    public bool isAlreadyInBasket = false;
    public GameEvent BallPickedUp;
    bool ballPickedUpTriggered;

    void Update()
    {
        if (ballPickedUpTriggered)
        {
            return;
        }

        if (GetComponent<XRGrabInteractable>().isSelected)
        {
            BallPickedUp.TriggerEvent();
        }
    }
}
