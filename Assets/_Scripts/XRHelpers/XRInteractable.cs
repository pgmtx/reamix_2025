using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInteractable : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Material material;
    private Renderer renderer;
    private Color originalEmission;
    private Color hoverEmission = Color.cyan;

    // Pour le respawn quand ca sort des bounds
    private Vector3 boundsOrigin = new Vector3(0, 2.45f, 13);
    private Vector3 boundsSize = new Vector3(19, 9, 40);
    private Bounds bounds;
    private Vector3 spawnPoint;

    private bool selected = false;

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

        // Respawn si out of bounds
        bounds = new Bounds(boundsOrigin, boundsSize);
        spawnPoint = transform.position;
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

        AudioSystem.Instance.Play3DSoundRdmPitchVol("object picked up", transform.position);
    }

    void OnSelectExit(SelectExitEventArgs args)
    {
        selected = false;

        if (!bounds.Contains(transform.position))
        {
            Debug.Log("Objet hors des limites !");
            transform.position = spawnPoint;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireCube(boundsOrigin, boundsSize);
    }
}