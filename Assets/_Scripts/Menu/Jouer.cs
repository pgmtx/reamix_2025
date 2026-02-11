using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Jouer : MonoBehaviour
{
    private XRSimpleInteractable interactable;

    public GameObject eclairage;

    public GameObject menu;

    public GameObject barriere;
    private XRGrabInteractable barriereInteractable;

    public GameObject spotlight;

    void Awake()
    {
        // On desactive les interactions avec la barriere
        barriereInteractable = barriere.GetComponent<XRGrabInteractable>();
        barriereInteractable.enabled = false;

        // Eclairage eteint
        eclairage.SetActive(false);
        spotlight.SetActive(true);

        // Mettre l'interaction sur le bouton Jouer
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnPressed);
    }

    void OnPressed(SelectEnterEventArgs args)
    {
        Debug.Log("Le bouton Jouer a ete presse !");

        // On desac le menu et remet les valeurs a defaut
        menu.SetActive(false);
        eclairage.SetActive(true);
        spotlight.SetActive(false);
        barriereInteractable.enabled = true;
    }
}
