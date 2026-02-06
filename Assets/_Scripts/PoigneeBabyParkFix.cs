using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoigneeBabyParkFix : MonoBehaviour
{
    private void LateUpdate()
    {
        Debug.Log(transform.localRotation);
        transform.localRotation = Quaternion.identity;
    }
}
