using TMPro;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreCounter m_ScoreCounter;

    [Space]
    [SerializeField] private TMP_Text[] m_DisplayTexts;
    [SerializeField] private TMP_Text m_HighScoreText;
    [SerializeField] private Transform m_AnimationParent;
    [SerializeField] private AnimationCurve m_ScaleOnScore;

    private void Update()
    {
        foreach (TMP_Text textObject in m_DisplayTexts)
        {
            textObject.text = $"{m_ScoreCounter.Score} pts";
        }

        int highScoreDifference = m_ScoreCounter.HighScore - m_ScoreCounter.Score;
        if (highScoreDifference > 0) m_HighScoreText.text = $"{highScoreDifference} points to High Score";
        else m_HighScoreText.text = "New High Score!";

        m_AnimationParent.localScale = Vector3.one * m_ScaleOnScore.Evaluate(Time.time - m_ScoreCounter.LastScoreTime);
    }

}
