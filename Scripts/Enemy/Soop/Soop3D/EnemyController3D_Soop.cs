using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyController3D_Soop : MonoBehaviour
{
    private EnemyController_Soop m_mainController;

    private Animator m_animator;
    /// <summary>애니메이터</summary>
    public Animator Animator { get { return m_animator; } }

    private void Awake()
    {
        m_mainController = GetComponentInParent<EnemyController_Soop>();

        m_animator = GetComponent<Animator>();
    }

    /// <summary>direction 방향으로 lerp회전을 함</summary>
    public void RotateToDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), m_mainController.Stat.RotationSpeed * Time.deltaTime);
    }
}
