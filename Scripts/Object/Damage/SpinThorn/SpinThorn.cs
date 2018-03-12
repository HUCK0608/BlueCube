using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_RotationDir { UnColock = -1, Clock = 1 }
public sealed  class SpinThorn : MonoBehaviour
{
    [SerializeField]
    private E_RotationDir m_pivotSpinDir;
    public E_RotationDir PivotSpinDir { get { return m_pivotSpinDir; } }

    [SerializeField]
    private float m_pivotSpinSpeed;
    public float PivotSpinSpeed { get { return m_pivotSpinSpeed; } }

    [SerializeField]
    private E_RotationDir m_thornSpinDir;
    public E_RotationDir ThornSpinDir { get { return m_thornSpinDir; } }

    [SerializeField]
    private float m_thornSpinSpeed;
    public float ThornSpinSpeed { get { return m_thornSpinSpeed; } }

    private WorldObject m_worldObejct;
    public WorldObject WorldObejct { get { return m_worldObejct; } }

    private void Awake()
    {
        m_worldObejct = GetComponent<WorldObject>();
    }
}
