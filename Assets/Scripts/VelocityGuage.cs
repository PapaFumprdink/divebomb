using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class VelocityGuage : MonoBehaviour
{
    const int SpeedSamples = 20;

    [SerializeField] private Rigidbody2D m_Target;
    [SerializeField] private Image m_Indicator;
    [SerializeField] private AnimationCurve m_ScaleCurve;

    [Space]
    [SerializeField] private TMPro.TMP_Text m_SpeedDisplay;
    [SerializeField] private string m_SpeedDisplayText;

    private List<float> m_SpeedSamples;

    private void Awake()
    {
        m_SpeedSamples = new List<float>();
    }

    private void Update()
    {
        // Only execute if we have a target and an indicator.
        if (m_Target && m_Indicator)
        {
            // Store the speed and angle because magnitude and trig functions are expensive and for brevity.
            float speed = m_Target.velocity.magnitude;
            float angle = Mathf.Atan2(m_Target.velocity.y, m_Target.velocity.x) * Mathf.Rad2Deg;

            //Add the current speed to the sample list and remove an entry from the start if we have enough samples.
            m_SpeedSamples.Add(speed);
            if (m_SpeedSamples.Count > SpeedSamples)
            {
                m_SpeedSamples.RemoveAt(0);
            }

            // Average out the speed - an average is needed because the fluctuations in speed are too fast to be readable.
            float averageSpeed = 0f;
            foreach (float speedSample in m_SpeedSamples)
            {
                averageSpeed += speedSample;
            }
            averageSpeed /= SpeedSamples;

            // Scale the indicator to the average speed and rotate it to the velocity's direction
            m_Indicator.transform.localScale = new Vector3(m_ScaleCurve.Evaluate(averageSpeed), 1f, 1f);
            m_Indicator.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Set the display text to the average speed to 1 decimal place.
            m_SpeedDisplay.text = string.Format(m_SpeedDisplayText, Mathf.Round(averageSpeed * 10f) / 10f);
        }
    }
}
