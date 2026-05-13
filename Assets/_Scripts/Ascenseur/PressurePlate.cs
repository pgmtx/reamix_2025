using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField]
    private GameEvent onPlayerPressure;

    // NOTE(Eric-Nicolas): PlaquePression ne touche volontairement pas le sol,
    // sinon cette méthode se déclenche car en contact avec le tapis.
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered by " + other.name + " with tag " + other.tag);
        if (other.CompareTag("Player"))
        {
            // TODO: Réactiver avant de commit
            /*
            if (!WhackAMoleManager.IsFinished)
            {
                Debug.Log("Whack-a-mole non terminé: la plaque de pression ne se déclenche pas.");
                return;
            }
            */

            onPlayerPressure.TriggerEvent();
            gameObject.SetActive(false);
            Debug.Log("Transition vers l'autre salle là");
        }
    }
}
