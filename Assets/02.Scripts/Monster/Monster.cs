using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Monster1,
    Monster2,
    Monster3,
    Monster4,
}

public class Monster : MonoBehaviour
{ 
    public MonsterType MonsterType;
    public GameObject MonsterDieEffect;
    public GameObject KeyItem;

    Rigidbody2D _rigidbody;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    public int NextMove;

    // 당근 공격!!
    public GameObject CarrotPrefab;
    public float ShootInterval = 4f; // 2초 간격으로 발사
    private float shootTimer;

    // 몬스터3 플레이어 감지
    public float DetectionRadius = 5f;
    public LayerMask PlayerLayer;
    private bool _isPlayerDetected = false;

    // 체력
    public int Health;
    public int MaxHealth = 5;

    private bool isDead = false;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (MonsterType == MonsterType.Monster4)
        {
            StartCoroutine(FlyRandomly());
            Health = MaxHealth;  
        }
        else
        {
            Invoke("Think", 5);
            Health = (MonsterType == MonsterType.Monster3) ? 25 : MaxHealth;
        }
    }

    private void Update()
    {
        _isPlayerDetected = Physics2D.OverlapCircle(transform.position, DetectionRadius, PlayerLayer);

        if (MonsterType == MonsterType.Monster3)
        {
            shootTimer -= Time.deltaTime;
            if ( _isPlayerDetected && shootTimer <= 0)
            {
                ShootCarrot();
                shootTimer = ShootInterval; // 타이머 초기화
            }
        }
        if (Health <= 0 && !isDead)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        if (MonsterType != MonsterType.Monster4) // 몬스터 4 타입이 아닐 때만 기본 이동 로직 적용
        {
            // 기본 이동
            _rigidbody.velocity = new Vector2(NextMove, _rigidbody.velocity.y);

            // 지형 체크
            Vector2 FrontVector = new Vector2(_rigidbody.position.x + NextMove * 0.2f, _rigidbody.position.y);
            Debug.DrawRay(FrontVector, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(FrontVector, Vector3.down, 1, LayerMask.GetMask("Ground"));
            if (rayHit.collider == null)
            {
                FlipX();
            }
        }
    }





    // 재귀함수 : 자기 자신을 또 호출하는 함수
    void Think()
    {
        NextMove = Random.Range(-1, 2);   //-1(왼쪽) , 0 , 1(오른쪽) 최대값은 포함 안됨

        float NextThinkTime = Random.Range(5f, 7f);
        Invoke("Think", NextThinkTime);

        if (MonsterType != MonsterType.Monster4)
        {
            _animator.SetInteger("MonsterRun", NextMove);
        }
        // Flip Sprite
        if(NextMove != 0)
        {
            _spriteRenderer.flipX = NextMove == 1;
        }
    }

    // 몬스터 4
    IEnumerator FlyRandomly()
    {
        while (!isDead)
        {
            // 다음 무작위 이동까지의 대기 시간 설정
            float waitTime = Random.Range(1f, 2f);

            // 완전한 2D 랜덤 방향을 생성
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            // 이동 설정
            _rigidbody.velocity = randomDirection.normalized * 5f;

            // 지형 체크 및 방향 전환
            if (!IsGrounded())
            {
                // 방향 반전
                randomDirection.x = -randomDirection.x;
                _rigidbody.velocity = randomDirection.normalized * 5f;
                _spriteRenderer.flipX = randomDirection.x < 0;  // 방향에 따른 sprite 뒤집기
            }

            // 지정된 시간 동안 현재 속도로 이동
            yield return new WaitForSeconds(waitTime);
        }
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Trap_Spike") || collision.gameObject.CompareTag("Monster"))
        {
            if (MonsterType == MonsterType.Monster4)
            {
                ChangeDirectionAndMove();
            }
            else
            {
                FlipX();
            }
            if (collision.gameObject.CompareTag("Player"))
            {
                return;
            }
            if (collision.gameObject.CompareTag("AttackOrange"))
            {
                if (MonsterType == MonsterType.Monster3)
                {
                    Damaged();
                }
                Debug.Log("오렌지에 맞았다 -5");
                Health -= 5;
                Destroy(collision.gameObject);  // 몬스터와 부딪히면 오렌지 삭제
            }
        }
    }
    void ChangeDirectionAndMove()
    {

        FlipX();

        // 새로운 방향으로 이동 시작
        Vector2 newDirection = _spriteRenderer.flipX ? Vector2.right : Vector2.left;
        _rigidbody.velocity = newDirection * 5f;
    }



    void FlipX()    // 애니메이션 방향 바꿈
    {
        NextMove *= -1;
        _spriteRenderer.flipX = NextMove == 1;
        CancelInvoke();         // 현재 작동 중인 모든 Invoke함수를 멈추는 함수
        Invoke("Think", 5);
    }

    void ShootCarrot()
    {
        GameObject carrot = Instantiate(CarrotPrefab, transform.position, Quaternion.identity); // 기본 회전 제거
        Rigidbody2D carrotRigidbody = carrot.GetComponent<Rigidbody2D>();

        if (NextMove > 0) 
        {
            Debug.Log("오른쪽발사");
            carrotRigidbody.velocity = new Vector2(8, 0);
            carrot.transform.rotation = Quaternion.Euler(0, 0, -180); 
        }
        else if (NextMove < 0) 
        {
            Debug.Log("왼쪽발사1");
            carrotRigidbody.velocity = new Vector2(-8, 0);
            carrot.transform.rotation = Quaternion.Euler(0, 0, -0); 
        }
        else // 가만히 있을 때
        {
            Debug.Log("왼쪽발사2");
            carrotRigidbody.velocity = new Vector2(-8, 0);
            carrot.transform.rotation = Quaternion.Euler(0, 0, -0);
        }
    }



    void Damaged()
    {
        _animator.SetTrigger("Hit");
        Health -= 1;
    }

    void Die()
    {
        if (!isDead)
        {
            isDead = true;

            if (MonsterDieEffect != null)
            {
                GameObject effect = Instantiate(MonsterDieEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1.3f);
            }

            if (MonsterType == MonsterType.Monster3)
            {
                Debug.Log("키아이템 생성!");
                Instantiate(KeyItem, transform.position, Quaternion.identity);
            }
            StartCoroutine(DieEffectDelay());
        }
    }
    IEnumerator DieEffectDelay()
    {
        yield return new WaitForSeconds(0.1f);
        this.gameObject.SetActive(false);
    }
}
