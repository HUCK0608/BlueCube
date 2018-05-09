using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckBlock : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_checkPoints;
    private int m_checkPointCount;

    private void Awake()
    {
        m_checkPointCount = m_checkPoints.Count;
    }

    /// <summary>끼는 곳이 있으면 True를 반환</summary>
    public bool IsBlock()
    {
        int layerMask = (-1) - (GameLibrary.LayerMask_Bullet |
                                     GameLibrary.LayerMask_BackgroundTrigger |
                                     GameLibrary.LayerMask_BackgroundCollision |
                                     GameLibrary.LayerMask_IgnoreRaycast |
                                     GameLibrary.LayerMask_Player |
                                     GameLibrary.LayerMask_Switch);

        float distance = PlayerManager.Instance.Skill.ChangeViewRect.SizeZ;

        RaycastHit hit;

        for(int i = 0; i < m_checkPointCount; i++)
        {
            if (GameLibrary.Raycast3D(m_checkPoints[i].position, Vector3.forward, out hit, distance, layerMask))
            {
                WorldObject blockWorldObject = hit.transform.GetComponentInParent<WorldObject>();

                blockWorldObject.ShowBlock();

                return true;
            }
        }

        return false;
    }
}
