﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_LookDirection2D { Right = 1, Left = -1 }

public sealed class Player2D : Player
{
    // 리지드바디
    Rigidbody2D m_rigidbody2D;

    Collider2D m_collider2D;


    RaycastHit2D m_hit2D;
    int m_layerMask;
    Vector2 m_upPos;

    // 캐릭터가 바라보는 방향
    private E_LookDirection2D m_lookDirection;
    public E_LookDirection2D LookDirection { get { return m_lookDirection; } }

    protected override void Awake()
    {
        base.Awake();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_collider2D = GetComponent<Collider2D>();
        m_lookDirection = E_LookDirection2D.Right;

        m_layerMask = (-1) - ((1 << 8) | (1 << 11));
        m_upPos = new Vector2(0, 0.05f + m_collider2D.bounds.extents.y);
    }

    private void Update()
    {
        Move();
        Jump();
    }

    // 2D 이동
    private void Move()
    {
        // 키보드 입력
        float move = Input.GetAxis("Horizontal") * Manager.Stat.MoveSpeed;

        // 중력을 사용중이지 않을 때
        if (!Manager.UseGravity)
        {
            // 이동키를 누를경우 중력 사용
            if (move != 0)
                Manager.UseGravity = true;
        }

        // 바라보는 방향 변경해주기
        if (m_lookDirection != E_LookDirection2D.Right && move > 0)
        {
            m_lookDirection = E_LookDirection2D.Right;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (m_lookDirection != E_LookDirection2D.Left && move < 0)
        {
            m_lookDirection = E_LookDirection2D.Left;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // 이동벡터 구하기
        Vector3 movement = Vector3.right * move;

        // 중력을 사용한다면 중력적용
        if (Manager.UseGravity)
        {
            // 점프 상태이거나 땅이 아니면 중력 적용
            if (Manager.IsJumping || !Manager.IsGrounded)
            {
                float nextVelocity = m_rigidbody2D.velocity.y + Manager.Stat.Gravity * Time.deltaTime;
                movement.y = nextVelocity;
            }
        }

        if(!Manager.IsJumping && Manager.IsGrounded)
        {
            m_hit2D = Physics2D.Raycast(transform.position, Vector2.down, 2f, m_layerMask);
            transform.position = m_hit2D.point + m_upPos;
        }

        // 이동
        m_rigidbody2D.velocity = movement;
    }

    // 점프
    private void Jump()
    {
        // 스페이스바를 누르고 땅이고 점프상태가 아닐 때 점프
        if(Input.GetKeyDown(KeyCode.Space) && Manager.IsGrounded && !Manager.IsJumping)
        {
            // 중력사용을 하지 않을 때 점프를 할 경우 중력사용
            if (!Manager.UseGravity)
                Manager.UseGravity = true;

            m_rigidbody2D.AddForce(Vector2.up * Manager.Stat.JumpPower);
            Manager.IsGrounded = false;
            Manager.IsJumping = true;
        }
    }
}
