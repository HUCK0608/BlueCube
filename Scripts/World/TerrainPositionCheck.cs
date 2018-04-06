using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public sealed class TerrainPositionCheck : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_child;

    private void Update()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        if (x % 2 != 0 || y % 2 != 0 || z % 2 != 0)
        {
            //if (x % 2 != 0)
            //    x = Mathf.Floor(x) + 1f;

            //if (y % 2 != 0)
            //    y = Mathf.Floor(y) + 1f;

            //if (z % 2 != 0)
            //    z = Mathf.Floor(z) + 1f;

            Debug.LogError("피벗이 맞지않거나 소수점이 존재!! 피벗은 짝수여야함!", gameObject);

            //transform.position = new Vector3(x, y, z);
        }

        for(int i = 0; i < 2; i++)
        {
            if (!m_child[i].localPosition.Equals(Vector3.zero))
                Debug.LogError("자식 좌표 이상!", gameObject);
        }
    }
}
