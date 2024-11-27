using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour
{
    public Button[] levelButtons; 
    public Color[] buttonColors;  
    public Color deactivatedColor;

    // Start is called before the first frame update
    void Start()
    {
        if (levelButtons == null || levelButtons.Length == 0)
        {
            Debug.LogError("No buttons assigned to levelButtons array!");
            return;
        }

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i;
            levelButtons[index].onClick.AddListener(() => OnLevelButtonClicked(index));
        }
    }

    public void OnLevelButtonClicked(int buttonIndex)
    {
        Debug.Log($"Button {buttonIndex} clicked!");

        if (StageManager.instance != null)
        {
            StageManager.instance.currentStage = buttonIndex;
        }

        SceneManager.LoadScene("Scene2");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StageScene") UpdateButtonsUI();
    }
    void UpdateButtonsUI()
    {
        if (StageManager.instance == null)
        {
            Debug.LogWarning("StageManager instance is null!");
            return;
        }
        int currentStage = StageManager.instance.currentStage;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelButtons[i] != null)
            {
                levelButtons[i].interactable = (i <= currentStage); // 현재 스테이지까지 활성화
                //levelButtons[i].GetComponent<Image>().color = (i <= currentStage) ? buttonColors[i] : deactivatedColor;
            }
        }
    }
}
