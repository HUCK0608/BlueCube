using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLibrary
{
    private static int m_ignoreLM_Bullet_Player = (-1) - ((1 << 8) | (1 << 11));
    public static int IgonoreLM_Bullet_Player { get { return m_ignoreLM_Bullet_Player; } }
}
