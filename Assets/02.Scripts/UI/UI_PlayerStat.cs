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

    public void Refresh()
    {
        AttackItemTextUI.text = $"X {ItemManager.Instance.GetItemCount(ItemType.Attack)}";
        KeyItemTextUI.text = $"{ItemManager.Instance.GetItemCount(ItemType.Key)}/2";
    }
}
