using UnityEngine;
using System.Collections; // Required for the Delay (Coroutine)

public class ElevatorAction : MonoBehaviour
{
    [Header("Elevator Settings")]
    public float height = 10f;
    public float speed = 3f;
    public float startDelay = 2f; // Time in seconds
    public AudioSource liftSound;

    [Header("Gate Settings")]
    public Transform gate; // Drag your Gate object here
    public float gateMoveDistance = 2.5f;
    public float gateSpeed = 2f;

    private Vector3 _elevatorEndPos;
    private Vector3 _gateOpenPos;
    private Vector3 _gateClosedPos;

    private bool _isElevatorMoving = false;
    private bool _isGateClosing = false;

    void Start()
    {
        _elevatorEndPos = transform.position + Vector3.up * height;

        if (gate != null)
        {
            _gateOpenPos = gate.localPosition;
            _gateClosedPos = _gateOpenPos + Vector3.up * gateMoveDistance;
        }
    }

    // This is called by your GameEventListener
    public void StartSequence()
    {
        StartCoroutine(ElevatorSequence());
    }

    IEnumerator ElevatorSequence()
    {
        Debug.Log("Player detected. Closing gates...");
        _isGateClosing = true;

        // Wait for the delay you requested
        yield return new WaitForSeconds(startDelay);

        Debug.Log("Starting lift!");
        _isElevatorMoving = true;
        if (liftSound != null) liftSound.Play();
    }

    void Update()
    {
        // Handle Gate Movement (Up)
        if (_isGateClosing && gate != null)
        {
            gate.localPosition = Vector3.MoveTowards(gate.localPosition, _gateClosedPos, gateSpeed * Time.deltaTime);
        }

        // Handle Elevator Movement (Up)
        if (_isElevatorMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _elevatorEndPos, speed * Time.deltaTime);

            // Once we reach the top
            if (Vector3.Distance(transform.position, _elevatorEndPos) < 0.01f)
            {
                _isElevatorMoving = false;
                _isGateClosing = false; // Stop trying to close the gate
                StartCoroutine(OpenGateAtTop());
            }
        }
    }

    IEnumerator OpenGateAtTop()
    {
        Debug.Log("Arrived! Opening gates...");
        // This moves the gate back down
        while (Vector3.Distance(gate.localPosition, _gateOpenPos) > 0.01f)
        {
            gate.localPosition = Vector3.MoveTowards(gate.localPosition, _gateOpenPos, gateSpeed * Time.deltaTime);
            yield return null;
        }
    }
}