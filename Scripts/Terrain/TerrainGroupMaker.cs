using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public sealed class TerrainGroupMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject m_framePrefab;

    [SerializeField]
    private int m_x, m_y, m_z;

    // 간격
    private static float m_interval = 2f;

    private void Update()
    {
        Vector3 createPosition = Vector3.zero;

        for (int z = 0; z < m_z; z++)
        {
            for (int y = 0; y < m_y; y++)
            {
                for (int x = 0; x < m_x; x++)
                {
                    Debug.Log(createPosition);
                    GameObject newTerrain = Instantiate(m_framePrefab) as GameObject;
                    newTerrain.transform.parent = transform;
                    newTerrain.transform.localPosition = createPosition;

                    createPosition.x += m_interval;
                }

                createPosition.x = 0;
                createPosition.y += m_interval;
            }
            createPosition.y = 0;
            createPosition.z += m_interval;
        }
    }
}
