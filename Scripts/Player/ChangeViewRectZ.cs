using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ChangeViewRectZ : MonoBehaviour
{
    private BoxCollider m_collider;

    private void Awake()
    {
        m_collider = GetComponent<BoxCollider>();

        m_collider.enabled = false;
    }

    private void LateUpdate()
    {
        Move();
    }

    public void SetColliderEnable(bool value)
    {
        m_collider.enabled = value;
    }

    private void Move()
    {
        if (!PlayerManager.Instance.IsViewChangeReady)
            return;

        ChangeViewRect changeViewRect = PlayerManager.Instance.Skill.ChangeViewRect;

        Vector3 newPosition = changeViewRect.transform.localPosition;
        newPosition.z += changeViewRect.SizeZ * 0.5f + m_collider.size.z * 0.5f;

        transform.localPosition = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals(GameLibrary.String_Player))
        {
            WorldObject worldObject = other.GetComponentInParent<WorldObject>();

            if(worldObject != null)
            {
                worldObject.IsIncludeChangeVeiwRectZ = true;
            }
        }
    }
}
