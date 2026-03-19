using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Menu : MonoBehaviour
{
    [Header("Menu demarrage (statique)")]
    [SerializeField] private GameObject menu;

    [SerializeField] private GameObject boutonJouer;
    private XRSimpleInteractable jouerInteractable;

    [SerializeField] private GameObject boutonQuitter;
    private XRSimpleInteractable quitterInteractable;

    [Header("Menu bouton main")]
    [SerializeField] private GameObject menuMain;


    [Header("GameObjects pour le Menu Demarrage")]
    [SerializeField] private GameObject barriere;
    private XRGrabInteractable barriereInteractable;


    [Header("GameObjects communs")]
    [SerializeField] private GameObject spotlight;
    private Light lightCompSpotlight;

    [SerializeField] private GameObject deplacement;

    [SerializeField] private XRRayInteractor rayInteractor;

    [SerializeField] private GameObject eclairagePlanetarium;
    private Light lightCompPlanetarium;
    private bool updateLight = false;

    void Awake()
    {
        InputAction triggerAction = new InputAction("Trigger", InputActionType.Button);

        //triggerAction.AddBinding("<XRController>{RightHand}/triggerPressed");
        //triggerAction.AddBinding("<XRController>{LeftHand}/triggerPressed");
        triggerAction.AddBinding("<XRController>/triggerPressed");

        triggerAction.performed += OnTriggerPressed;
        triggerAction.Enable();

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
        jouerInteractable?.activated.AddListener(OnPressedJouer);

        // Mettre l'interaction sur le bouton Quitter
        quitterInteractable = boutonQuitter.GetComponent<XRSimpleInteractable>();
        quitterInteractable?.activated.AddListener(OnPressedQuitter);
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

    private void OnTriggerPressed(InputAction.CallbackContext ctx)
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            var interactable = hit.collider.GetComponent<XRSimpleInteractable>();

            if (interactable == jouerInteractable)
                OnPressedJouer(null);

            if (interactable == quitterInteractable)
                OnPressedQuitter(null);
        }
    }

    void OnPressedJouer(ActivateEventArgs args)
    {
        Debug.Log("Le bouton Jouer a ete presse !");

        updateLight = true;

        // On desac les boutons et remet a defaut
        boutonJouer.SetActive(false);
        boutonQuitter.SetActive(false);
        barriereInteractable.enabled = true;
        deplacement.SetActive(true);
    }

    void OnPressedQuitter(ActivateEventArgs args)
    {
        Debug.Log("Le jeu se ferme !!!!!");
        Application.Quit();
    }
}
