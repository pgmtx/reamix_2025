using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    [Header("Roues")]
    public Transform wheel1;
    public Transform wheel2;
    public Transform wheel3;
    public Transform wheel4;

    [Header("Wheel 1")]
    public Transform[] cubes1 = new Transform[8];

    [Header("Wheel 2")]
    public Transform[] cubes2 = new Transform[8];

    [Header("Wheel 3")]
    public Transform[] cubes3 = new Transform[8];

    [Header("Wheel 4")]
    public Transform[] cubes4 = new Transform[8];

    [Header("Rotation Settings")]
    public float rotationStep = 45f;
    public float rotationDuration = 1f;

    private int currentIndexWheel1 = 0;
    private int currentIndexWheel2 = 0;
    private int currentIndexWheel3 = 0;
    private int currentIndexWheel4 = 0;
    private bool isRotating = false;

    public void RotateStep()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateWheels());
        }
    }

    private IEnumerator RotateWheels()
    {
        isRotating = true;

        float elapsed = 0f;

        Quaternion start1 = wheel1.localRotation;
        Quaternion start2 = wheel2.localRotation;
        Quaternion start3 = wheel3.localRotation;
        Quaternion start4 = wheel4.localRotation;

        Quaternion end1 = start1 * Quaternion.Euler(0f, rotationStep, 0f);
        Quaternion end2 = start2 * Quaternion.Euler(0f, rotationStep, 0f);
        Quaternion end3 = start3 * Quaternion.Euler(0f, rotationStep, 0f);
        Quaternion end4 = start4 * Quaternion.Euler(0f, rotationStep, 0f);

        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;

            if (wheel1) wheel1.localRotation = Quaternion.Slerp(start1, end1, t);
            if (wheel2) wheel2.localRotation = Quaternion.Slerp(start2, end2, t);
            if (wheel3) wheel3.localRotation = Quaternion.Slerp(start3, end3, t);
            if (wheel4) wheel4.localRotation = Quaternion.Slerp(start4, end4, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (wheel1) wheel1.localRotation = end1;
        if (wheel2) wheel2.localRotation = end2;
        if (wheel3) wheel3.localRotation = end3;
        if (wheel4) wheel4.localRotation = end4;

        isRotating = false;
        currentIndexWheel1 = (currentIndexWheel1 + 1) % cubes1.Length;
        currentIndexWheel2 = (currentIndexWheel2 + 1) % cubes2.Length;
        currentIndexWheel3 = (currentIndexWheel3 + 1) % cubes3.Length;
        currentIndexWheel4 = (currentIndexWheel4 + 1) % cubes4.Length;
        Debug.Log("Indexes: "+ currentIndexWheel1 + " "+ currentIndexWheel2 + " "+ currentIndexWheel3 + " "+ currentIndexWheel4);
        Debug.Log(cubes1[currentIndexWheel1].name + cubes2[currentIndexWheel2].name + cubes3[currentIndexWheel3].name + cubes4[currentIndexWheel4].name);
        string l1 = cubes1[currentIndexWheel1] != null ? cubes1[currentIndexWheel1].name : "_";
        string l2 = cubes2[currentIndexWheel2] != null ? cubes2[currentIndexWheel2].name : "_";
        string l3 = cubes3[currentIndexWheel3] != null ? cubes3[currentIndexWheel3].name : "_";
        string l4 = cubes4[currentIndexWheel4] != null ? cubes4[currentIndexWheel4].name : "_";

        Debug.Log("Word: " + l1 + l2 + l3 + l4);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RotateStep();
        }
    }
}
