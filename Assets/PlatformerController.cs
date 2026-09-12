using UnityEngine;
using UnityEngine.InputSystem;

// Setup:
// 1. Add this component to a player GameObject with a Rigidbody2D and Collider2D.
// 2. Set the Rigidbody2D body type to Dynamic and freeze its Z rotation.
// 3. Create a child GameObject named GroundCheck and place it just below the player's feet.
// 4. Assign GroundCheck to the Ground Check field and select the layer used by platforms
//    in Ground Layer. Adjust Movement Speed, Jump Force, and Ground Check Radius as needed.
public class PlatformerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D playerRigidbody;
    private float horizontalInput;
    private bool jumpRequested;
    private bool isGrounded;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            horizontalInput = 0f;
            return;
        }

        horizontalInput = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            horizontalInput -= 1f;
        }

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            horizontalInput += 1f;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = CheckGrounded();

        Vector2 velocity = playerRigidbody.linearVelocity;
        velocity.x = horizontalInput * movementSpeed;
        playerRigidbody.linearVelocity = velocity;

        if (jumpRequested && isGrounded)
        {
            playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        jumpRequested = false;
    }

    private bool CheckGrounded()
    {
        if (groundCheck == null)
        {
            return false;
        }

        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(
            groundCheck.position,
            groundCheckRadius,
            groundLayer);

        foreach (Collider2D groundCollider in groundColliders)
        {
            if (groundCollider.attachedRigidbody != playerRigidbody)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}