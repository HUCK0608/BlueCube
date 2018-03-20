using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerWeapon : MonoBehaviour
{
    private PlayerManager m_playerManager;

    private void Awake()
    {
        m_playerManager = GetComponent<PlayerManager>();
    }

    public void ShootFireBall3D(Vector3 direction)
    {
        GameManager.Instance.BulletManager.ShootBullet(E_BulletType.PlayerFireBall, 1, transform.position, direction, 0.3f, 3f);
    }
}
