using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject boutonJouer;
    private XRSimpleInteractable jouerInteractable;

    [SerializeField] private GameObject boutonQuitter;
    private XRSimpleInteractable quitterInteractable;

    [SerializeField] private GameObject eclairagePlanetarium;
    private Light lightCompPlanetarium;
    private bool updateLight = false;

    [SerializeField] private GameObject menu;

    [SerializeField] private GameObject barriere;
    private XRGrabInteractable barriereInteractable;

    [SerializeField] private GameObject spotlight;
    private Light lightCompSpotlight;

    [SerializeField] private GameObject deplacement;

    void Awake()
    {
        // Bloquer les deplacements du joueur
        deplacement.SetActive(false);

        // Pas d'interactions avec la barrière du babypark
        barriereInteractable = barriere.GetComponent<XRGrabInteractable>();
        barriereInteractable.enabled = false;

        // Eclairage eteint
        // Valeur defaut du planetarium -> lightCompPlanetarium.intensity = 6;
        lightCompPlanetarium = eclairagePlanetarium.GetComponent<Light>();
        lightCompPlanetarium.intensity = 0;

        // Valeur defaut du spotlight -> lightCompSpotlight.intensity = 8;
        lightCompSpotlight = spotlight.GetComponent<Light>();
        spotlight.SetActive(true);

        // Mettre l'interaction sur le bouton Jouer
        jouerInteractable = boutonJouer.GetComponent<XRSimpleInteractable>();
        jouerInteractable?.selectEntered.AddListener(OnPressedJouer);

        // Mettre l'interaction sur le bouton Quitter
        quitterInteractable = boutonQuitter.GetComponent<XRSimpleInteractable>();
        quitterInteractable?.selectEntered.AddListener(OnPressedQuitter);
    }

    void Update()
    {
        if (updateLight)
        {
            if (lightCompPlanetarium.intensity < 6)
            {
                lightCompPlanetarium.intensity += Time.deltaTime;
            }

            if (lightCompSpotlight.intensity > 0)
            { 
                lightCompSpotlight.intensity -= Time.deltaTime;
            }

            if (lightCompPlanetarium.intensity >= 6 && lightCompSpotlight.intensity <= 0)
            {
                Debug.Log("Menu desactive !");
                menu.SetActive(false);
            }
        }
    }

    void OnPressedJouer(SelectEnterEventArgs args)
    {
        Debug.Log("Le bouton Jouer a ete presse !");

        updateLight = true;

        // On desac les boutons et remet a defaut
        boutonJouer.SetActive(false);
        boutonQuitter.SetActive(false);
        barriereInteractable.enabled = true;
        deplacement.SetActive(true);
    }

    void OnPressedQuitter(SelectEnterEventArgs args)
    {
        Debug.Log("Le jeu se ferme !!!!!");
        Application.Quit();
    }
}
