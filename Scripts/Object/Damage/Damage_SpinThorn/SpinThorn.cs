using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rotation_Dir { UnColoc = -1, Clock = 1 }
public sealed  class SpinThorn : MonoBehaviour
{
    [SerializeField]
    private Rotation_Dir m_pivotSpinDir;
    public Rotation_Dir PivotSpinDir { get { return m_pivotSpinDir; } }

    [SerializeField]
    private float m_pivotSpinSpeed;
    public float PivotSpinSpeed { get { return m_pivotSpinSpeed; } }

    [SerializeField]
    private Rotation_Dir m_thornSpinDir;
    public Rotation_Dir ThornSpinDir { get { return m_thornSpinDir; } }

    [SerializeField]
    private float m_thornSpinSpeed;
    public float ThornSpinSpeed { get { return m_thornSpinSpeed; } }
}
