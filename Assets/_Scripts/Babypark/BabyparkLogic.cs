using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyparkLogic : MonoBehaviour
{
    public GameObject Poignee;
    public GameEvent PlayerOutBabyPark;

    private void Awake()
    {
        // Déplacement de la poignee au milieu
        ConfigurableJoint joint = Poignee.GetComponent<ConfigurableJoint>();
        Poignee.transform.localPosition = Poignee.transform.localPosition + joint.axis.normalized * 2 * joint.linearLimit.limit;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger hit: " + other.name);
        if (other.tag == "Player")
        {
            PlayerOutBabyPark.TriggerEvent();
        }
    }
}
