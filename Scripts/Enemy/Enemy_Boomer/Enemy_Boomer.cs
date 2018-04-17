using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStat_Boomer))]
public sealed class Enemy_Boomer : MonoBehaviour
{
    private EnemyStat_Boomer m_stat;

    private float m_addTime;

    private void Awake()
    {
        m_stat = GetComponent<EnemyStat_Boomer>();
    }

    private void Update()
    {
        Rotate();
        Shoot();
    }

    private void Rotate()
    {
        Vector3 playerPosition = PlayerManager.Instance.Player3D_Object.transform.position;
        playerPosition.y = transform.position.y;

        Vector3 directionToPlayer = playerPosition - transform.position;
        directionToPlayer = directionToPlayer.normalized;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), m_stat.RotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        m_addTime += Time.deltaTime;

        if(m_addTime >= m_stat.ShootDelay)
        {
            EnemyProjectileManager.Instance.UseProjectile(E_EnemyProjectile.Bomb, m_stat.ShootPosition.position, PlayerManager.Instance.Player3D_Object.transform.position);
            m_addTime = 0f;
        }
    }
}
