using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rotation_Dir { UnColoc = -1, Clock = 1 }
public sealed class SpinThorn_Pivot : MonoBehaviour
{
    [SerializeField]
    private float m_rotationSpeed;

    [SerializeField]
    private Rotation_Dir m_rotationDir;

    private float m_sign;

    private void Awake()
    {
        m_sign = (int)m_rotationDir;
    }

    private void Update()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + m_rotationSpeed * m_sign * Time.deltaTime, 0);
    }
}
