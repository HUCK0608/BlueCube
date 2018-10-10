using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CheckBlock : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_checkPoints;
    private int m_checkPointCount;

    private bool m_isBlock;
    public bool IsBlock { get { return m_isBlock; } }

    private void Awake()
    {
        m_checkPointCount = m_checkPoints.Count;
    }

    public void Check()
    {
        m_isBlock = false;

        int layerMask = (-1) - (GameLibrary.LayerMask_Bullet |
                                     GameLibrary.LayerMask_BackgroundTrigger |
                                     GameLibrary.LayerMask_BackgroundCollision |
                                     GameLibrary.LayerMask_IgnoreRaycast |
                                     GameLibrary.LayerMask_Player |
                                     GameLibrary.LayerMask_Switch |
                                     GameLibrary.LayerMask_Ladder);

        float distance = PlayerManager.Instance.Skill.ChangeViewRect.SizeZ;

        RaycastHit hit;

        for (int i = 0; i < m_checkPointCount; i++)
        {
            if (GameLibrary.Raycast3D(m_checkPoints[i].position, Vector3.forward, out hit, distance, layerMask))
            {
                WorldObject blockWorldObject = hit.transform.GetComponentInParent<WorldObject>();

                if (!blockWorldObject.IsIncludeChangeViewRect)
                    break;

                blockWorldObject.SetMaterial(E_WorldObject_ShaderType.Block);
                m_isBlock = true;
                break;
            }
        }
    }
}
