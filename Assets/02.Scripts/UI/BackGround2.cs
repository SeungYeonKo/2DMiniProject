using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround2 : MonoBehaviour
{
    public float Speed = 1;

    private void Update()
    {
        Vector2 dir = Vector2.down;
       
        Vector2 newPosition = transform.position + (Vector3)(dir * Speed) * Time.deltaTime;

        if (newPosition.y < -11.87)
        {
            newPosition.y = 11.95f;
        }
        transform.position = newPosition;
    }
}
