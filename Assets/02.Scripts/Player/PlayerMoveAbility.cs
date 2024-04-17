using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMoveAbility : MonoBehaviour
{
    UI_PlayerStat uI_PlayerStat;
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

    // 아이템
    public int AttackItemCount;
    public event Action OnAttackItemChanged; // 이벤트 추가

    // 이펙트
    public GameObject PlayerDieEffect;

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
        }
        else if (moveInput < -0.01f) // 왼쪽 이동 
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
        if (Health <= 0)
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
            if (AttackItemCount > 0) // 공격 아이템이 있을 때만 공격 가능
            {
                Vector3 projectilePosition = transform.position + new Vector3(1f * Mathf.Sign(transform.localScale.x), 0, 0);
                float launchAngle = transform.localScale.x < 0 ? 135f : 45f;
                GameObject orange = Instantiate(OrangePrefab, projectilePosition, Quaternion.identity);
                Rigidbody2D orangeRb = orange.GetComponent<Rigidbody2D>();
                if (orangeRb != null)
                {
                    float launchPower = 5f;
                    Vector2 launchDirection = new Vector2(Mathf.Cos(launchAngle * Mathf.Deg2Rad), Mathf.Sin(launchAngle * Mathf.Deg2Rad));
                    orangeRb.AddForce(launchDirection * launchPower, ForceMode2D.Impulse);
                }
                StartCoroutine(OrangeDestroy(orange));
                AttackItemCount -= 1; // 아이템 사용 후 개수 감소
                OnAttackItemChanged?.Invoke();
                //FindObjectOfType<UI_PlayerStat>().UpdateAttackItemCount(); // UI 업데이트   
            }
            else
            {
                OnAttackItemChanged?.Invoke();
            }
        }
    }

    IEnumerator OrangeDestroy(GameObject orange)
    {
        yield return new WaitForSeconds(3f);
        if (orange != null)
        {
            Destroy(orange); // 오렌지 객체를 파괴
        }
    }

    public void Heal(int amount)
    {
        if (Health < MaxHealth)
        {
            Health += amount;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            FindObjectOfType<UI_PlayerStat>().UpdateHealthDisplay();
        }
    }

    void Damaged()
    {
        _animator.SetTrigger("Hit");
        Health -= 1;
    }


    void Die()
    {
        if (PlayerDieEffect != null)
        {
            GameObject effect = Instantiate(PlayerDieEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1.3f);
        }
        StartCoroutine(DieEffectDelay());
    }

    IEnumerator DieEffectDelay()
    {
        yield return new WaitForSeconds(0.1f);
        this.gameObject.SetActive(false);
    }



    public void AddAttackItem()
    {
        AttackItemCount += 1;
        OnAttackItemChanged?.Invoke();
        FindObjectOfType<UI_PlayerStat>().UpdateAttackItemCount();  // UI 업데이트 호출

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Ground" || other.collider.tag == "Trap_Spike")
        {
            isGrounded = true;
        }
        if (other.collider.tag == "Trap_Spike")
        {
            Debug.Log("체력 -1");
            Damaged();
            FindObjectOfType<UI_PlayerStat>().UpdateHealthDisplay();
        }

        if(other.collider.tag == "Monster")
        {
            Debug.Log("체력 -1");
            Damaged();
            FindObjectOfType<UI_PlayerStat>().UpdateHealthDisplay();
        }

        if (other.gameObject.CompareTag("Item"))
        {
            ItemObject itemObject = other.gameObject.GetComponent<ItemObject>();
            if (itemObject != null && itemObject.ItemType == ItemType.Health)
            {
                Heal(1);
            }
            if (itemObject != null && itemObject.ItemType == ItemType.Attack)
            {
                AddAttackItem();
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("EndPosition"))
        {
            Debug.Log("Stage1_Clear!!");
        }

    }
}