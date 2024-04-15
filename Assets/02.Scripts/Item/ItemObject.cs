using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemType ItemType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와의 충돌을 감지
        if (other.CompareTag("Player"))
        {
            PlayerMoveAbility player = other.GetComponent<PlayerMoveAbility>();

            if (player != null)
            {
                ApplyEffect(player);
                Destroy(gameObject); // 효과 적용 후 아이템 제거
            }
        }
    }

    private void ApplyEffect(PlayerMoveAbility player)
    {
        switch (ItemType)
        {
            case ItemType.Health:
                player.Heal(1); // 체력 아이템의 경우, 체력을 1만큼 회복
                break;
            case ItemType.Attack:
                // 공격력 향상 로직이 필요하다면 여기에 추가
                break;
            case ItemType.Key:
                // 키 아이템 로직이 필요하다면 여기에 추가
                break;
        }
    }
}
