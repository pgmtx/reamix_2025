using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorPorteBabypark : MonoBehaviour
{
    private void Awake()
    {
        ConfigurableJoint joint = GetComponent<ConfigurableJoint>();
        Debug.Log(gameObject.transform.localPosition);
        Debug.Log(transform.localPosition - joint.axis.normalized * joint.linearLimit.limit);
        transform.localPosition = transform.localPosition + joint.axis.normalized * 2 * joint.linearLimit.limit;
    }
}
