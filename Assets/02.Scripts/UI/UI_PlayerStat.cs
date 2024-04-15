using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStat : MonoBehaviour
{
    public PlayerMoveAbility playerMoveAbility;

    public Image[] HeartImages; // 하트 이미지 배열로 선언
    public Text AttackItemTextUI;
    public Text KeyItemTextUI;

    void Update()
    {
        UpdateHealthDisplay();
    }

    public void UpdateHealthDisplay()
    {
        if (playerMoveAbility != null && HeartImages.Length == playerMoveAbility.MaxHealth)
        {
            // 체력 수치에 따라 하트 이미지 활성화 또는 비활성화
            for (int i = 0; i < HeartImages.Length; i++)
            {
                HeartImages[i].enabled = i < playerMoveAbility.Health;
            }
        }
    }
}
