using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerController3D : MonoBehaviour
{
    private PlayerManager m_playerManager;

    private Rigidbody m_rigidbody;

    private Transform m_head;
    private Transform m_body;

    private void Awake()
    {
        m_playerManager = GetComponentInParent<PlayerManager>();

        m_rigidbody = GetComponent<Rigidbody>();

        m_head = transform.Find("Head");
        m_body = transform.Find("Body");
    }


    /// <summary>이동 방향 벡터를 반환. 입력이 없으면 (0, 0, 0)을 반환</summary>
    public Vector3 GetMoveDirection()
    {
        Vector3 moveDirection = Vector3.zero;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveX.Equals(0f) && moveZ.Equals(0f))
            return moveDirection;

        Vector3 rightKeyPos = Vector3.right + Vector3.back;
        Vector3 upKeyPos = Vector3.forward + Vector3.right;

        if (moveX > 0f)
            moveDirection += rightKeyPos;
        if (moveX < 0f)
            moveDirection += -rightKeyPos;
        if (moveZ > 0f)
            moveDirection += upKeyPos;
        if (moveZ < 0f)
            moveDirection += -upKeyPos;

        return moveDirection.normalized;
    }

    /// <summary>moveDirecton * moveSpeed로 Rigidbody.velocity로 이동</summary>
    public void Move(Vector3 moveDirection, float moveSpeed)
    {
        m_rigidbody.velocity = moveDirection * moveSpeed;
    }

    /// <summary>direction방향으로 머리 회전</summary>
    public void RotateHead(Vector3 direction)
    {
        m_head.rotation = Quaternion.LookRotation(direction);
    }

    /// <summary>direction방향으로 몸 회전</summary>
    public void RotateBody(Vector3 direction)
    {
        m_body.rotation = Quaternion.Slerp(m_body.rotation, Quaternion.LookRotation(direction), m_playerManager.Stat.RotationSpeed * Time.deltaTime);
    }

    /// <summary>direction방향으로 머리와 몸 회전</summary>
    public void RotateHeadAndBody(Vector3 direction)
    {
        RotateHead(direction);
        RotateBody(direction);
    }
}
