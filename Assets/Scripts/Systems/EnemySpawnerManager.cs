using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager Instance;
    public GameObject enemySpanwer;
    public GameObject enemySpanwer2;
    public GameObject enemySpanwer3;
    public GameObject enemySpanwer4;
    public GameObject enemySpanwer5;
    public GameObject enemySpanwer6;
    public GameObject enemySpanwer7;

    public bool isEnemySpanwer1 { get; set; }
    public bool isEnemySpanwer2 { get; set; }
    public bool isEnemySpanwer3 { get; set; }
    public bool isEnemySpanwer4 { get; set; }


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
            }
            
            if(enemySpanwer3 != null && isEnemySpanwer2)
            {
                enemySpanwer3.SetActive(false);
            }
            
            if(enemySpanwer4 != null && !isEnemySpanwer2)
            {
                enemySpanwer4.SetActive(false);
            }
            
            if(enemySpanwer5 != null && isEnemySpanwer3)
            {
                enemySpanwer5.SetActive(false);
            }
            
            if(enemySpanwer6 != null && isEnemySpanwer4)
            {
                enemySpanwer6.SetActive(false);
            }
            
            if(enemySpanwer7 != null && !isEnemySpanwer4)
            {
                enemySpanwer7.SetActive(false);
            }

            Player.instance.isGameObj = false;
        }
    }
}
