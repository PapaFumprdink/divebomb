using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
[DisallowMultipleComponent]
public sealed class PointsDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text m_PointsDisplay;

    public int Points { set => m_PointsDisplay.text = value.ToString(); }
}
