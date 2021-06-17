using UnityEngine;
using TMPro;
using System;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text[] m_DisplayTexts;
    [SerializeField] private TMP_Text m_HighScoreText;
    [SerializeField] private Transform m_AnimationParent;
    [SerializeField] private AnimationCurve m_ScaleOnScore;

    private static float m_LastScoreTime;

    public static int Score { get; private set; }

    public static void AddScoreStatic (int points)
    {
        Score += points;
        m_LastScoreTime = Time.time;
    }

    public void AddScore (int points)
    {
        AddScoreStatic(points);
    }

    private void Update()
    {
        foreach (TMP_Text textObject in m_DisplayTexts)
        {
            textObject.text = $"{Score} pts";
        }

        m_HighScoreText.text = "High Scores yet to be Implemented";

        m_AnimationParent.localScale = Vector3.one * m_ScaleOnScore.Evaluate(Time.time - m_LastScoreTime);
    }

    public static void ResetScore()
    {
        Score = 0;
    }
}
