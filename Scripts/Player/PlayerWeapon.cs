﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerWeapon : MonoBehaviour
{
    // 파이어볼 매니저
    [SerializeField]
    private BulletBundle m_fireballBundle;

    // 2D 플레이어 (바라보는 방향을 구하기 위해 필요)
    private Player2D m_player2D;

    // 발사 위치
    private Transform m_fireBallMuzzle2D;
    private Transform m_fireBallMuzzle3D;

    // 플레이어 매니저
    private PlayerManager m_manager;

    private void Awake()
    {
        m_manager = GetComponent<PlayerManager>();

        m_player2D = transform.Find("2D").GetComponent<Player2D>();

        m_fireBallMuzzle2D = transform.Find("2D").Find("FireBallMuzzle");
        m_fireBallMuzzle3D = transform.Find("3D").Find("FireBallMuzzle");
    }

    private void Update()
    {
        ShootFireBall();
    }

    // 파이어볼 발사
    public void ShootFireBall()
    {
        // 불 발사 키를 눌렀을 경우
        if(Input.GetKeyDown(m_manager.ShootFireKey))
        {
            // 2D에서 발사
            if(GameManager.Instance.ViewType == E_ViewType.View2D)
            {
                ShootFireBall2D();
            }
            // 3D에서 발사
            else if(GameManager.Instance.ViewType == E_ViewType.View3D)
            {
                ShootFireBall3D();
            }
        }
    }

    // 2D에서 발사
    private void ShootFireBall2D()
    {
        Vector3 direction = Vector3.right * (1 * (int)m_player2D.LookDirection);

        m_fireballBundle.ShootBullet(m_fireBallMuzzle2D.position, direction.normalized);
    }

    // 3D에서 발사
    private void ShootFireBall3D()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        // 총알레이어를 무시하는 레이어
        int layerMask = (-1) - ((1 << 8) | (1 << 11));

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 direction = hit.point - m_fireBallMuzzle3D.position;
            m_fireballBundle.ShootBullet(m_fireBallMuzzle3D.position, direction.normalized);
        }
    }
}
