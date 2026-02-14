using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInteractable : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Material material;
    private Renderer renderer;
    private Color originalEmission;
    private Color hoverEmission = Color.cyan;

    private bool selected = false;

    [SerializeField] private AudioSource pickupSound;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Recuperation du renderer ou le premier trouvé chez les enfants
        renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>();
            if (renderer == null)
            {
                Debug.Log("Material non trouvé !!!!!!!!!");
                this.enabled = false;
            }
        }
        material = renderer.material;
        
        originalEmission = material.GetColor("_Color");

        grabInteractable.hoverEntered.AddListener(OnHoverEnter);
        grabInteractable.hoverExited.AddListener(OnHoverExit);
        grabInteractable.selectEntered.AddListener(OnSelectEnter);
        grabInteractable.selectExited.AddListener(OnSelectExit);
    }

    void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (selected) return;
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
        material.DisableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", originalEmission);
        selected = true;

        AudioSystem.Instance.PlayRdmPitchVol(pickupSound);
    }

    void OnSelectExit(SelectExitEventArgs args)
    {
        selected = false;
    }
}
