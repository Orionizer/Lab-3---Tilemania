using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    Animator playerAnimator;
    float gravityScaleAtStart;
    CapsuleCollider2D playerCapsuleCollider;
    BoxCollider2D playerBoxCollider;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float bounceSpeed = 10f;
    [SerializeField] Vector2 deathSpeed = new Vector2(10f, 20f);

    bool isAlive = true;
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCapsuleCollider = GetComponent<CapsuleCollider2D>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = playerRigidbody.gravityScale;
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        Run();
        FlipSprite();
        ClimbLadder();
        Death();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!playerBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) 
        {
            playerAnimator.SetBool("isBounceJumping", false);
            return;
        }
        if (value.isPressed)
        {
            if (playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Bounce")))
            {
                playerRigidbody.linearVelocity += new Vector2(0f, bounceSpeed);
                playerAnimator.SetBool("isBounceJumping", true);
            }
            else
            {
                playerRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
            }
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
            if(playerAnimator.GetBool("isBounceJumping") == true)
            {
                transform.localScale = new Vector2(-(Mathf.Sign(playerRigidbody.linearVelocity.x)), 1f);
            }
            else
            {
                transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.linearVelocity.x), 1f);
            }
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }
    }
    void ClimbLadder()
    {
        if (!playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            playerRigidbody.gravityScale = gravityScaleAtStart;
            playerAnimator.SetBool("isClimbing", false);
            return;
        }
        playerRigidbody.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2 (playerRigidbody.linearVelocity.x, moveInput.y * climbSpeed);
        playerRigidbody.linearVelocity = climbVelocity;

        bool hasVerticalSpeed = Mathf.Abs(playerRigidbody.linearVelocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("isClimbing", hasVerticalSpeed);
    }
    void Death()
    {
        if (playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            isAlive = false;
            playerAnimator.SetTrigger("Dying");
            playerRigidbody.linearVelocity = deathSpeed;
        }
    }
}
