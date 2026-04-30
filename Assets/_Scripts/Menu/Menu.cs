using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Menu : MonoBehaviour
{
    [Header("Menu demarrage (statique)")]
    [SerializeField] private GameObject menuPrincipal;

    [SerializeField] private GameObject boutonJouer;
    private XRSimpleInteractable jouerInteractable;

    [SerializeField] private GameObject boutonQuitter;
    private XRSimpleInteractable quitterInteractable;


    [Header("Menu bouton main")]
    [SerializeField] private GameObject menuMain;
    private InputAction boutonA;

    [SerializeField] private GameObject boutonReprendreMain;
    private XRSimpleInteractable reprendreInteractableMain;

    [SerializeField] private GameObject boutonQuitterMain;
    private XRSimpleInteractable quitterInteractableMain;


    [Header("GameObjects pour le Menu Demarrage")]
    [SerializeField] private GameObject poigneeBarriere;
    private XRGrabInteractable poigneeBarriereInteractable;

    [SerializeField] private GameObject spotlight;
    private Light lightCompSpotlight;


    [Header("GameObjects communs")]
    [SerializeField] private GameObject deplacement;

    [SerializeField] private GameObject rotation;

    [SerializeField] private XRRayInteractor rayInteractorGauche;
    [SerializeField] private XRRayInteractor rayInteractorDroite;

    [SerializeField] private GameObject eclairagePlanetarium;
    private Light lightCompPlanetarium;
    private bool restoreLights = false;
    private bool shutLights = false;

    void Awake()
    {
        InputAction triggerAction = new InputAction("Trigger", InputActionType.Button);

        //triggerAction.AddBinding("<XRController>{RightHand}/triggerPressed");
        //triggerAction.AddBinding("<XRController>{LeftHand}/triggerPressed");
        triggerAction.AddBinding("<XRController>/triggerPressed");

        triggerAction.performed += OnTriggerPressed;
        triggerAction.Enable();

        // Bloquer les deplacements et la rotation du joueur
        deplacement.SetActive(false);
        rotation.SetActive(false);

        // Pas d'interactions avec la barrière du babypark
        poigneeBarriereInteractable = poigneeBarriere.GetComponent<XRGrabInteractable>();
        poigneeBarriereInteractable.enabled = false;

        // Eclairage eteint
        // Valeur defaut du planetarium -> lightCompPlanetarium.intensity = 6;
        lightCompPlanetarium = eclairagePlanetarium.GetComponent<Light>();
        lightCompPlanetarium.intensity = 0;

        // Valeur defaut du spotlight -> lightCompSpotlight.intensity = 8;
        lightCompSpotlight = spotlight.GetComponent<Light>();
        spotlight.SetActive(true);

        // Mettre l'interaction sur le bouton Jouer
        jouerInteractable = boutonJouer.GetComponent<XRSimpleInteractable>();
        jouerInteractable?.activated.AddListener(OnPressedJouer);

        // Mettre l'interaction sur le bouton Quitter
        quitterInteractable = boutonQuitter.GetComponent<XRSimpleInteractable>();
        quitterInteractable?.activated.AddListener(OnPressedQuitter);

        // Mettre l'interaction sur le bouton Reprendre
        reprendreInteractableMain = boutonReprendreMain.GetComponent<XRSimpleInteractable>();
        reprendreInteractableMain?.activated.AddListener(OnPressedReprendre);

        // Mettre l'interaction sur le bouton Quitter du menu Main
        quitterInteractableMain = boutonQuitterMain.GetComponent<XRSimpleInteractable>();
        quitterInteractableMain?.activated.AddListener(OnPressedQuitter);

        // Setup du bouton de pause ptetre mettre sur la manette gauche
        boutonA = new InputAction(
            "BoutonA",
            InputActionType.Button,
            "<XRController>{RightHand}/primaryButton"
        );

        boutonA.performed += OnBoutonAPressed;
        boutonA.Enable();
    }

    void Update()
    {
        if (restoreLights)
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
                // Uniquement pour la première fois -> Desac menu principale
                if (menuPrincipal.activeSelf)
                {
                    Debug.Log("Menu desactive !");
                    menuPrincipal.SetActive(false);
                }
                
                // Desac le spotlight et stop màj lumières
                Debug.Log("Spotlight desactive !");
                spotlight.SetActive(false);
                restoreLights = false;
            }
        }
    }

    private void OnTriggerPressed(InputAction.CallbackContext ctx)
    {
        CheckRay(rayInteractorGauche);
        CheckRay(rayInteractorDroite);
    }

    void CheckRay(XRRayInteractor ray)
    {
        if (ray.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            var interactable = hit.collider.GetComponent<XRSimpleInteractable>();

            if (interactable == jouerInteractable)
                OnPressedJouer(null);

            if (interactable == quitterInteractable || interactable == quitterInteractableMain)
                OnPressedQuitter(null);

            if (interactable == reprendreInteractableMain) 
                OnPressedReprendre(null);
        }
    }

    void OnPressedJouer(ActivateEventArgs args)
    {
        Debug.Log("Le bouton Jouer a ete presse !");

        restoreLights = true;

        // On desac les boutons et remet a defaut
        boutonJouer.SetActive(false);
        boutonQuitter.SetActive(false);
        poigneeBarriereInteractable.enabled = true;
        deplacement.SetActive(true);
        rotation.SetActive(true);
    }

    void OnPressedQuitter(ActivateEventArgs args)
    {
        Debug.Log("Le jeu se ferme !!!!!");
        Application.Quit();
    }

    void OnPressedReprendre(ActivateEventArgs args)
    {
        Debug.Log("Le bouton Reprendre a ete presse !");

        restoreLights = true;

        deplacement.SetActive(true);
        rotation.SetActive(true);
        menuMain.SetActive(false);
    }

    private void OnBoutonAPressed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Bouton pause presse");

        if (menuPrincipal.activeSelf)
        {
            return;
        }

        if (menuMain.activeSelf) // On remet la lumière normalement progressivement
        {
            restoreLights = true;
            // Activer la rotation
            deplacement.SetActive(true);
            rotation.SetActive(true);
        } 
        else // On allume le spotlight et éteins la lumière du jeu
        {
            // Desac la rotation
            deplacement.SetActive(false);
            rotation.SetActive(false);

            spotlight.SetActive(true);
            restoreLights = false;
            lightCompPlanetarium.intensity = 0;
            lightCompSpotlight.intensity = 8;
        }

        menuMain.SetActive(!menuMain.activeSelf);
    }
}
