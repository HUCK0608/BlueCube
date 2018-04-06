using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLibrary
{
    // Material 부분

    private static Material m_material_CanChange = Resources.Load("Shaders/WorldObject/WorldObject_CanChange_Material") as Material;
    public static Material Material_CanChange { get { return m_material_CanChange; } }

    private static Material m_material_blue = Resources.Load("Materials/Dumy3") as Material;
    public static Material Material_Blue { get { return m_material_blue; } }

    // layerMask 부분

    private static int m_layerMask_Igonore_RBP = (-1) - ((1 << 2) | (1 << 8) | (1 << 11));
    /// <summary> Ignore Layer Mask (Respawn, Bullet, Player) </summary>
    public static int LayerMask_Ignore_RBP { get { return m_layerMask_Igonore_RBP; } }

    // string 부분

    private static string m_string_Player = "Player";
    public static string String_Player { get { return m_string_Player; } }

    private static string m_string_PlayerAttack = "PlayerAttack";
    public static string String_PlayerAttack { get { return m_string_PlayerAttack; } }

    private static string m_string_PlayerSkill = "PlayerSkill";
    public static string String_PlayerSkill { get { return m_string_PlayerSkill; } }

    private static string m_string_Enemy = "Enemy";
    public static string String_Enemy { get { return m_string_Enemy; } }

    private static string m_string_EnemyAttack = "EnemyAttack";
    public static string String_EnemyAttack { get { return m_string_EnemyAttack; } }

    private static string m_string_IgnoreTag = "IgnoreTag";
    public static string String_IgnoreTag { get { return m_string_IgnoreTag; } }

    // Bool 부분

    /// <summary>시점변환중이거나 2D일경우 true를 반환</summary>
    public static bool Bool_IsGameStop_Old
    {
        get
        {
            return PlayerManager.Instance.IsViewChange ||
                     PlayerManager.Instance.CurrentView.Equals(E_ViewType.View2D)
                     ? true : false;
        }
    }

    /// <summary>시점변환중이거나 랜더러가 꺼져있을경우 true를 반환</summary>
    public static bool Bool_IsGameStop(WorldObject worldObject)
    {
        return PlayerManager.Instance.IsViewChange ||
                 !worldObject.IsOnRenderer
                 ? true : false;
    }

    /// <summary>시점변환중이거나 시점변환 준비중이거나 관찰시점이면 true를 반환</summary>
    public static bool Bool_IsPlayerStop
    {
        get
        {
            return PlayerManager.Instance.IsViewChange ||
                     PlayerManager.Instance.IsViewChangeReady ||
                     CameraManager.Instance.IsObserve
                     ? true : false;
        }
    }

    /// <summary>플레이어이거나 플레이어공격이거나 플레이어스킬이면 true를 반환</summary>
    public static bool Bool_IsPlayerTag(string tag)
    {
        if (tag.Equals(m_string_Player) || tag.Equals(m_string_PlayerAttack) || tag.Equals(m_string_PlayerSkill))
            return true;

        return false;
    }

    // Function 부분
    public static IEnumerator Timer(float limitTime)
    {
        float addTime = 0f;

        while(true)
        {
            // 게임시간이 멈추지 않았을 경우 실행
            if (!Bool_IsGameStop_Old)
            {
                addTime += Time.deltaTime;
                if (addTime >= limitTime)
                    break;
            }
            yield return null;
        }
    }

    // 레이
    private static Ray m_ray = new Ray();

    /// <summary>파라미터 속성으로 3D레이를 쏴서 무언가 충돌하면 true를 반환</summary>
    public static bool Raycast3D(Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance, int layerMask)
    {
        m_ray.origin = origin;
        m_ray.direction = direction;

        if(Physics.Raycast(m_ray, out hit, maxDistance, layerMask))
            return true;

        return false;
    }

    /// <summary>파라미터 속성으로 3D레이를 쏴서 무언가 충돌하면 true를 반환</summary>
    public static bool Raycast3D(Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
    {
        m_ray.origin = origin;
        m_ray.direction = direction;

        if (Physics.Raycast(m_ray, maxDistance, layerMask))
            return true;

        return false;
    }

    /// <summary>파라미터 속성으로 2D레이를 쏴서 무언가 충돌하면 true를 반환</summary>
    public static bool Raycast2D(Vector2 origin, Vector2 direction, out RaycastHit2D hit, float maxDistance, int layerMask)
    {
        hit = Physics2D.Raycast(origin, direction, maxDistance, layerMask);

        if (hit.collider != null)
            return true;

        return false;
    }
}
