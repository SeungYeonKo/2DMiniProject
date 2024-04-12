using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Movespeed = 3.5f;
    public float RunSpeed = 5.3f;         
    public float jumpForce = 3f; 
    private Rigidbody2D rb;
    private bool isGrounded = true; 
    private float moveInput; 
    private bool jumpInput = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal"); 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true; 
        }
    }

    void FixedUpdate()
    {
        Move(); 
        if (jumpInput && isGrounded) 
        {
            Jump();
            jumpInput = false; 
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveInput * Movespeed, rb.velocity.y);
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}