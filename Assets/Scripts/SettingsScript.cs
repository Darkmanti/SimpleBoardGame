using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] Toggle fullscreenObject;
    [SerializeField] Toggle vsyncObject;

    [SerializeField] TMP_Dropdown selectPlayersComponent;
    [SerializeField] TMP_Dropdown resolutionComponent;

    [SerializeField] AudioSource clickSound;

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

        // Add the resolutions
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

        Toggle fullscreenToggle = fullscreenObject.GetComponent<Toggle>();
        fullscreenToggle.isOn = Screen.fullScreen;

        vsyncObject.GetComponent<Toggle>().isOn = true;
        QualitySettings.vSyncCount = 1;

        resolutionComponent.SetValueWithoutNotify(currentResId);
        resolutionComponent.RefreshShownValue();

        clickSound.gameObject.SetActive(true);
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


    public void ExitPressed()
    {
        Application.Quit();
    }

    public void PlayClickSound()
    {
        clickSound.Play();
    }

    public void ToggleVSync()
    {
        Toggle toggle = vsyncObject.GetComponent<Toggle>();
        bool flag = toggle.isOn;

        if(flag)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }
}
