using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerWeapon : MonoBehaviour
{
    private PlayerManager m_playerManager;

    [SerializeField]
    private int m_fireBallDamage;

    [SerializeField]
    private float m_fireBallSpeed;

    [SerializeField]
    private float m_fireBallDurationTime;

    [SerializeField]
    private float m_reloadTime;
    public float ReloadTime { get { return m_reloadTime; } }

    private bool m_canUse;
    public bool CanUse { get { return m_canUse; } }

    private void Awake()
    {
        m_playerManager = GetComponent<PlayerManager>();

        m_canUse = true;
    }

    /// <summary>3D방식의 파이어볼 발사</summary>
    public void ShootFireBall3D(Vector3 direction)
    {
        Vector3 playerPosition = m_playerManager.Player3D_Object.transform.position;

        BulletManager.Instance.ShootBullet(E_BulletType.PlayerFireBall, m_fireBallDamage, playerPosition, direction, m_fireBallSpeed, m_fireBallDurationTime);
        StartCoroutine(Reload());
    }

    /// <summary>2D방식의 파이어볼 발사</summary>
    public void ShootFireBall2D(Vector3 direction)
    {
        Vector3 playerPosition = m_playerManager.Player2D_Object.transform.position;

        BulletManager.Instance.ShootBullet(E_BulletType.PlayerFireBall, m_fireBallDamage, playerPosition, direction, m_fireBallSpeed, m_fireBallDurationTime);
        StartCoroutine(Reload());
    }

    // 재장전
    private IEnumerator Reload()
    {
        m_canUse = false;

        float addTime = 0f;

        while(true)
        {
            addTime += Time.deltaTime;

            if (addTime >= m_reloadTime)
                break;

            yield return null;
        }

        m_canUse = true;
    }
}
