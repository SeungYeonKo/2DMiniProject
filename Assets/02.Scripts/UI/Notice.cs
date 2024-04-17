using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{
    public Image HowToImage; // 인스펙터에서 직접 할당

    private void Start()
    {
        // 시작할 때 이미지를 비활성화합니다. 이미지는 인스펙터에서 할당되어야 합니다.
        if (HowToImage != null)
            HowToImage.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어가 충돌했을 때 HowToImage를 활성화합니다.
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("HowToPlay!");
            HowToImage.gameObject.SetActive(true);
        }
    }
}