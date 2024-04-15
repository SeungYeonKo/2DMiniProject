using Unity.VisualScripting;
using UnityEngine;
using static Item;

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

    // 원거리 공격 
    public GameObject OrangePrefab;

    void Start()
    {
        Health = MaxHealth;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 이동
        moveInput = Input.GetAxis("Horizontal");
        if (moveInput > 0.01f) // 오른쪽 이동  
        {
            transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        } else if (moveInput < -0.01f) // 왼쪽 이동 
        {
            transform.localScale = new Vector3(-1.3f, 1.3f, 1.3f);
        }

        _animator.SetBool("isRunning", Mathf.Abs(moveInput) > 0.01f && isGrounded);

        // 점프 
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _isJump = true;
        }
        Attack();
        if(Health <=  0)
        {
            Die();
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

        if (_rigidbody.velocity.y < 0)
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

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject orange = Instantiate(OrangePrefab, transform.position + new Vector3(1f, 0, 0), Quaternion.identity); // 오렌지 프리팹 생성 위치 조정
            Rigidbody2D orangeRb = orange.GetComponent<Rigidbody2D>(); // 오렌지의 Rigidbody2D 가져오기
            if (orangeRb != null)
            {
                float launchAngle = 45f; // 발사 각도 (도)
                float launchPower = 5f; // 발사력
                Vector2 launchDirection = new Vector2(Mathf.Cos(launchAngle * Mathf.Deg2Rad), Mathf.Sin(launchAngle * Mathf.Deg2Rad)); // 발사 방향 벡터 계산
                orangeRb.AddForce(launchDirection * launchPower, ForceMode2D.Impulse); // 발사
            }
        }
    }

    void Die()
    {
            this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" || collision.collider.tag == "Trap_Spike")
        {
            isGrounded = true;
        }
        if (collision.collider.tag == "Trap_Spike")
        {
            Debug.Log("체력 -1");
            Health -= 1;
            FindObjectOfType<UI_PlayerStat>().UpdateHealthDisplay();
        }
        if(collision.collider.tag == "Item")
        {
            Debug.Log("아이템을 먹었다!!");
            collision.gameObject.SetActive(false);
        }
    }
}