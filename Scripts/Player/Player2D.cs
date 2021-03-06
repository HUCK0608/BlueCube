﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_LookDirection2D { Right = 1, Left = -1 }

public sealed class Player2D : Player
{
    // 애니메이터
    private Animator m_animator;

    // 리지드바디
    private Rigidbody2D m_rigidbody2D;

    // 캐릭터가 바라보는 방향
    private E_LookDirection2D m_lookDirection;
    public E_LookDirection2D LookDirection { get { return m_lookDirection; } }

    // 땅 체크를 위한 변수들
    private RaycastHit2D m_hit;
    private Vector2 m_rayOrigin;
    private Vector2 m_rayDirection;
    private Vector2 m_onGroundUpPos;
    private Vector3 m_onGroundPos;
    private float m_boundsX;
    private float m_rayOriginY;
    private int m_ignoreLayerMask;
    private float m_rayDistance;

    protected override void Awake()
    {
        base.Awake();
        m_animator = GetComponent<Animator>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_lookDirection = E_LookDirection2D.Right;

        InitCheckGround();
    }
    
    private void InitCheckGround()
    {
        m_rayDirection = Vector2.down;

        // 콜라이더 박스 x 크기
        m_boundsX = GetComponent<Collider2D>().bounds.extents.x;

        // 레이의 시작 y 위치
        m_rayOriginY = 0.1f;

        // 플레이어를 올릴 땅 위 위치
        m_onGroundUpPos = new Vector3(0, 0.05f);

        // 플레이어하고 총알을 무시하는 레이어마스크
        m_ignoreLayerMask = (-1) - ((1 << 8) | (1 << 11));

        // 레이 거리
        m_rayDistance = 0.16f;
    }

    private void Update()
    {
        CheckGround();
        Move();
        Jump();
        SetAni();
    }

    private void CheckGround()
    {
        // 2방향 체크
        // 2방향 레이를 쏴서 한곳에 무언가라도 닿으면 땅이라고 알리고 2곳 모두 아무것도 안 닿으면 땅이 아니라고 알림

        // 좌
        m_rayOrigin = transform.position;
        m_rayOrigin.x -= m_boundsX;
        m_rayOrigin.y += m_rayOriginY;

        m_hit = Physics2D.Raycast(m_rayOrigin, m_rayDirection, m_rayDistance, m_ignoreLayerMask);

        if (m_hit.collider != null)
            m_playerManager.IsGrounded = true;
        else
            m_playerManager.IsGrounded = false;

        // 우
        m_rayOrigin = transform.position;
        m_rayOrigin.x += m_boundsX;
        m_rayOrigin.y += m_rayOriginY;

        m_hit = Physics2D.Raycast(m_rayOrigin, m_rayDirection, m_rayDistance, m_ignoreLayerMask);

        if (m_hit.collider != null)
            m_playerManager.IsGrounded = true;
        else if (!m_playerManager.IsGrounded)
            m_playerManager.IsGrounded = false;
    }

    // 2D 이동
    private void Move()
    {
        // 현재 시점변환 중이라면 캐릭터 정지
        if(m_playerManager.Skill_CV.IsChanging)
        {
            m_rigidbody2D.velocity = Vector2.zero;
            return;
        }

        // 키보드 입력
        float move = Input.GetAxis("Horizontal") * m_playerManager.Stat.ForwardMoveSpeed;

        // 애니메이션 설정변수
        if (move == 0)
            m_playerManager.IsRunning = false;
        else
            m_playerManager.IsRunning = true;

        // 중력을 사용중이지 않을 때
        if (!m_playerManager.UseGravity)
        {
            // 이동키를 누를경우 중력 사용
            if (move != 0)
                m_playerManager.UseGravity = true;
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

        //// 땅이라면 땅위치에서 살짝 띄어줌
        if (m_playerManager.IsGrounded)
        {
            // 점프중일경우 점프중이 아니라고 알림
            if (m_playerManager.IsJumping)
                m_playerManager.IsJumping = false;

            m_rayOrigin = transform.position;
            m_rayOrigin.y += m_rayOriginY;

            m_hit = Physics2D.Raycast(m_rayOrigin, m_rayDirection, m_rayDistance, m_ignoreLayerMask);

            if (m_hit.collider != null)
            {
                m_onGroundPos = transform.position;
                m_onGroundPos.y = m_hit.point.y + m_onGroundUpPos.y;

                transform.position = m_onGroundPos;
            }
        }
        // 땅이 아니라면 중력 적용
        else
        {
            float nextVelocity = m_rigidbody2D.velocity.y + m_playerManager.Stat.Gravity * Time.deltaTime;
            movement.y = nextVelocity;
        }

        // 이동
        m_rigidbody2D.velocity = movement;
    }

    // 점프
    private void Jump()
    {
        // 스페이스바를 누르고 땅일경우 점프
        if(Input.GetKeyDown(KeyCode.Space) && m_playerManager.IsGrounded && !m_playerManager.IsJumping)
        {
            m_rigidbody2D.AddForce(Vector2.up * m_playerManager.Stat.JumpPower);
            m_playerManager.IsJumping = true;
        }
    }

    // 애니메이션 설정
    private void SetAni()
    {
        m_animator.SetBool("IsRunning", m_playerManager.IsRunning);
        m_animator.SetBool("IsJumping", m_playerManager.IsJumping);
    }
}
