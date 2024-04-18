using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        if (collision.collider.CompareTag("AttackOrange")) 
        {
            collision.gameObject.SetActive(false); 
        }
    }
}
