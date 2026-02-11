using UnityEngine;

public class SimpleElevator : MonoBehaviour
{
    [Header("Movement Settings")]
    public float travelDistance = 5f;
    public float speed = 2f;

    private Vector3 _startPos;
    private Vector3 _endPos;
    private bool _isMoving = false;

    void Start()
    {
        _startPos = transform.position;
        _endPos = _startPos + Vector3.up * travelDistance;
    }

    void Update()
    {
        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _endPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _endPos) < 0.001f)
            {
                _isMoving = false;
            }
        }
    }

    // This fires when you step ONTO the platform
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ACTIVATE: " + other.name + " stepped on the lift!");
        _isMoving = true;

        // This makes the player move WITH the elevator
        other.transform.SetParent(transform);
    }

    // This "un-parents" you when you step off or reach the top
    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}