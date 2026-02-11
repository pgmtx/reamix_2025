using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Jouer : MonoBehaviour
{
    private XRSimpleInteractable interactable;

    [SerializeField]
    private GameObject eclairage;

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject barriere;
    private XRGrabInteractable barriereInteractable;

    [SerializeField]
    private GameObject spotlight;

    [SerializeField]
    private GameObject deplacement;

    void Awake()
    {
        // Bloquer les deplacements du joueur
        deplacement.SetActive(false);

        // Pas d'interactions avec la barrière du babypark
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
        deplacement.SetActive(true);
    }
}
