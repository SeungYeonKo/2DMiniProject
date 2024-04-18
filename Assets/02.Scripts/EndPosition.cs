using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPosition : MonoBehaviour
{
    public PlayerMoveAbility playerMoveAbility; // PlayerMoveAbility 컴포넌트에 대한 참조

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 키 아이템 수가 충분한지 확인
            if (playerMoveAbility.KeyItemCount >= playerMoveAbility.MaxKeyItemCount)
            {
                // 다음 스테이지로 이동
                LoadNextStage();
            }
            else
            {
                Debug.Log("키 아이템이 부족합니다!");
            }
        }
    }

    void LoadNextStage()
    {
        // 현재 스테이지를 기반으로 다음 씬 로드
        switch (playerMoveAbility.CurrentStage.StageType)
        {
            case StageType.Stage1:
                SceneManager.LoadScene("Stage2");
                break;
            case StageType.Stage2:
                SceneManager.LoadScene("Stage3");
                break;
            case StageType.Stage3:
                SceneManager.LoadScene("EndingScene");
                break;
        }
    }
}