using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSystem : MonoBehaviour
{
    [SerializeField] public int arrowAmount;

    private void Start()
    {
        arrowAmount = GameData.arrow;
    }
    // Start is called before the first frame update
    public int IncreaseArrowAmount(int amount) // function to increase the arrow amount
    {
        arrowAmount += amount;
        GameData.arrow = arrowAmount;
        return arrowAmount;
    }

    public int ReduceArrowAmount()  //// function to reduce the arrow amount
    {
        arrowAmount--;
        GameData.arrow = arrowAmount;
        return arrowAmount;
    }

    public int GetArrowAmount() // function to return the arrow amount
    {
        arrowAmount = GameData.arrow;
        return arrowAmount;
    }
}
