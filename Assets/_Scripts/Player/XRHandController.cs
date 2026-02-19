using UnityEngine;
using UnityEngine.InputSystem;

public class XRHandController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator handAnimator;
    
    [Header("Manual Input Actions")]
    [SerializeField] private InputAction gripAction;
    [SerializeField] private InputAction triggerAction;

    void OnEnable()
    {
        gripAction.Enable();
        triggerAction.Enable();
    }

    void OnDisable()
    {
        gripAction.Disable();
        triggerAction.Disable();
    }

    void Start()
    {
        if (handAnimator == null)
            handAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (handAnimator == null) return;

        float grip = gripAction.ReadValue<float>();
        float trigger = triggerAction.ReadValue<float>();

        Debug.Log($"Grip: {grip:F2}, Trigger: {trigger:F2}");

        handAnimator.SetFloat("Grip", grip);
        handAnimator.SetFloat("Trigger", trigger);
    }
}