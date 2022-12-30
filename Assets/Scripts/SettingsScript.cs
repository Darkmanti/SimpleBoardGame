using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    Transform resolutionObject;
    Transform fullscreenObject;
    Transform selectPlayersObject;

    TMP_Dropdown selectPlayersComponent;
    TMP_Dropdown resolutionComponent;

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


    // Start is called before the first frame update
    void Start()
    {
        Transform settingsMenu = transform.Find("SettingsMenu");
        resolutionObject = settingsMenu.Find("Resolution");
        fullscreenObject = settingsMenu.Find("Fullscreen");

        Transform sceneMenu = transform.Find("SceneMenu");
        selectPlayersObject = sceneMenu.Find("SelectPlayers");

        resolutionComponent = resolutionObject.GetComponent<TMP_Dropdown>();
        selectPlayersComponent = selectPlayersObject.GetComponent<TMP_Dropdown>();

        List<TMP_Dropdown.OptionData> availableResolutions = new();

        resolutions = Screen.resolutions;

        // Print and add the resolutions
        foreach (var res in resolutions)
        {
            //Debug.Log(res.width + "x" + res.height + " : " + res.refreshRate);
            TMP_Dropdown.OptionData item = new(res.width + "x" + res.height + "  " + res.refreshRate + "hz");
            availableResolutions.Add(item);
        }

        resolutionComponent.ClearOptions();
        resolutionComponent.AddOptions(availableResolutions);
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
