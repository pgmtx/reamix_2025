using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    public Transform platform;
    public float targetYHeight = 5f;
    public float speed = 2f;

    private bool _shouldMove = false;
    private Vector3 _targetPosition;

    void Start()
    {
        if (platform != null)
        {
            _targetPosition = platform.position + new Vector3(0, targetYHeight, 0);
        }
    }

    void Update()
    {
        if (_shouldMove && platform != null)
        {
            platform.position = Vector3.MoveTowards(platform.position, _targetPosition, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // This is a "Hammer" debug—it will print NO MATTER WHAT touches it
        Debug.Log("I WAS TOUCHED BY: " + other.gameObject.name);
        _shouldMove = true;
    }
}