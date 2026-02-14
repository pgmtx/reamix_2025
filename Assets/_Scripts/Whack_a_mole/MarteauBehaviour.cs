using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class MarteauBehaviour : MonoBehaviour
{
    [SerializeField] private GameEvent marteauPickedUp;
    private XRGrabInteractable xrGrabInteractable;
    private void Awake()
    {
        xrGrabInteractable = GetComponent<XRGrabInteractable>();

        xrGrabInteractable.selectEntered.AddListener(OnSelectEnter);
    }

    void OnSelectEnter(SelectEnterEventArgs args)
    {
        marteauPickedUp.TriggerEvent();
    }
}
