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

    private void Awake()
    {
        m_playerManager = GetComponent<PlayerManager>();

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
        if(Input.GetKeyDown(m_playerManager.ShootFireKey))
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
        int playerLook = (int)m_playerManager.Player2D_S.LookDirection;

        Vector3 shootDirection = Vector3.right * playerLook;

        m_fireballBundle.ShootBullet(m_fireBallMuzzle2D.position, shootDirection.normalized);
    }

    // 3D에서 발사
    private void ShootFireBall3D()
    {
        Vector3 shootDirection = m_playerManager.Player3D_GO.transform.forward;

        m_fireballBundle.ShootBullet(m_fireBallMuzzle3D.position, shootDirection.normalized);
    }
}
