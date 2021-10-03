using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class OilRig : EnemyBase
{
    [SerializeField] private Transform m_TurretRotor;
    [SerializeField] private Transform m_TurretBase;
    [SerializeField] private float m_Range;

    [Space]
    [SerializeField] private float m_ZDepth;

    [Space]
    [SerializeField] private float m_LRBCooldown;
    [SerializeField] private float m_LRBDuration;

    [Space]
    [SerializeField] private Image m_LRBCooldownSlider;
    [SerializeField] private Image m_LRBFireSlider;

    float m_LastLRBTime;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, m_ZDepth);

        m_LRBCooldownSlider.fillAmount = 0f;
        m_LRBFireSlider.fillAmount = 0f;

        ResetLBRTimer();
    }

    protected override void Update()
    {
        base.Update();

        if (CurrentTarget)
        {
            if ((CurrentTarget.transform.position - transform.position).sqrMagnitude < m_Range * m_Range)
            {
                Fire(0, false);
                ResetLBRTimer();
            }
            else if (Time.time > m_LastLRBTime + m_LRBCooldown + m_LRBDuration)
            {
                StartCoroutine(LRB());
            }

            m_LRBCooldownSlider.fillAmount = (Time.time - m_LastLRBTime) / (m_LRBCooldown);
            m_LRBFireSlider.fillAmount = 0f;

            Quaternion turretOrientation = Quaternion.LookRotation((CurrentTarget.transform.position - m_TurretRotor.position).normalized, Vector3.up);
            m_TurretBase.rotation = Quaternion.Euler(0f, turretOrientation.eulerAngles.y + 90f, 0f);
            m_TurretRotor.rotation = turretOrientation * Quaternion.Euler(0f, 90f, 90f);
        }
    }

    private IEnumerator LRB()
    {
        ResetLBRTimer();

        float time = 0f;
        while (time < m_LRBDuration)
        {
            Fire(1, false);

            m_LRBCooldownSlider.fillAmount = 0f;
            m_LRBFireSlider.fillAmount = (m_LRBDuration - time) / m_LRBDuration;

            time += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_Range);
        Gizmos.color = Color.white;
    }

    private void OnValidate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, m_ZDepth);
    }

    public void ResetLBRTimer()
    {
        m_LastLRBTime = Time.time;
    }
}
