using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SpinThorn_Thorn : MonoBehaviour
{
    [SerializeField]
    private float m_rotationSpeed;

    [SerializeField]
    private Rotation_Dir m_rotationDir;

    private int m_sign;

    private void Awake()
    {
        m_sign = (int)m_rotationDir;
    }

    private void Update()
    {
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + m_rotationSpeed * m_sign * Time.deltaTime);
    }
}
