 using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Balles : MonoBehaviour
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