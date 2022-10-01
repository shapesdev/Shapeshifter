using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float m_Speed = 10f;
    [SerializeField]
    private float m_JumpSpeed = 10f;

    private Rigidbody2D m_Rigidbody;
    private SpriteRenderer m_SpriteRenderer;
    private bool m_Grounded;
    private int m_JumpCount = 0;

    private float m_LastGroundedTime;
    private float m_LastJumpTime;

    private void Start() {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update() {
        Move();

        if(Input.GetKeyDown(KeyCode.Space)) {
            if(m_Grounded || m_JumpCount == 1) {
                Jump();
            }
            else if(m_LastGroundedTime > 0 && m_LastJumpTime > 0 && m_JumpCount == 0) {
                Jump();
            }
        }
    }

    private void Move() {
        var input = Input.GetAxisRaw("Horizontal");
        m_Rigidbody.velocity = new Vector2(input * m_Speed, m_Rigidbody.velocity.y);
        m_LastJumpTime -= Time.deltaTime;
    }

    private void Jump() {
        if(m_Rigidbody.velocity.y < -0.1) {
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0);
        }
        m_Rigidbody.AddForce(Vector2.up * m_JumpSpeed, ForceMode2D.Impulse);
        m_JumpCount++;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Ground")) {
            m_Grounded = true;
            m_JumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            m_Grounded = false;
            m_LastGroundedTime -= Time.deltaTime;
        }
    }
}