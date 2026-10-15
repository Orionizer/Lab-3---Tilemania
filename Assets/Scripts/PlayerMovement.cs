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
}
