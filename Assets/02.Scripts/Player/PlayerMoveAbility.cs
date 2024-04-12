using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    // [ 체력 ]
    public int Health;
    public int MaxHealth = 10;

    // [ 이동 ]
    public float Movespeed = 3.5f;

    // [ 점프 ]
    public float JumpPower = 3f;
    public float FallMultiplier = 2.5f;

    private bool isGrounded = true;
    private bool _isJump = false;
    private float moveInput; 
    

    void Start()
    {
        Health = MaxHealth;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        // 플레이어의 수평 이동 입력에 따라 애니메이션 상태를 변경
        if (Mathf.Abs(moveInput) > 0.01f)  // 움직임이 감지되면
        {
            _animator.SetBool("isRunning", true);
        }
        else  // 움직임이 없으면
        {
            _animator.SetBool("isRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _isJump = true;
        }
    }

    void FixedUpdate()
    {
        Move();
        if (_isJump)
        {
            Jump();
            _isJump = false;
        }

        // 점프 하강 중 추가 중력 적용
        if (_rigidbody.velocity.y < 0) // 하강 중일 때
        {
            _rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void Move()
    {
        _rigidbody.velocity = new Vector2(moveInput * Movespeed, _rigidbody.velocity.y);
    }

    void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0); // 점프 전 속도 초기화
        _rigidbody.AddForce(new Vector2(0f, JumpPower), ForceMode2D.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            gameObject.SetActive(false);
        }
    }
}