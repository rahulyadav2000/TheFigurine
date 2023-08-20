using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    public GameObject keyBindings;
    public GameObject settings;
    public GameObject optionsMenu;

    public Toggle screenToggle, syncToggle;

    // list for the resolutions 
    public List<int> widths = new List<int>() { 568, 960, 1280, 1920 };
    public List<int> heights = new List<int>() { 320, 540, 800, 1080 };

    private int index;

    public TextMeshProUGUI resolution;
    // Start is called before the first frame update
    void Start()
    {
        if (keyBindings != null)
        {
            keyBindings.SetActive(false);
        }
        if (settings != null)
        {
            settings.SetActive(true);
        }

        // Set fullscreen toggle to match current fullscreen state.
        screenToggle.isOn = Screen.fullScreen;

        // Set vSync toggle to match current vSync state.
        if (QualitySettings.vSyncCount == 0)
        {
            syncToggle.isOn = false;
        }
        else
        {
            syncToggle.isOn = true;
        }
    }


    public void KeyBindingsMenu()
    {
        if (keyBindings != null)
        {
            keyBindings.SetActive(true);
            settings.SetActive(false);
        }
    }

    public void SettingMenu()
    {
        if(settings != null)
        {
            settings.SetActive(true);
            keyBindings.SetActive(false);
        }
    }

    public void Apply() // function to apply the changed settings
    {
        if (syncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        int width = widths[index];
        int height = heights[index];
        Screen.SetResolution(width, height, screenToggle.isOn);
    }

    public void LeftButton()  // function to move to the previous resoluton
    {
        index--;
        if (index < 0)
        {
            index = 0;
        }
        ResTextUpdate();
    }
    public void RightButton()   // function to move to the next resoluton
    {
        index++;
        if (index > widths.Count - 1)
        {
            index = widths.Count - 1;
        }
        ResTextUpdate();
    }

    public void ResTextUpdate()
    {
        resolution.text = widths[index].ToString() + " X " + heights[index].ToString();
    }

    public void Close()
    {
        if(optionsMenu != null)
        {
            optionsMenu.SetActive(false);
        }
    }
}
