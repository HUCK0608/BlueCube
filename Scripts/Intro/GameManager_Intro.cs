using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager_Intro : MonoBehaviour
{
    private void Awake()
    {
        // 판과 코기의 충돌을 없앰
        Physics2D.IgnoreLayerCollision(GameLibrary.LayerMask_CorgiIntro, GameLibrary.LayerMask_PanIntro);
    }
}
