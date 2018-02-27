using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_MaterialType { Default, Change }

public static class GameLibrary
{
    // Material 부분

    private static Material m_material_red = Resources.Load("Materials/Dumy2") as Material;
    public static Material Material_Red { get { return m_material_red; } }

    // layerMask 부분

    private static int m_layerMask_Igonore_BPE = (-1) - ((1 << 8) | (1 << 11) | (1 << 12));
    /// <summary> Ignore Layer Mask (Bullet, Player, Effect) </summary>
    public static int LayerMask_Ignore_BPE { get { return m_layerMask_Igonore_BPE; } }

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

    private static E_MaterialType m_enum_Material_Default = E_MaterialType.Default;
    public static E_MaterialType Enum_Material_Default { get { return m_enum_Material_Default; } }

    private static E_MaterialType m_enum_Material_Change = E_MaterialType.Change;
    public static E_MaterialType Enum_Material_Change { get { return m_enum_Material_Change; } }

    // Function 부분

    /// <summary>시점변환중이거나 2D시점일 경우 true를 반환</summary>
    public static bool IsTimeStop
    {
        get
        {
            return GameManager.Instance.PlayerManager.Skill_CV.IsChanging || 
                   GameManager.Instance.PlayerManager.Skill_CV.ViewType.Equals(Enum_View2D) 
                   ? true : false;
        }
    }
}
