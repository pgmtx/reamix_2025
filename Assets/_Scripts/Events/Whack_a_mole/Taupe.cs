using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR.Interaction.Toolkit;

public class Taupe : MonoBehaviour
{
    private Animator animator;
    private WhackAMoleManager manager;
    public bool isUp { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
        manager = FindObjectOfType<WhackAMoleManager>();
    }

    public void GoUp()
    {
        isUp = true;
        animator.SetTrigger("up");
    }

    public void GoDown()
    {
        isUp = false;
        animator.SetTrigger("down");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isUp || manager.score >= 5) return;

        XRGrabInteractable grab = other.GetComponentInParent<XRGrabInteractable>();

        if (other.CompareTag("Marteau") && grab != null && grab.isSelected)
        {
            manager.OnTaupeHit(this);
        }
    }
}
