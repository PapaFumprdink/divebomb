using UnityEngine;
using TMPro;
using UnityEngine.UI;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class SettingsMenu : MonoBehaviour
{
    [SerializeField] private MenuCommands m_MenuCommands;

    [Space]
    [SerializeField] private TMP_InputField m_XResField;
    [SerializeField] private TMP_InputField m_YResField;
    [SerializeField] private TMP_Dropdown m_FullscreenModeDropdown;
    [SerializeField] private Slider m_VolumeSlider;

    private void Start()
    {
        m_MenuCommands.UpdateUI(m_XResField, m_YResField, m_FullscreenModeDropdown, m_VolumeSlider);
    }
}
