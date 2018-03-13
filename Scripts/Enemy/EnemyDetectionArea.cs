using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyDetectionArea : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_size;

    private void OnDrawGizmos()
    {
        Color defaultColor = Gizmos.color;
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, m_size);

        Gizmos.color = defaultColor;
    }

    public bool CheckDetected(Vector3 target)
    {
        bool checkX = false;
        bool checkY = false;
        bool checkZ = false;

        float halfX = m_size.x * 0.5f;
        float halfY = m_size.y * 0.5f;
        float halfZ = m_size.z * 0.5f;

        Vector3 thisPosition = transform.position;

        if (target.x >= thisPosition.x - halfX && target.x <= thisPosition.x + halfX)
            checkX = true;

        if (target.y >= thisPosition.y - halfY && target.y <= thisPosition.y + halfY)
            checkY = true;

        if (target.z >= thisPosition.z - halfZ && target.z <= thisPosition.z + halfZ)
            checkZ = true;

        if (checkX && checkY && checkZ)
            return true;

        return false;
    }
}
