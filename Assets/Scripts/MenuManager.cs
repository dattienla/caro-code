using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManeger : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject playModePanel;
    [SerializeField] private GameObject playerVsPlayerPanel;

    // Start is called before the first frame update
    void Start()
    {
        SetPanel("MainMenuPanel");
    }
    public void SetPanel(string panelName)
    {
        if (panelName == "MainMenuPanel")
        {
            mainMenuPanel.SetActive(true);
            playModePanel.SetActive(false);
            playerVsPlayerPanel.SetActive(false);
        }
        else if(panelName == "PlayModePanel")
        {
            mainMenuPanel.SetActive(false);
            playModePanel.SetActive(true);
            playerVsPlayerPanel.SetActive(false);
        }
        else if (panelName == "PlayerVsPlayerPanel")
        {
            mainMenuPanel.SetActive(false);
            playModePanel.SetActive(false);
            playerVsPlayerPanel.SetActive(true);
        }
    }
    public void PlayVsComputer()
    {
        SceneManager.LoadScene("GameScene3");
    }
    
    public void PlayNormalGame()
    {
        SceneManager.LoadScene("GameScene1");
    }
    public void PlayTimeRushGame()
    {
        SceneManager.LoadScene("GameScene2");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
