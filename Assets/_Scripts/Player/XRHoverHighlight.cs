using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRHoverHighlight : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Material material;
    private Color originalEmission;
    private Color hoverEmission = Color.cyan;

    private bool selected = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        material = GetComponent<Renderer>().material;
        
        originalEmission = material.GetColor("_EmissionColor");

        grabInteractable.hoverEntered.AddListener(OnHoverEnter);
        grabInteractable.hoverExited.AddListener(OnHoverExit);
        grabInteractable.selectEntered.AddListener(OnSelectEnter);
        grabInteractable.selectExited.AddListener(OnSelectExit);
    }

    void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (selected) return;
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", hoverEmission);
    }

    void OnHoverExit(HoverExitEventArgs args)
    {
        material.SetColor("_EmissionColor", originalEmission);
    }

    void OnSelectEnter(SelectEnterEventArgs args)
    {
        material.SetColor("_EmissionColor", originalEmission);
        selected = true;
    }

    void OnSelectExit(SelectExitEventArgs args)
    {
        selected = false;
    }
}
