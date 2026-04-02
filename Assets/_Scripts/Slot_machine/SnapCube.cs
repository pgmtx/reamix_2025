using System.Threading.Tasks;
using UnityEditor.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapCube : MonoBehaviour
{
    private static bool activeHologram;
    GameObject hologram;

    bool placed = false;
    bool inTheTrigger = false;
    public GameObject snappedObject;

    private void OnTriggerStay(Collider other)
    {
        if (placed) return;

        XRGrabInteractable interactable = other.GetComponent<XRGrabInteractable>();
        if (interactable != null && interactable.isSelected && hologram == null && activeHologram == false)
        {
            activeHologram = true;
            hologram = Instantiate(other.gameObject);

            hologram.GetComponent<Rigidbody>().isKinematic = true;

            foreach (var col in hologram.GetComponentsInChildren<Collider>()) col.enabled = false;

            hologram.transform.position = this.transform.position;
            hologram.transform.rotation = this.transform.rotation;

            hologram.SetActive(true);
            inTheTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (placed) return;

        if (hologram != null && activeHologram == true)
        {
            Destroy(hologram);
            activeHologram = false;
            MyAsyncTask();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (placed || !inTheTrigger) return;
        if (inTheTrigger == true)
        {
            snappedObject = collision.gameObject;
            collision.gameObject.GetComponent<XRGrabInteractable>().selectExited.AddListener(OnGrabAgain);
            placed = true;
            activeHologram = false;
            inTheTrigger = false;
            string[] nameParts = snappedObject.name.Split('_');
            GameObject.Find("machine").GetComponent<WheelRotation>().CheckIfWon(nameParts[1][0], (int)char.GetNumericValue(name[^1]),false);
            Destroy(hologram);
            

            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            collision.gameObject.transform.position = this.gameObject.transform.position;
            collision.gameObject.transform.rotation = this.gameObject.transform.rotation;
            foreach (var col in GetComponents<BoxCollider>()) col.enabled = false;
        }
        else
        {   
            inTheTrigger = false;
        }
        this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void OnGrabAgain(SelectExitEventArgs args)
    {
        placed = false;
        Rigidbody rb = snappedObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        string[] nameParts = snappedObject.name.Split('_');
        GameObject.Find("machine").GetComponent<WheelRotation>().CheckIfWon(nameParts[1][0], (int)char.GetNumericValue(name[^1]), true);

        args.interactableObject.transform.GetComponent<XRGrabInteractable>().selectExited.RemoveListener(OnGrabAgain);

        snappedObject = null;
        foreach (var col in GetComponents<BoxCollider>()) col.enabled = true;
        this.gameObject.layer = LayerMask.NameToLayer("Socle");
    }

    async void MyAsyncTask()
    {
        await Task.Delay(100);
        inTheTrigger = false;
    }
}
