using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class StartScene : MonoBehaviour
{
    public Button PlayButton;
    public Button ExitButton;

    void Start()
    {
       
        if (PlayButton != null)
        {
            PlayButton.onClick.AddListener(LoadStage1Scene);
        }

        if (ExitButton != null)
        {
            ExitButton.onClick.AddListener(ExitGame);
        }
    }

    void LoadStage1Scene()
    {
        SceneManager.LoadScene("Stage1");
    }


    void ExitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();  // 애플리케이션 종료
#endif
    }
}