using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLibrary
{
    // layerMask 부분

    private static int m_ignoreLM_PEE = (-1) - ((1 << 8) | (1 << 11) | (1 << 12));
    /// <summary> Ignore Layer Mask (Player, Enemy, Effect) </summary>
    public static int IgonoreLM_PEE { get { return m_ignoreLM_PEE; } }

    // string 부분

    private static string m_string_Player = "Player";
    public static string String_Player { get { return m_string_Player; } }

    private static string m_string_PlayerAttack = "PlayerAttack";
    public static string String_PlayerAttack { get { return m_string_PlayerAttack; } }

    private static string m_string_PlayerSkill = "PlayerSkill";
    public static string String_PlayerSkill { get { return m_string_PlayerSkill; } }

    private static string m_string_Enemy = "Enemy";
    public static string String_Enemy { get { return m_string_Enemy; } }

    private static string m_string_Effect = "Effect";
    public static string String_Effect { get { return m_string_Effect; } }

    // Enum 부분

    private static E_ViewType m_enum_View2D = E_ViewType.View2D;
    public static E_ViewType Enum_View2D { get { return m_enum_View2D; } }

    private static E_ViewType m_enum_View3D = E_ViewType.View3D;
    public static E_ViewType Enum_View3D { get { return m_enum_View3D; } }

    private static E_LookDirection2D m_enum_LD2D_Left = E_LookDirection2D.Left;
    public static E_LookDirection2D Enum_LD2D_Left { get { return m_enum_LD2D_Left; } }

    private static E_LookDirection2D m_enum_LD2D_Right = E_LookDirection2D.Right;
    public static E_LookDirection2D Enum_LD2D_Right { get { return m_enum_LD2D_Right; } }
}
