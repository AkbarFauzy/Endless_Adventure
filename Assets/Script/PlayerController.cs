using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EndlessRunner.Module.Player {
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float jumpForce = 5f;

        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;

        [SerializeField] private Animator TopAnim;
        [SerializeField] private Animator BottomAnim;
        
        private Rigidbody2D rb;
        private bool isGrounded;
        private float speed = 5f;
        private float initialSpeed = 5f;
        private float groundCheckRadius = 0.2f;
        private float growthRate = 0.01f;
        private float timeElapsed;

        private float _attackCooldown = 1f;
        private float _timeSinceLastAttack;

        private bool IsPlay { get => GameManager.Instance.isPlaying && !GameManager.Instance.loadNextLevel; }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (IsPlay) {
                _timeSinceLastAttack += Time.deltaTime;
                timeElapsed = Time.timeSinceLevelLoad;

                if (Input.GetKeyDown(KeyCode.F) && _timeSinceLastAttack > _attackCooldown)
                {
                    _timeSinceLastAttack = 0f;
                    TopAnim.SetTrigger("Attack");
                }

                isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
                BottomAnim.SetBool("isGrounded", isGrounded);
                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
                speed = initialSpeed * Mathf.Pow(1 + growthRate, timeElapsed);
            }
            else
            {
                speed = 0f;
            }
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Deadzone")) && IsPlay)
            {
                GameManager.Instance.GameOver();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Deadzone") && IsPlay) {
                GameManager.Instance.GameOver();
            }
        }

    }

}

