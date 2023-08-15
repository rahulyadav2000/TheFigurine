using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public static float health { get; set; } = 100f;
    public static int arrow { get; set; } = 10;

    public static int figurineAmount { get; set; } = 0;

    public float[] playerPos;

    public GameData(Player player, ArrowSystem arrowSystem)
    {
        arrow = arrowSystem.GetArrowAmount();
        figurineAmount = Spawner.instance.figurineIndex;

        playerPos = new float[3];
        playerPos[0] = player.transform.position.x;
        playerPos[1] = player.transform.position.y;
        playerPos[2] = player.transform.position.z;
    }

    public int GetFigurineAmount()
    {
        return figurineAmount;
    }

    public int GetArrows()
    {
        return arrow;
    }

    public float GetHealth()
    {
        return health;
    }
}
