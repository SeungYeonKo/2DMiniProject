using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Monster1,
    Monster2,
    Monster3
}

public class MonsterMoveAbility : MonoBehaviour
{ 
    public MonsterType MonsterType;
    Rigidbody2D _rigidbody;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    public int NextMove;

    // 당근 공격!!
    public GameObject CarrotPrefab;
    public float ShootInterval = 2f; // 2초 간격으로 발사
    private float shootTimer;

    // 체력
    public int Health;
    public int MaxHealth = 5;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke("Think", 5);     // 함수를 5초 뒤에 호출

        Health = MaxHealth;
    }

    private void Update()
    {
        if (MonsterType == MonsterType.Monster3)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                ShootCarrot();
                shootTimer = ShootInterval; // 타이머 초기화
            }
        }
    }

    void FixedUpdate()
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

    // 재귀함수 : 자기 자신을 또 호출하는 함수
    void Think()
    {
        NextMove = Random.Range(-1, 2);   //-1(왼쪽) , 0 , 1(오른쪽) 최대값은 포함 안됨

        float NextThinkTime = Random.Range(5f, 7f);
        Invoke("Think", NextThinkTime);

        _animator.SetInteger("MonsterRun", NextMove);

        // Flip Sprite
        if(NextMove != 0)
        {
            _spriteRenderer.flipX = NextMove == 1;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Trap_Spike")
            || collision.gameObject.CompareTag("Monster"))
        {
            FlipX();
        }
    }

    void ShootCarrot()
    {
        GameObject carrot = Instantiate(CarrotPrefab, transform.position, Quaternion.Euler(0, 0, -90));
        Rigidbody2D carrotRigidbody = carrot.GetComponent<Rigidbody2D>();
        int direction = _spriteRenderer.flipX ? 1 : -1; // 현재 몬스터의 방향에 따라 당근 발사 방향 결정
        carrotRigidbody.velocity = new Vector2(direction * 3, 0); // 당근 발사 속도 및 방향 설정
    }


    void FlipX()    // 애니메이션 방향 바꿈
    {
        NextMove *= -1;
        _spriteRenderer.flipX = NextMove == 1;
        CancelInvoke();         // 현재 작동 중인 모든 Invoke함수를 멈추는 함수
        Invoke("Think", 5);
    }
}
