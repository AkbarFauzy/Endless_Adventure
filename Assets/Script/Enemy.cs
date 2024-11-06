using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMovement { 
    Stay,
    Waving,
    Circular,
    Jumping, 
}

public class Enemy : MonoBehaviour
{
    public string EnemyName;
    [SerializeField] EnemyMovement movement;
    [SerializeField] private float speed = 2f;

    private bool isGrounded;
    private Rigidbody2D rb;
    private EnemySpawner _spawner;
    private float amplitude = 1f;
    private float frequency = 1f;
    private float initialX;
    private float jumpInterval = 4f;
    private float timeSinceLastJump;

    [SerializeField] private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialX = transform.position.x;
        if (movement == EnemyMovement.Stay) {
            rb.velocity = Vector2.left * speed;
        }
    }


    private void Update()
    {
        switch (movement) {
            case EnemyMovement.Waving:
                WavingMovement();
                break;
            case EnemyMovement.Circular:
                break;
            case EnemyMovement.Jumping:
                timeSinceLastJump += Time.deltaTime;
                Jump();
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            anim.SetTrigger("Died");
        }
    }

    public void SetSpawner(EnemySpawner spawner) {
        _spawner = spawner;
    }

    private void Jump() {
        if (isGrounded && timeSinceLastJump > jumpInterval)
        {
            anim.SetTrigger("Jump");
            rb.velocity = new Vector2(-2f, 5f);
            timeSinceLastJump = 0;
        }   
    }

    private void WavingMovement() {
        float x = initialX + speed * Time.time; 
        float y = amplitude * Mathf.Sin(frequency * Time.time);
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private void OnDied() {
        if(_spawner != null)
            _spawner.AddEnemyToPool(this);

        if (GameManager.Instance != null)
            GameManager.Instance.EnemyKilled(this);
    }

}
