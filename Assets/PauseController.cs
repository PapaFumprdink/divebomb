using System;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerController))]
public sealed class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject m_PauseMenu;

    private PlayerController m_Controller;
    private bool m_Paused;

    private void Awake()
    {
        m_Controller = GetComponent<PlayerController>();

        m_Controller.CancelEvent += OnCancel;
    }

    private void OnCancel()
    {
        TogglePause();
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
        Time.timeScale = m_Paused ? 0f : 1f;
        m_PauseMenu.SetActive(m_PauseMenu);
    }
}
