using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TerrianManager : MonoBehaviour
{
    private int m_terrianAmount;

    private void Awake()
    {
        InitTerrian();
    }

    private void InitTerrian()
    {
        m_terrianAmount = transform.childCount;
    }
}
