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
        // 회전
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + m_spinThorn.ThornSpinSpeed * m_spinDir * Time.deltaTime);
    }
}
