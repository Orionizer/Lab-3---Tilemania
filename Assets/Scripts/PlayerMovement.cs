using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    Animator playerAnimator;
    float gravityScaleAtStart;
    BoxCollider2D playerBoxCollider;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = playerRigidbody.gravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!playerBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) 
        {
            return;
        }
        if (value.isPressed)
        {
            playerRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, playerRigidbody.linearVelocity.y);
        playerRigidbody.linearVelocity = playerVelocity;
    }

    void FlipSprite()
    {
        if (Mathf.Abs(moveInput.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.linearVelocity.x), 1f);
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }
    }
    void ClimbLadder()
    {
        if (!playerBoxCollider.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            playerRigidbody.gravityScale = gravityScaleAtStart;
            return;
        }
        playerRigidbody.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2 (playerRigidbody.linearVelocity.x, moveInput.y * climbSpeed);
        playerRigidbody.linearVelocity = climbVelocity;
    }
}
