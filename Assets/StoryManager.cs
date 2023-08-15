using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public void OnEnable()
    {
        SceneManager.LoadScene(2);
    }
}
