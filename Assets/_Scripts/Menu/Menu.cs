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


    // Etat des lumières
    private enum LightState
    {
        Idle,
        Restoring,
        Dimming
    }

    private LightState currentLightState = LightState.Idle;


    void Awake()
    {
        InputAction triggerAction = new InputAction("Trigger", InputActionType.Button);

        triggerAction.AddBinding("<XRController>/triggerPressed");
        triggerAction.performed += OnTriggerPressed;
        triggerAction.Enable();

        // Bloquer les deplacements et la rotation du joueur
        deplacement.SetActive(false);
        rotation.SetActive(false);

        // Pas d'interactions avec la barrière du babypark
        poigneeBarriereInteractable = poigneeBarriere.GetComponent<XRGrabInteractable>();
        poigneeBarriereInteractable.enabled = false;

        // Setup lumières
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
        switch (currentLightState)
        {
            case LightState.Restoring:
                UpdateRestoreLights();
                break;

            case LightState.Dimming:
                UpdateDimmingLights();
                break;
        }
    }

    // Lumière qui revient
    void UpdateRestoreLights()
    {
        lightCompPlanetarium.intensity = Mathf.Min(6, lightCompPlanetarium.intensity + Time.deltaTime * 2f);
        lightCompSpotlight.intensity = Mathf.Max(0, lightCompSpotlight.intensity - Time.deltaTime * 2f);

        if (lightCompPlanetarium.intensity >= 6 && lightCompSpotlight.intensity <= 0)
        {
            if (menuPrincipal.activeSelf)
                menuPrincipal.SetActive(false);

            spotlight.SetActive(false);
            currentLightState = LightState.Idle;
        }
    }

    // Lumière qui s’éteint
    void UpdateDimmingLights()
    {
        lightCompPlanetarium.intensity = Mathf.Max(0, lightCompPlanetarium.intensity - Time.deltaTime * 2f);
        lightCompSpotlight.intensity = Mathf.Min(8, lightCompSpotlight.intensity + Time.deltaTime * 2f);

        if (lightCompPlanetarium.intensity <= 0 && lightCompSpotlight.intensity >= 8)
        {
            currentLightState = LightState.Idle;
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

        currentLightState = LightState.Restoring;

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

        currentLightState = LightState.Restoring;

        deplacement.SetActive(true);
        rotation.SetActive(true);
        menuMain.SetActive(false);
    }

    private void OnBoutonAPressed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Bouton pause presse");

        if (menuPrincipal.activeSelf)
            return;

        bool isOpening = !menuMain.activeSelf;

        if (isOpening)
        {
            // Pause
            deplacement.SetActive(false);
            rotation.SetActive(false);

            spotlight.SetActive(true);
            currentLightState = LightState.Dimming;
        }
        else
        {
            // Reprendre
            deplacement.SetActive(true);
            rotation.SetActive(true);

            currentLightState = LightState.Restoring;
        }

        menuMain.SetActive(isOpening);
    }
}