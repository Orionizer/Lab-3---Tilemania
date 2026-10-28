using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    Rigidbody2D enemyRigidbody;
    BoxCollider2D enemyFeetCollider;
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyFeetCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        enemyRigidbody.linearVelocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipEnemy();
    }

    void FlipEnemy()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody.linearVelocity.x)),1f);
    }
}
