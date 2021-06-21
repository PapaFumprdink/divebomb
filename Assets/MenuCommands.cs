using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable Objects/Menu Commands")]
public sealed class MenuCommands : ScriptableObject
{
    [SerializeField] private SceneList m_SceneList;
    [SerializeField] private AudioMixer m_Mixer;
    [SerializeField] private string m_MasterFaderParameter;

    private Settings m_Settings;

    private void OnEnable()
    {
        m_Settings = Settings.LoadSettings();
        ApplyGraphicsSettings();
    }

    public void StartGame ()
    {
        SceneManager.LoadScene(m_SceneList.GameSceneIndex);
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene(m_SceneList.OptionsSceneIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(m_SceneList.MenuSceneIndex);
    }

    public void OpenControls ()
    {
        SceneManager.LoadScene(m_SceneList.ControlsSceneIndex);
    }

    public void QuitGame ()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetXResolution (int newX)
    {
        m_Settings.resolutionX = newX;
    }

    public void SetXResolution(string newXString)
    {
        if (int.TryParse(newXString, out int newX))
        {
            SetXResolution(newX);
        }
    }

    public void SetYResolution(int newY)
    {
        m_Settings.resolutionY = newY;
    }

    public void SetYResolution(string newYString)
    {
        if (int.TryParse(newYString, out int newY))
        {
            SetYResolution(newY);
        }
    }

    public void SetFullscreenMode (int newMode)
    {
        m_Settings.fullscreenMode = newMode;
    }

    public void ApplyGraphicsSettings ()
    {
        Screen.SetResolution(m_Settings.resolutionX, m_Settings.resolutionY, (FullScreenMode)m_Settings.fullscreenMode);
    }

    public void SetAudioVolume (float percent)
    {
        m_Settings.mainVolume = percent;
        if (m_Mixer) m_Mixer.SetFloat(m_MasterFaderParameter, percent);
    }

    public void SaveSettings ()
    {
        m_Settings.SaveSettings();
    }

    public void UpdateUI (TMP_InputField xRes, TMP_InputField yRes, TMP_Dropdown fullscreenMode, Slider volumeSlider)
    {
        xRes.text = m_Settings.resolutionX.ToString();
        yRes.text = m_Settings.resolutionY.ToString();
        fullscreenMode.value = m_Settings.fullscreenMode;

        volumeSlider.value = m_Settings.mainVolume;
    }
}

[System.Serializable]
class Settings
{
    const string PlayerDataStorePath = "/Save Data/";
    const string SettingsFileName = "/Settings.json";

    public int resolutionX, resolutionY;
    public int fullscreenMode;
    public float mainVolume;

    public Settings ()
    {
        resolutionX = 1920;
        resolutionY = 1080;
        fullscreenMode = 3;
        mainVolume = 1f;
    }

    public void SaveSettings ()
    {
        if (!Directory.Exists(Application.persistentDataPath + PlayerDataStorePath))
        {
            Directory.CreateDirectory(Application.persistentDataPath + PlayerDataStorePath);
        }

        string jsonData = JsonUtility.ToJson(this);
        File.WriteAllText(Application.persistentDataPath + PlayerDataStorePath + SettingsFileName, jsonData);
    }

    public static Settings LoadSettings ()
    {
        if (File.Exists(Application.persistentDataPath + PlayerDataStorePath + SettingsFileName))
        {
            string jsonData = File.ReadAllText(Application.persistentDataPath + PlayerDataStorePath + SettingsFileName);
            Settings settings = JsonUtility.FromJson<Settings>(jsonData);
            return settings;
        }

        return new Settings();
    }
}
