using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] Toggle fullscreenObject;

    [SerializeField] TMP_Dropdown selectPlayersComponent;
    [SerializeField] TMP_Dropdown resolutionComponent;

    bool fullscreen;

    Resolution[] resolutions;

    public void FullScreenToogle()
    {
        Toggle toggle = fullscreenObject.GetComponent<Toggle>();
        Screen.fullScreen = toggle.isOn;
    }

    public void ChangeResolution()
    {
        int i = resolutionComponent.value;
        Screen.SetResolution(resolutions[i].width, resolutions[i].height, Screen.fullScreen, resolutions[i].refreshRate);
    }


    void Start()
    {
        List<TMP_Dropdown.OptionData> availableResolutions = new();

        Resolution currentRes = Screen.currentResolution;
        int currentResId = 0;
        int i = 0;

        resolutions = Screen.resolutions;

        // Print and add the resolutions
        foreach (var res in resolutions)
        {
            if((res.width == currentRes.width) && (res.height == currentRes.height) && (res.refreshRate == currentRes.refreshRate))
            {
                currentResId = i;
            }

            TMP_Dropdown.OptionData item = new(res.width + "x" + res.height + "  " + res.refreshRate + "hz");
            availableResolutions.Add(item);

            i++;
        }

        resolutionComponent.ClearOptions();
        resolutionComponent.AddOptions(availableResolutions);

        Toggle toggle = fullscreenObject.GetComponent<Toggle>();
        toggle.isOn = Screen.fullScreen;

        resolutionComponent.SetValueWithoutNotify(currentResId);
        resolutionComponent.RefreshShownValue();
    }

    public void ChangeNumberOfPlayers()
    {
        int value = selectPlayersComponent.value + 1;
        GameState.SetNumberOfPlayers(value);
    }

    // I don't know how else to do it
    public void PlayFirstScene()
    {
        SceneManager.LoadScene("FirstMap");
    }

    public void PlaySecondScene()
    {
        //SceneManager.LoadScene("SecondMap");
    }


    public void ExitPressed()
    {
        Application.Quit();
    }
}
