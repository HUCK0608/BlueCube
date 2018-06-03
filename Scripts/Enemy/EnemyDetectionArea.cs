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

    public bool IsDectected()
    {
        if (PlayerManager.Instance.CurrentView.Equals(E_ViewType.View3D))
            return CheckDetected3D();
        else
            return CheckDetected2D();
    }

    private bool CheckDetected3D()
    {
        Vector3 target = PlayerManager.Instance.Player3D_Object.transform.position;

        bool checkX = false;
        bool checkY = false;
        bool checkZ = false;

        float half = 0.5f;

        float halfX = m_size.x * half;
        float halfY = m_size.y * half;
        float halfZ = m_size.z * half;

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

    private bool CheckDetected2D()
    {
        Vector3 target = PlayerManager.Instance.Player2D_Object.transform.position;

        bool checkX = false;
        bool checkY = false;

        float half = 0.5f;

        float halfX = m_size.x * half;
        float halfY = m_size.y * half;

        Vector3 thisPosition = transform.position;

        if (target.x >= thisPosition.x - halfX && target.x <= thisPosition.x + halfX)
            checkX = true;

        if (target.y >= thisPosition.y - halfY && target.y <= thisPosition.y + halfY)
            checkY = true;

        if (checkX && checkY)
            return true;

        return false;
    }
}
