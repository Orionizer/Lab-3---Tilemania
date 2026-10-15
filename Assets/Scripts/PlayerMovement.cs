using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D playerRigidBody;
    [SerializeField] float moveSpeed = 5f;
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Run();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, playerRigidBody.linearVelocity.y);
        playerRigidBody.linearVelocity = playerVelocity;
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(playerRigidBody.linearVelocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.linearVelocity.x), 1f);
        }
    }
}
