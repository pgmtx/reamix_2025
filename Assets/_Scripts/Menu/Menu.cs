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
    private Light light;
    private bool updateLight = false;

    [SerializeField] private GameObject menu;

    [SerializeField] private GameObject barriere;
    private XRGrabInteractable barriereInteractable;

    [SerializeField] private GameObject spotlight;

    [SerializeField] private GameObject deplacement;

    void Awake()
    {
        // Bloquer les deplacements du joueur
        deplacement.SetActive(false);

        // Pas d'interactions avec la barrière du babypark
        barriereInteractable = barriere.GetComponent<XRGrabInteractable>();
        barriereInteractable.enabled = false;

        // Eclairage eteint
        // Valeur defaut du planetarium -> light.intensity = 6;
        light = eclairagePlanetarium.GetComponent<Light>();
        light.intensity = 0;
        spotlight.SetActive(true);

        // Mettre l'interaction sur le bouton Jouer
        jouerInteractable = boutonJouer.GetComponent<XRSimpleInteractable>();
        jouerInteractable.selectEntered.AddListener(OnPressedJouer);

        // Mettre l'interaction sur le bouton Quitter
        quitterInteractable = boutonQuitter.GetComponent<XRSimpleInteractable>();
        quitterInteractable.selectEntered.AddListener(OnPressedQuitter);
    }

    void Update()
    {
        if (updateLight)
        {
            if (light.intensity < 6)
            {
                light.intensity += Time.deltaTime;
            }
            else
            {
                menu.SetActive(false);
            }
        }
    }

    void OnPressedJouer(SelectEnterEventArgs args)
    {
        Debug.Log("Le bouton Jouer a ete presse !");

        //allumerLumiere();
        updateLight = true;

        // On desac le menu et remet les valeurs a defaut
        boutonJouer.SetActive(false);
        boutonQuitter.SetActive(false);
        spotlight.SetActive(false);
        barriereInteractable.enabled = true;
        deplacement.SetActive(true);
    }

    void OnPressedQuitter(SelectEnterEventArgs args)
    {
        Debug.Log("Le jeu se ferme !!!!!");
        Application.Quit();
    }
}
