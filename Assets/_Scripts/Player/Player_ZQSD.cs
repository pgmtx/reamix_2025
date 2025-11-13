using System;
using System.Collections.Generic;
using UnityEngine;

public class Player_ZQSD : MonoBehaviour
{
    // Header affiche dans l'inspecteur un titre
    [Header("Movement")]
    public const float DefaultSpeed = 6f;
    public float Speed = DefaultSpeed;
    // SerializeField pour montrer les valeurs dans l'inspecteur même si elles sont privées
    [SerializeField] private float accelerationDamping = 25f;
    [SerializeField] private float decelerationDamping = 15f;
    private Vector2 inputMovement;
    private Vector3 currentVelocity;
    private Vector3 velocity;

    [Header("Jump & Gravity")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float fallGravityMultiplier = 2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField][ReadOnly] private bool isGrounded;

    [Header("Camera")]
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private Transform rotateWithCamera;
    public float MouseSensitivity = 20f;
    private Vector2 lookInput;
    private float xRotation = 0f;

    [Header("References")]
    [SerializeField] private CharacterController controller;

    // Awake appelée avant la première frame où l'objet est actif dans la scène
    private void Awake()
    {
        // Testing
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Start est appelée juste après Awake
    private void Start()
    {

    }

    // Update est appelée à toutes les frames
    void Update()
    {
        GatherInputs();
        HandleCamera();
        HandleMovement();
        HandleCrouch();
    }

    // FixedUpdate est comme update mais utile pour la physique
    private void FixedUpdate()
    {
        HandleGravity();
    }

    void GatherInputs()
    {
        // Les noms et les touches sont modifiables dans Edit -> Porject Settings -> Input Manager -> Axes
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inputMovement = new Vector2(h, v);

        float mouseX = 40 * Input.GetAxis("Mouse X");
        float mouseY = 40 * Input.GetAxis("Mouse Y");
        lookInput = new Vector2(mouseX, mouseY);
    }

    void HandleCamera()
    {
        float mouseX = lookInput.x * MouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * MouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Tous les GO qui sont dans rotateWithCamera suivront la rotation de la camera
        foreach (Transform child in Helpers.GetComponentsInDirectChildren<Transform>(rotateWithCamera))
        {
            child.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        // Calculer le vecteur de mouvement
        Vector3 targetVelocity = transform.right * inputMovement.x + transform.forward * inputMovement.y;
        targetVelocity.y = 0f;
        targetVelocity.Normalize();
        targetVelocity *= Speed;

        // Appliquer du damping (modifie mouvement progressivement)
        float effectiveDamping = (targetVelocity.magnitude > currentVelocity.magnitude)
            ? accelerationDamping
            : decelerationDamping;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, effectiveDamping * Time.deltaTime);

        // Déplacer le joueur
        controller.Move(currentVelocity * Time.deltaTime);
    }

    void HandleGravity()
    {
        // Coller au sol
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Appliquer la gravité
        if (velocity.y < 3.5f) // Rend la chute plus rapide
            velocity.y += gravity * fallGravityMultiplier * Time.deltaTime;
        else
            velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleCrouch()
    {
        // Disgusting temporaire
        if (Input.GetButtonDown("Crouch"))
        {
            Debug.Log("S'accroupir");
            if (isGrounded)
            {
                transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
                Speed = DefaultSpeed / 2;
            }
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            Debug.Log("Se lever");
            if (isGrounded)
            {
                transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
                Speed = DefaultSpeed;
            }
        }
    }
}
