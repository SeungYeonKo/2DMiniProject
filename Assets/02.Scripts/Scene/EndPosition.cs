using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndPosition : MonoBehaviour
{
    public PlayerMoveAbility playerMoveAbility; // PlayerMoveAbility 컴포넌트에 대한 참조
    public GameObject ClearEffect;
    public Text NoKeyText;


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 키 아이템 수가 충분한지 확인
            if (playerMoveAbility.KeyItemCount >= playerMoveAbility.MaxKeyItemCount)
            {
                if (ClearEffect != null)
                {
                    GameObject effect = Instantiate(ClearEffect, transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
                }
                // 3초 후 다음 스테이지 로드
                StartCoroutine(LoadNextStageAfterDelay());
            }
            else
            {
                StartCoroutine(DisplayNoKeyText());
            }
        }
    }
    IEnumerator DisplayNoKeyText()
    {
        NoKeyText.gameObject.SetActive(true); 
        yield return new WaitForSeconds(1); 
        NoKeyText.gameObject.SetActive(false); 
    }

    IEnumerator LoadNextStageAfterDelay()
    {
        yield return new WaitForSeconds(3); // 3초 지연
        LoadNextStage();
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
                SceneManager.LoadScene("EndingScene");
                break;
        }
    }
}