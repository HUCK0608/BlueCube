using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLibrary
{
    // Material 부분
    private static Material m_material_Default = Resources.Load("Materials/WorldObject/WorldObject_Default_Material") as Material;
    public static Material Material_Default { get { return m_material_Default; } }

    private static Material m_material_CanChange = Resources.Load("Materials/WorldObject/WorldObject_CanChange_Material") as Material;
    public static Material Material_CanChange { get { return m_material_CanChange; } }

    private static Material m_material_Blue = Resources.Load("Materials/Dumy3") as Material;
    public static Material Material_Blue { get { return m_material_Blue; } }

    private static Material m_material_Red = Resources.Load("Materials/Dumy2") as Material;
    public static Material Material_Red { get { return m_material_Red; } }

    private static Material m_material_Block = Resources.Load("Materials/WorldObject/WorldObject_Block_Material") as Material;
    public static Material Material_Block { get { return m_material_Block; } }

    // layerMask 부분

    private static int m_layerMask_Player = LayerMask.NameToLayer("Player");
    public static int LayerMask_Player { get { return m_layerMask_Player.Shift(); } }

    private static int m_layerMask_Bullet = LayerMask.NameToLayer("Bullet");
    public static int LayerMask_Bullet { get { return m_layerMask_Bullet.Shift(); } }

    private static int m_layerMask_IgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
    public static int LayerMask_IgnoreRaycast { get { return m_layerMask_IgnoreRaycast.Shift(); } }

    private static int m_layerMask_BackgroundCollision = LayerMask.NameToLayer("Background Collision");
    public static int LayerMask_BackgroundCollision { get { return m_layerMask_BackgroundCollision.Shift(); } }

    private static int m_layerMask_BackgroundTrigger = LayerMask.NameToLayer("Background Trigger");
    public static int LayerMask_BackgroundTrigger { get { return m_layerMask_BackgroundTrigger.Shift(); } }

    private static int m_layerMask_CanPushWay = LayerMask.NameToLayer("CanPushWay");
    public static int LayerMask_CanPushWay { get { return m_layerMask_CanPushWay.Shift(); } }

    private static int m_layerMask_InteractionPush = LayerMask.NameToLayer("Interaction Push");
    public static int LayerMask_InteractionPush { get { return m_layerMask_InteractionPush.Shift(); } }

    private static int m_layerMask_InteractionPickPut = LayerMask.NameToLayer("Interaction PickPut");
    public static int LayerMask_InteractionPickPut { get { return m_layerMask_InteractionPickPut.Shift(); } }

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

    private static string m_string_Damage = "Damage";
    public static string String_Damage { get { return m_string_Damage; } }

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

    /// <summary>시점변환중이거나 시점변환 준비중이면 true를 반환</summary>
    public static bool Bool_IsPlayerStop
    {
        get
        {
            return PlayerManager.Instance.IsViewChange ||
                     PlayerManager.Instance.IsViewChangeReady
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

    /// <summary>position위치의 피벗을 반환</summary>
    public static Vector3 GetGamePivot(this Vector3 position)
    {
        Vector3 normalizedPosition = position;
        
        // 각 좌표 내림
        normalizedPosition.x = Mathf.Floor(normalizedPosition.x);
        normalizedPosition.y = Mathf.Floor(normalizedPosition.y);
        normalizedPosition.z = Mathf.Floor(normalizedPosition.z);

        float one = 1f;
        float two = 2f;

        // 각 좌표에 2로 나눈 나머지의 절대값이 1일 경우 1을 더해줌
        // y는 1을 빼줌
        if (Mathf.Abs((normalizedPosition.x % two)).Equals(one))
            normalizedPosition.x += one;
        if (Mathf.Abs((normalizedPosition.y % two)).Equals(one))
            normalizedPosition.y += one;
        if (Mathf.Abs((normalizedPosition.z % two)).Equals(one))
            normalizedPosition.z += one;

        return normalizedPosition;
    }

    // 확장 메서드 부분
    public static int Shift(this int value)
    {
        return (1 << value);
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

    private static RaycastHit2D m_hit2D;

    /// <summary>파라미터 속성으로 2D레이를 쏴서 무언가 충돌하면 true를 반환</summary>
    public static bool Raycast2D(Vector2 origin, Vector2 direction, float maxDistance, int layerMask)
    {
        m_hit2D = Physics2D.Raycast(origin, direction, maxDistance, layerMask);

        if (m_hit2D.collider != null)
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
