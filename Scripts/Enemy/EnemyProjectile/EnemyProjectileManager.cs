using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_EnemyProjectile { Bomb }
public sealed class EnemyProjectileManager : MonoBehaviour
{
    private static EnemyProjectileManager m_instance;
    public static EnemyProjectileManager Instance { get { return m_instance; } }

    [SerializeField]
    private GameObject m_prefab_Projectile_Bomb;

    // 투사체 모음
    private List<EnemyProjectile> m_projectiles_Bomb;
    private int m_projectile_BombCount;

    private void Awake()
    {
        m_instance = this;

        m_projectiles_Bomb = new List<EnemyProjectile>();
    }

    /// <summary>투사체를 사용한다</summary>
    public void UseProjectile(E_EnemyProjectile projectileType, Vector3 origin, Vector3 destination)
    {
        EnemyProjectile projectile = null;

        if(projectileType.Equals(E_EnemyProjectile.Bomb))
        {
            if (!m_projectile_BombCount.Equals(0))
            {
                for(int i = 0; i < m_projectile_BombCount; i++)
                {
                    if (m_projectiles_Bomb[i].IsCanUse)
                    {
                        projectile = m_projectiles_Bomb[i];
                        break;
                    }
                }
            }
        }

        // 투사체가 없을경우 새로운 투사체를 만든다.
        if (projectile == null)
            projectile = CreateProjectile(projectileType);

        // 투사체 사용
        projectile.UseProjectile(origin, destination);
    }

    /// <summary>새로운 투사체를 만든다</summary>
    private EnemyProjectile CreateProjectile(E_EnemyProjectile projectileType)
    {
        EnemyProjectile newProjectile = null;

        if(projectileType.Equals(E_EnemyProjectile.Bomb))
        {
            GameObject newProjectileObject = Instantiate(m_prefab_Projectile_Bomb);

            newProjectileObject.transform.parent = transform;
            newProjectileObject.transform.position = transform.position;

            newProjectile = newProjectileObject.GetComponent<EnemyProjectile>();

            m_projectiles_Bomb.Add(newProjectile);
            m_projectile_BombCount++;
        }

        return newProjectile;
    }
}
