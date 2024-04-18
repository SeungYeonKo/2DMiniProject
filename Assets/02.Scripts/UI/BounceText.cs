using UnityEngine;
using UnityEngine.UI;

public enum AlphabetType
{
    C,
    L,
    E,
    A,
    R
}

public class MovingText : MonoBehaviour
{
    public float moveSpeed = 50f; // 텍스트 이동 속도
    public float amplitude = 20f; // 텍스트의 위아래 이동 범위
    public AlphabetType alphabetType;

    private Text textComponent;
    private RectTransform rectTransform;
    private float startY;
    private float initialY;

    void Start()
    {
        textComponent = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
        startY = rectTransform.anchoredPosition.y;
        initialY = startY; // 초기 Y 위치 저장
    }

    void Update()
    {
        // 알파벳 타입에 따른 텍스트 이동 패턴 적용
        float newY = startY + Mathf.Sin(Time.time * moveSpeed + (int)alphabetType) * amplitude;

        // 알파벳에 따라 위아래로 다른 폭으로 움직이도록 조정
        if (alphabetType == AlphabetType.E || alphabetType == AlphabetType.A)
        {
            newY = initialY + Mathf.Sin(Time.time * moveSpeed + (int)alphabetType) * amplitude * 0.5f;
        }
        else if (alphabetType == AlphabetType.L)
        {
            newY = initialY + Mathf.Sin(Time.time * moveSpeed + (int)alphabetType) * amplitude * 1.5f;
        }

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
    }
}