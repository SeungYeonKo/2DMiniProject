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
                player.Heal(1); 
                break;
            case ItemType.Attack:
                player.AddAttackItem();                
                break;
            case ItemType.Key:

                break;
        }
    }
}
