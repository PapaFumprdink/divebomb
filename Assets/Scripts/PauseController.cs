using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class PauseController : MonoBehaviour
{
    public static event System.Action OnGameReload;
    public static event System.Action OnGameQuit;

    [SerializeField] private UnityEvent m_OnPause;
    [SerializeField] private UnityEvent m_OnResume;

    [Space]
    [SerializeField] private SceneList m_SceneList;

    private Controls m_Controls;
    private bool m_Paused;

    private void Awake()
    {
        // Create a seperate control instance and bind needed controls.
        m_Controls = new Controls();
        m_Controls.General.Cancel.performed += (ctx) => TogglePause();

        // Unpause to hide the menus.
        Unpause();
    }

    private void OnEnable()
    {
        m_Controls.Enable();
    }

    private void OnDisable()
    {
        m_Controls.Disable();
    }

    public void Pause ()
    {
        m_Paused = true;
        UpdatePauseMenu();
    }

    public void Unpause()
    {
        m_Paused = false;
        UpdatePauseMenu();
    }

    public void TogglePause()
    {
        m_Paused = !m_Paused;
        UpdatePauseMenu();
    }

    private void UpdatePauseMenu()
    {
        // Update the timescale. If we are paused, set timescale to 0 to effectivley 'pause' the game.
        Time.timeScale = m_Paused ? 0f : 1f;

        if (m_Paused) m_OnPause?.Invoke();
        else m_OnResume?.Invoke();
    }

    public void ReloadGame ()
    {
        OnGameReload?.Invoke();

        SceneManager.LoadScene(m_SceneList.GameSceneIndex);
    }

    public void QuitGame ()
    {
        OnGameQuit?.Invoke();

        SceneManager.LoadScene(m_SceneList.GameSceneIndex);
    }
}
