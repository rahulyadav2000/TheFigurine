using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager Instance;
    public GameObject enemySpanwer;
    public GameObject enemySpanwer2;

    public bool isEnemySpanwer1 { get; set; }
    private void Awake()
    {
        Instance = this;
    }
  

    // Update is called once per frame
    void Update()
    {
        
        if(Player.instance.isGameObj)
        {
            if(enemySpanwer != null && isEnemySpanwer1)
            {
                enemySpanwer.SetActive(false);
            }
            if(enemySpanwer2 != null && !isEnemySpanwer1)
            {
                enemySpanwer2.SetActive(false);
                //Player.instance.isGameObj = false;
            }
            Player.instance.isGameObj = false;
        }
    }
}
