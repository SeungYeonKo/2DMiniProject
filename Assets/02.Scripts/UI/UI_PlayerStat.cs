using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStat : MonoBehaviour
{
    public PlayerMoveAbility playerMoveAbility;

    public Image[] HeartImages;
    public Text AttackItemTextUI;
    public Text KeyItemTextUI;

    // 하트이미지
    void Update()
    {
        UpdateHealthDisplay();
    }
    void Start()
    {
        if (playerMoveAbility != null)
        {
            playerMoveAbility.OnMaxKeyItemCountChanged += UpdateKeyItemCount; // MaxKeyItemCount 변경 이벤트 구독
            UpdateKeyItemCount(); // 초기 설정 반영
            playerMoveAbility.OnAttackItemChanged += UpdateAttackItemCount; // 공격 아이템 변경 이벤트 구독
            UpdateAttackItemCount(); // 초기 설정 반영
        }
    }

    public void UpdateAttackItemCount()
    {
        if (playerMoveAbility != null)
        {
            AttackItemTextUI.text = $"X {playerMoveAbility.AttackItemCount}";  // 공격 아이템 카운트 업데이트
        }
    }

    public void UpdateKeyItemCount()
    {
        if (playerMoveAbility != null)
        {
            KeyItemTextUI.text = $"{playerMoveAbility.KeyItemCount}/{playerMoveAbility.MaxKeyItemCount}";
        }
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