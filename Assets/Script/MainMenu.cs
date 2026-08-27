using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private enum GameMode
    {
        Destruction,
        Explore
    }

    [Header("Panels")]
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject modeSelectPanel;
    [SerializeField] private GameObject stageSelectPanel;

    [Header("Stage Buttons")]
    [SerializeField] private Button classroom1Button;
    [SerializeField] private Button classroom2Button;

    private GameMode selectedMode;

    private void Start()
    {
        ShowTitle();
    }

    public void ShowTitle()
    {
        titlePanel.SetActive(true);
        modeSelectPanel.SetActive(false);
        stageSelectPanel.SetActive(false);
    }

    public void ShowModeSelect()
    {
        titlePanel.SetActive(false);
        modeSelectPanel.SetActive(true);
        stageSelectPanel.SetActive(false);
    }

    public void SelectDestructionMode()
    {
        selectedMode = GameMode.Destruction;

        ShowStageSelect();

        classroom1Button.interactable = true;

        // Classroom2の破壊モードは未実装
        classroom2Button.interactable = false;
    }

    public void SelectExploreMode()
    {
        selectedMode = GameMode.Explore;

        ShowStageSelect();

        classroom1Button.interactable = true;
        classroom2Button.interactable = true;
    }

    private void ShowStageSelect()
    {
        titlePanel.SetActive(false);
        modeSelectPanel.SetActive(false);
        stageSelectPanel.SetActive(true);
    }

    public void SelectClassroom1()
    {
        if (selectedMode == GameMode.Destruction)
        {
            SceneManager.LoadScene("Classroom1");
        }
        else
        {
            SceneManager.LoadScene("Classroom1Explore");
        }
    }

    public void SelectClassroom2()
    {
        if (selectedMode == GameMode.Destruction)
        {
            return;
        }

        SceneManager.LoadScene("Classroom2");
    }

    public void BackToModeSelect()
    {
        ShowModeSelect();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}