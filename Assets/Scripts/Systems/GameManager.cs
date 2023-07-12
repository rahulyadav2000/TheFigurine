using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject winScene;
    public bool isWinning;

    private void Start()
    {
        winScene = GameObject.FindGameObjectWithTag("WinScene");
        if(winScene != null)
            winScene.SetActive(false);
    }

    private void Update()
    {
        WinningState();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();     
    }

    public void WinningState()
    {
        if(GameData.figurineAmount == 1 && isWinning)
        {
            winScene.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }
}
