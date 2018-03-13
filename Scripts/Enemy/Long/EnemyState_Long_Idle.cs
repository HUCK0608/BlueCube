using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyState_Long_Idle : EnemyState
{
    private Transform player;

    [SerializeField]
    private EnemyDetectionArea m_detectionArea;

    protected override void Awake()
    {
        base.Awake();

        player = GameManager.Instance.PlayerManager.Player3D_GO.transform;
    }

    public override void InitState()
    {
        
    }

    private void Update()
    {
        Debug.Log(m_detectionArea.CheckDetected(player.position));
    }

    public override void EndState()
    {
    }
}
