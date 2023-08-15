using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject inventoryUI;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public bool isWinning;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if(inventoryUI  != null) 
            inventoryUI.SetActive(false);
        if(pauseMenu != null) 
            pauseMenu.SetActive(false);

        if(optionsMenu != null) 
            optionsMenu.SetActive(false);
    }

    private void Update()
    {
        if(isWinning)
        {
            WinningState();
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
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();     
    }

    public void WinningState()
    {
        if(Player.instance.isFinalFigurineCollected)
        {
            SceneManager.LoadScene(4);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1f;
        GameData.health = 100;
    }

    public void LoseScreenEnabler()
    {
        SceneManager.LoadScene(5);
    }

    public void MenuScreen()
    {
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void TogglePauseMenu(bool isActive)
    {
        if(pauseMenu != null)
        {
            pauseMenu.SetActive(isActive);
            Time.timeScale = 0.0f;
        }
        if(!isActive)
        {
            Time.timeScale = 1.0f;
        }
    }

    public void OptionMenu()
    {
        if(optionsMenu != null)
        {
            optionsMenu.SetActive(true);
        }
    }

    public void LoadGame()
    {
        GameData data = LoadSaveSystem.LoadGameData();
        if(Player.instance != null)
        {
            Player.instance.arrow.arrowAmount = data.GetArrows();
            Spawner.instance.figurineIndex = data.GetFigurineAmount();

            Vector3 position;
            position.x = data.playerPos[0];
            position.y = data.playerPos[1];
            position.z = data.playerPos[2];
            Player.instance.transform.position = position;
        }
    }

    public void SaveGame()
    {
        LoadSaveSystem.SaveGameData(Player.instance, Player.instance.arrow);
    }

    public void LoadGameData()
    {
        SceneManager.LoadScene(2);
        GameData.health = 100;
        LoadGame();
    }
}
