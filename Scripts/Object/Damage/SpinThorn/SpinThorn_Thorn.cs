using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SpinThorn_Thorn : MonoBehaviour
{
    // 부모 스크립트
    private SpinThorn m_spinThorn;

    // 회전방향
    private int m_spinDir;

    private void Awake()
    {
        m_spinThorn = GetComponentInParent<SpinThorn>();

        // 회전방향 설정
        m_spinDir = (int)m_spinThorn.ThornSpinDir;
    }

    private void Update()
    {
        Rotation();
    }

    // 회전
    private void Rotation()
    {
        // 시점변환중이거나 탐지모드이거나 2D이면 리턴
        if (GameLibrary.Bool_IsGameStop_Old)
            return;

        // 2D일경우 멈춤
        if (GameManager.Instance.PlayerManager.Skill_CV.ViewType.Equals(E_ViewType.View2D))
            return;

        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + m_spinThorn.ThornSpinSpeed * m_spinDir * Time.deltaTime);
    }
}
