using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameObject winScene;
    public GameObject loseScreen;
    public bool isWinning;
    public bool isLoseScreen;

    public GameObject inventoryUI;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if(isWinning)
        {
            winScene = GameObject.FindGameObjectWithTag("WinScene");
        }
        if(winScene != null)
            winScene.SetActive(false);

        if(inventoryUI  != null) 
            inventoryUI.SetActive(false);
        if(loseScreen != null)
            loseScreen.SetActive(false);
    }

    private void Update()
    {
        WinningState();

        if(isLoseScreen)
        {
            if (Player.instance.isDead)
            {
                Invoke(nameof(LoseScreenEnabler), 2.5f);
            }
        }
    }

    public void ToggleInventorySystem(bool isActiveInven)
    {
        if(inventoryUI != null)
        {
            inventoryUI.SetActive(isActiveInven);
        }
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
        GameData.health = 100;
    }

    public void LoseScreenEnabler()
    {
        if(loseScreen != null)
        {
            loseScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
