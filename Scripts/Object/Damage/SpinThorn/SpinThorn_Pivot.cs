using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SpinThorn_Pivot : MonoBehaviour
{
    // 부모 스크립트
    private SpinThorn m_spinThorn;

    // 회전방향
    private int m_spinDir;

    private void Awake()
    {
        m_spinThorn = GetComponentInParent<SpinThorn>();

        // 회전방향 설정
        m_spinDir = (int)m_spinThorn.PivotSpinDir;
    }

    private void Update()
    {
        Rotation();
    }

    // 회전
    private void Rotation()
    {
        // 시점 변환중이거나 활성화 되지 않았을 경우 리턴
        if (GameManager.Instance.PlayerManager.Skill_CV.IsChanging || !m_spinThorn.WorldObejct.Enabled)
            return;

        // 2D일경우 멈춤
        if (GameManager.Instance.PlayerManager.Skill_CV.ViewType.Equals(E_ViewType.View2D))
            return;

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + m_spinThorn.PivotSpinSpeed * m_spinDir * Time.deltaTime, 0);
    }
}
