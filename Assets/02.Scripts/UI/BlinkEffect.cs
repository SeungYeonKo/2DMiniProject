using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 네임스페이스를 사용합니다.

public class BlinkEffect : MonoBehaviour
{
    public Text textComponent; 
    public float time = 0;
    public float blinkSpeed = 0.5f;

    void Start()
    {
        textComponent = GetComponent<Text>(); 
        if (textComponent == null)
        {
            Debug.LogError("BlinkEffect script requires a Text component on the same GameObject.");
        }
    }

    void Update()
    {
        if (textComponent != null)
        {
            if (time < 0.5f)
            {
                textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1 - time * 2);
            }
            else
            {
                textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, (time - 0.5f) * 2);
                if (time > 1f)
                {
                    time = 0; 
                }
            }
        }

        time += Time.deltaTime;
    }
}