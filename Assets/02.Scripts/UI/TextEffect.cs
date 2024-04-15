using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class TextEffect : MonoBehaviour
{
    public float MoveSpeed;     //텍스트 이동속도
    public float AlphaSpeed;    // 투명도 변환속도
    Text NoOrangeText;

    Color alpha;
    Vector3 originalPosition;   // 원래 위치를 저장할 변수

    private void Start()
    {
        NoOrangeText = GetComponent<Text>();
        alpha = NoOrangeText.color;
        originalPosition = transform.position; // 시작 위치 저장
    }

    void Update()
    {
        transform.Translate(new Vector3 (0, MoveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * AlphaSpeed);
        NoOrangeText.color = alpha;
    }

   void OnDisable()
    {
        alpha = NoOrangeText.color;
        alpha.a = 1f; 
        NoOrangeText.color = alpha; 
        transform.position = originalPosition;
    }
}
