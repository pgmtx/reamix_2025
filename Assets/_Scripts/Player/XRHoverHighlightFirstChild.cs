using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRHoverHighlightFirstChild : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Material material;
    private Color originalEmission;
    private Color hoverEmission = Color.cyan;

    private bool selected = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        material = GetComponentInChildren<Renderer>().material;

        originalEmission = material.GetColor("_Color");

        grabInteractable.hoverEntered.AddListener(OnHoverEnter);
        grabInteractable.hoverExited.AddListener(OnHoverExit);
        grabInteractable.selectEntered.AddListener(OnSelectEnter);
        grabInteractable.selectExited.AddListener(OnSelectExit);

        Debug.Log("Emission" + originalEmission);
    }

    void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (selected) return;
        Debug.Log("Hovering an object");
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", originalEmission * 2f);
    }

    void OnHoverExit(HoverExitEventArgs args)
    {
        material.DisableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", originalEmission);
    }

    void OnSelectEnter(SelectEnterEventArgs args)
    {
        Debug.Log("Selected an object");
        material.DisableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", originalEmission);
        selected = true;
    }

    void OnSelectExit(SelectExitEventArgs args)
    {
        selected = false;
    }
}
