using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject winScene;
    public GameObject loseScreen;
    public bool isWinning;

    public GameObject inventoryUI;

    private void Start()
    {
        winScene = GameObject.FindGameObjectWithTag("WinScene");
        if(winScene != null)
            winScene.SetActive(false);

        inventoryUI.SetActive(false);
        loseScreen.SetActive(false);
    }

    private void Update()
    {
        WinningState();

        /*        if(GameData.health == 0)
                {
                    Time.timeScale = 0.0f;
                }
                else
                {
                    Time.timeScale = 1.0f;
                }*/
        if(Player.instance.isDead)
        {
            Invoke(nameof(LoseScreenEnabler), 2.5f);
        }
    }

    private void LateUpdate()
    {
        if (Player.instance.isInventoryActive)
        {
            inventoryUI.SetActive(true);
        }
        else
        {
            inventoryUI.SetActive(false);
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
        loseScreen.SetActive(true);
        Time.timeScale = 0;
    }
}
