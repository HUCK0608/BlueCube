using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    /// <summary>문이 열렸을 때 로컬 위치</summary>
    private static Vector3 m_doorOpenLocalPosition = new Vector3(0f, -8.0f, 0f);

    private WorldObject m_worldObject;

    /// <summary>열릴 그룹</summary>
    [SerializeField]
    private Transform m_openGroup;

    /// <summary>열리는 속도</summary>
    [SerializeField]
    private float m_openSpeed = 3f;

    protected virtual void Start()
    {
        m_worldObject = GetComponent<WorldObject>();
    }

    /// <summary>문 열기</summary>
    protected void OpenDoor()
    {
        StartCoroutine(OpenDoorLogic());
    }

    /// <summary>문이 열리는 로직</summary>
    private IEnumerator OpenDoorLogic()
    {
        while(true)
        {
            if(!GameLibrary.Bool_IsGameStop(m_worldObject))
                m_openGroup.localPosition = Vector3.MoveTowards(m_openGroup.localPosition, m_doorOpenLocalPosition, m_openSpeed * Time.deltaTime);

            if (m_openGroup.localPosition.Equals(m_doorOpenLocalPosition))
                break;

            yield return null;
        }

        m_openGroup.gameObject.SetActive(false);
    }
}
