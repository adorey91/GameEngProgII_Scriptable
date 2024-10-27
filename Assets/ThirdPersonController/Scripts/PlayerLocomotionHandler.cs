using System.Collections.Generic;
using UnityEngine;

// Sam Robichaud 
// NSCC Truro 2024
// This work is licensed under CC BY-NC-SA 4.0 (https://creativecommons.org/licenses/by-nc-sa/4.0/)
public class PlayerLocomotionHandler : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CharacterController characterController;
    public static float movementSpeed;

    [Header("Debugging Values")]
    [SerializeField] public bool isSprinting;

    [Header("Debug Output (read only)")]
    public static float playerVelocity;
    [SerializeField] private bool playerIsGrounded;

    [Header("Movement Speeds")]
    //public float walkingSpeed;
    //public float joggingSpeed = 4f;
    //public float sprintingSpeed = 8f;
    public float rotationSpeed = 15f;
    public float gravity = -30f; // Gravity value to apply to the player
    public float jumpHeight = 3.0f; // Jump height

    private Vector3 moveDirection;
    private Vector3 velocity;
    private bool isJumping = false; // Track if player is currently jumping

    [SerializeField] private CharacterStatsSO playerStats;
    [SerializeField] private List<ItemsSO> collectedItems = new List<ItemsSO>();

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Start()
    {
        Debug.Log($"Character: {playerStats.characterName}, Health: {playerStats.health}, Attack Power: {playerStats.attackPower}, Walking Speed: {playerStats.walkingSpeed}, Jogging Speed: {playerStats.walkingSpeed}, Sprinting Speed: {playerStats.sprintingSpeed}");
    }

    public void Update()
    {
        if (playerStats.NameChanged())
        {
            Debug.Log($"Character: {playerStats.characterName}, Health: {playerStats.health}, Attack Power: {playerStats.attackPower}, Walking Speed: {playerStats.walkingSpeed}, Jogging Speed: {playerStats.walkingSpeed}, Sprinting Speed: {playerStats.sprintingSpeed}");
        }
    }



    public void HandleAllPlayerMovement()
    {
        playerIsGrounded = characterController.isGrounded; // Check if grounded
        HandlePlayerMovement();
        HandlePlayerRotation();
        UpdatePlayerVelocityMagnitude();  // Debugging tool

    }

    private void HandlePlayerMovement()
    {
        // Normal movement calculation
        moveDirection = cameraTransform.forward * inputManager.verticalInput;
        moveDirection += cameraTransform.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        // Adjust speed based on sprinting, jogging, or walking
        if (isSprinting)
        {
            moveDirection *= playerStats.sprintingSpeed;
        }
        else if (inputManager.moveAmount >= 0.5f)
        {
            moveDirection *= playerStats.joggingSpeed;
        }
        else
        {
            moveDirection *= playerStats.walkingSpeed;
        }

        // Apply gravity
        if (characterController.isGrounded)
        {
            isJumping = false;
            if (velocity.y < 0)
            {
                velocity.y = -2f; // Small downward force to stay grounded
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // Apply gravity when not grounded
        }

        // Move the character controller
        characterController.Move(moveDirection * Time.deltaTime + velocity * Time.deltaTime);

    }

    private void HandlePlayerRotation()
    {
        // Calculate target direction based on camera forward/right vectors
        Vector3 targetDirection = cameraTransform.forward * inputManager.verticalInput;
        targetDirection += cameraTransform.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        // Rotate smoothly towards the target direction
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    public void HandleJump()
    {
        // Only jump if grounded
        if (characterController.isGrounded)
        {
            isJumping = true;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Apply jump force
        }
    }

    private void UpdatePlayerVelocityMagnitude()
    {
        // Calculate the player's velocity magnitude, including both movement and vertical velocity (gravity/jumping)
        playerVelocity = characterController.velocity.magnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            ItemHolder chest = other.GetComponent<ItemHolder>();
            ItemsSO chestItem = chest.chestItem;

            if (!collectedItems.Contains(chestItem))
            {
                chestItem.CollectItem();
                collectedItems.Add(chestItem);
            }
            else
            {
                Debug.Log($"{chestItem} was already collected and cannot be picked up again.");
            }
        }
    }
}