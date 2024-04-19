using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PlayerMoveAbility : MonoBehaviour
{
    // 참조
    public CurrentStage CurrentStage;


    private Rigidbody2D _rigidbody;
    private Animator _animator;

    public Text NoOrangeText;

    // 이벤트
    public event Action OnAttackItemChanged; 
    public event Action OnKeyItemChanged;
    public event Action OnMaxKeyItemCountChanged;

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
    public int KeyItemCount;
    public int MaxKeyItemCount;

    // 이펙트
    public GameObject PlayerDieEffect;

    // 사운드 
    public AudioSource PlayerDeath;
    public AudioSource PlayerJump;
    public AudioSource PlayerMinusHealth;
    public AudioSource PlayerAttack;
    public AudioSource PlayerEatHealthItem;

    private bool _isDie;
    
    public SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        CurrentStage = GetComponent<CurrentStage>() ?? FindObjectOfType<CurrentStage>();

        GameObject SoundController = GameObject.Find("PlayerDeath");
        GameObject SoundController2 = GameObject.Find("PlayerJump");
        GameObject SoundController3 = GameObject.Find("PlayerMinusHealth");
        GameObject SoundController4 = GameObject.Find("PlayerAttack");
        GameObject SoundController5 = GameObject.Find("PlayerEatHealthItem");
        PlayerDeath = SoundController.GetComponent<AudioSource>();
        PlayerDeath = SoundController2.GetComponent<AudioSource>();
        PlayerDeath = SoundController3.GetComponent<AudioSource>();
        PlayerDeath = SoundController4.GetComponent<AudioSource>();
        PlayerDeath = SoundController5.GetComponent<AudioSource>();
        
 

        if (CurrentStage == null)
        {
            Debug.LogError("StageClear 참조가 설정되지 않았습니다!");
            return; 
        }

        SetKeyItemCountBasedOnStage();
    }

    void Start()
    {
        _isDie = false;
        Health = MaxHealth;
        NoOrangeText.gameObject.SetActive(false); // 초기에는 경고 메시지를 숨김
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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || CompareTag("Monster"))
        {
            _isJump = true;
            PlayerJump.Play();
        }
        Attack();
        if (Health <= 0)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.D))
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
                PlayerAttack.Play();
                AttackItemCount -= 1; // 아이템 사용 후 개수 감소
                OnAttackItemChanged?.Invoke();
                //FindObjectOfType<UI_PlayerStat>().UpdateAttackItemCount(); // UI 업데이트   
            }
            else
            {
                NoOrangeText.gameObject.SetActive(true);
                OnAttackItemChanged?.Invoke();
            }
        }
    }

    IEnumerator OrangeDestroy(GameObject orange)
    {
        yield return new WaitForSeconds(1f);
        if (orange != null)
        {
            Destroy(orange); // 오렌지 객체를 파괴
        }
    }

    public void Heal(int amount)
    {
        if (Health < MaxHealth)
        {
            PlayerEatHealthItem.Play();
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
        PlayerMinusHealth.Play();
        Health -= 1;
    }

    void Die()
    {
        if (!_isDie)
        {
            _isDie = true;
            if (PlayerDieEffect != null)
            {
                GameObject effect = Instantiate(PlayerDieEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1.3f);
            }
            PlayerDeath.Play();
            StartCoroutine(DieEffectDelay());
        }
    }

    IEnumerator DieEffectDelay()
    {
        Color newColor = _spriteRenderer.color;
        newColor.a = 0f;
        _spriteRenderer.color = newColor;

        yield return new WaitForSeconds(1f);  // 사망 사운드 길이만큼 대기

        SceneManager.LoadScene("StartScene");  // 씬 로드
    }


    public void AddAttackItem()
    {
        AttackItemCount += 1;
        OnAttackItemChanged?.Invoke();
        FindObjectOfType<UI_PlayerStat>().UpdateAttackItemCount();  // UI 업데이트 호출
    }

    public void AddKeyItem()
    {
        Debug.Log("키아이템획득!");
        KeyItemCount += 1;
        OnKeyItemChanged?.Invoke(); 
        FindAnyObjectByType<UI_PlayerStat>().UpdateKeyItemCount();
    }

    private void SetKeyItemCountBasedOnStage()
    {
        switch (CurrentStage.StageType)
        {
            case StageType.Stage1:
                MaxKeyItemCount = 2;
                break;
            case StageType.Stage2:
                MaxKeyItemCount = 4;
                break;
            case StageType.Stage3:
                MaxKeyItemCount = 6;
                break;
            default:
                MaxKeyItemCount = 2;
                break;
        }
        OnMaxKeyItemCountChanged?.Invoke(); // 이벤트 발생
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground") || other.collider.CompareTag("Trap_Spike") || other.collider.CompareTag("Monster"))
        {
            isGrounded = true;
        }
        if (other.collider.tag == "Trap_Spike" || other.collider.tag == "Monster" || other.collider.tag == "Carrot")
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
            if (itemObject != null && itemObject.ItemType == ItemType.Key)
            {
                AddKeyItem();
            }
            Destroy(other.gameObject);
        }
    }
}