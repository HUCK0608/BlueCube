using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerWeapon : MonoBehaviour
{
    // 플레이어 매니저
    private PlayerManager m_playerManager;

    // 파이어볼 매니저
    [SerializeField]
    private BulletBundle m_fireballBundle;

    // 발사 위치
    private Transform m_fireBallMuzzle2D;
    private Transform m_fireBallMuzzle3D;

    private Ray m_ray;
    private RaycastHit m_hit;
    private int m_layerMask;
    private Vector3 m_shootDirection;

    private void Awake()
    {
        m_playerManager = GetComponent<PlayerManager>();

		m_fireBallMuzzle2D = m_playerManager.Player2D_GO.transform.Find("FireBallMuzzle");
		m_fireBallMuzzle3D = m_playerManager.Player3D_GO.transform.Find("FireBallMuzzle");

        m_layerMask = (-1) - ((1 << 8) | (1 << 11));
    }

    private void Update()
    {
        ShootFireBall();
    }

    // 파이어볼 발사
    public void ShootFireBall()
    {
        // 시점변환중이거나 탐지모드일경우 리턴
        if (GameLibrary.Bool_IsCO)
            return;

        // 불 발사 키를 눌렀을 경우
        if(Input.GetKeyDown(m_playerManager.ShootFireKey))
        {
            // 2D에서 발사
            if(GameManager.Instance.PlayerManager.Skill_CV.ViewType == E_ViewType.View2D)
            {
                ShootFireBall2D();
            }
            // 3D에서 발사
            else if(GameManager.Instance.PlayerManager.Skill_CV.ViewType == E_ViewType.View3D)
            {
                ShootFireBall3D();
            }
        }
    }

    // 2D에서 발사
    private void ShootFireBall2D()
    {
        int playerLook = (int)m_playerManager.Player2D_S.LookDirection;

        m_shootDirection = Vector3.right * playerLook;

        m_fireballBundle.ShootBullet(m_fireBallMuzzle2D.position, m_shootDirection.normalized);
    }

    // 3D에서 발사
    private void ShootFireBall3D()
    {
        m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(m_ray, out m_hit, Mathf.Infinity, m_layerMask))
        {
            m_shootDirection = m_hit.point - m_fireBallMuzzle3D.position;
            m_shootDirection.y = 0;
        }

        m_fireballBundle.ShootBullet(m_fireBallMuzzle3D.position, m_shootDirection.normalized);
    }
}
