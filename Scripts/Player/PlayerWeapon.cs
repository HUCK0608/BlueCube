using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerWeapon : MonoBehaviour
{
    // 플레이어 매니저
    private PlayerManager_Old m_playerManager;

    // 파이어볼 매니저
    [SerializeField]
    private BulletBundle m_fireballBundle;

    // 발사 위치
    private Transform m_fireBallMuzzle2D;
    private Transform m_fireBallMuzzle3D;

    private void Awake()
    {
        m_playerManager = GetComponent<PlayerManager_Old>();

		m_fireBallMuzzle2D = m_playerManager.Player2D_GO.transform.Find("FireBallMuzzle");
		m_fireBallMuzzle3D = m_playerManager.Player3D_GO.transform.Find("FireBallMuzzle");

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

        Vector3 shootDirection = Vector3.right * playerLook;

        m_fireballBundle.ShootBullet(m_fireBallMuzzle2D.position, shootDirection.normalized);
    }

    // 3D에서 발사
    private void ShootFireBall3D()
    {
        // 법선이 y양의 방향을 보고있고 플레이어위치에 평면을 생성
        Plane plane = new Plane(Vector3.up, m_playerManager.Player3D_GO.transform.position);

        // 마우스 위치의 광선 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 충돌된 거리를 담을 변수
        float rayDistance;

        // 충돌 위치를 담을 변수
        Vector3 hitPoint = Vector3.zero;

        // 평면에서 광선 발사
        if (plane.Raycast(ray, out rayDistance))
        {
            // 충돌 위치 구하기
            hitPoint = ray.GetPoint(rayDistance);
        }

        // 발사방향 구하기
        Vector3 shootDirection = hitPoint - m_fireBallMuzzle3D.position;
        // y의 방향 없애기
        shootDirection.y = 0f;

        m_fireballBundle.ShootBullet(m_fireBallMuzzle3D.position, shootDirection.normalized);
    }
}
