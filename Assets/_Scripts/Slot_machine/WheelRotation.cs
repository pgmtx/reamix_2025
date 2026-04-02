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

    [Header("Levier Casino")]
    public Transform handleAnchor;
    public float thresholdAngle = 55f;

    [Header("Event")]
    public GameEvent motFini;

    private bool hasPulled = false;
    private int currentIndexWheel1 = 0;
    private int currentIndexWheel2 = 0;
    private int currentIndexWheel3 = 0;
    private int currentIndexWheel4 = 0;
    private int counter = 0;
    private bool isRotating = false;

    public void Start()
    {
        RotateStep();
    }

    public string word;
    public void RotateStep()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateToWord());
        }
    }

    private IEnumerator RotateToWord()
    {
        isRotating = true;
        string[] listeWords = { "MOOO", "MEOW", "JHIN", "BARK", "WOOF", "WICK", "CROA", "OINK", "ROAR", "PEEP" };
        word = listeWords[Random.Range(0, listeWords.Length)];
        Debug.Log("Cible : " + word);
        counter = 0;

        Coroutine[] routines = new Coroutine[4];
        for (int i = 0; i < 4; i++)
        {
            routines[i] = StartCoroutine(RotateSingleWheelToLetter(i, word[i]));
        }

        foreach (var r in routines) yield return r;

        isRotating = false;
    }

    private IEnumerator RotateSingleWheelToLetter(int wheelIndex, char targetLetter)
    {
        Transform wheel = GetWheel(wheelIndex);
        Transform[] cubes = GetCubes(wheelIndex);
        int targetIndex = -1;

        for (int j = 0; j < cubes.Length; j++)
        {
            if (cubes[j].name[1] == targetLetter)
            {
                targetIndex = j;
                break;
            }
        }

        if (targetIndex == -1) yield break;

        int currentIndex = GetCurrentIndexForWheel(wheelIndex);

        while (currentIndex != targetIndex)
        {
            Quaternion start = wheel.localRotation;
            Quaternion end = start * Quaternion.Euler(0, rotationStep, 0);

            float elapsed = 0;
            float stepDuration = rotationDuration / 4f;

            while (elapsed < stepDuration)
            {
                wheel.localRotation = Quaternion.Slerp(start, end, elapsed / stepDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            wheel.localRotation = end;

            currentIndex = (currentIndex + 1) % cubes.Length;
            SetCurrentIndexForWheel(wheelIndex, currentIndex);
        }
        Vector3 currentPos = cubes[targetIndex].localPosition;
        cubes[targetIndex].localPosition = new Vector3(currentPos.x, currentPos.y, -0.05509732f);
    }

    private int GetCurrentIndexForWheel(int wheelIndex)
    {
        switch (wheelIndex)
        {
            case 0: return currentIndexWheel1;
            case 1: return currentIndexWheel2;
            case 2: return currentIndexWheel3;
            case 3: return currentIndexWheel4;
            default: return 0;
        }
    }

    private void SetCurrentIndexForWheel(int wheelIndex, int value)
    {
        switch (wheelIndex)
        {
            case 0: currentIndexWheel1 = value; break;
            case 1: currentIndexWheel2 = value; break;
            case 2: currentIndexWheel3 = value; break;
            case 3: currentIndexWheel4 = value; break;
        }
    }

    private Transform[] GetCubes(int index)
    {
        if (index == 0) return cubes1;
        if (index == 1) return cubes2;
        if (index == 2) return cubes3;
        return cubes4;
    }

    private Transform GetWheel(int index)
    {
        if (index == 0) return wheel1;
        if (index == 1) return wheel2;
        if (index == 2) return wheel3;
        return wheel4;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RotateStep();
        }

        float currentAngle = handleAnchor.localEulerAngles.z;

        if (currentAngle > 180) currentAngle -= 360;

        if (currentAngle >= 55f && !hasPulled && !isRotating)
        {
            hasPulled = true;
            RotateStep();
        }

        if (hasPulled && Mathf.Abs(currentAngle) < 5f)
        {
            hasPulled = false;
        }
    }

    public void CheckIfWon(char lettre, int slot, bool removed)
    {
        if (word[slot] == lettre && removed)
            counter--;
        if (word[slot] == lettre && !removed)
            counter++;
        if (counter == 4)
        {
            motFini.TriggerEvent();
            Debug.Log("u won gg");
        }
    }
}
