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
    public MonsterType MosterType;
    Rigidbody2D _rigidbody;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // 기본 이동 
        _rigidbody.velocity = new Vector2(-1, _rigidbody.velocity.y);
    }

    
}
