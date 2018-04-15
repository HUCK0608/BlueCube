﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_PutType { Put, Throw }
public abstract class Interaction_Put : MonoBehaviour
{
    // pickPut
    private Interaction_PickPut m_pickPut;

    // 반투명 오브젝트
    [SerializeField]
    private GameObject m_fadeObject;
    private Material m_fadeObjectMaterial;

    // 놓기 타입
    [SerializeField]
    private E_PutType m_putType;
    public E_PutType PutType { get { return m_putType; } }

    // 최대 놓을 수 있는 거리
    [SerializeField]
    protected float m_maxPutDistacne;

    private Vector3 m_putPosition;
    /// <summary>놓을 위치를 반환</summary>
    public Vector3 PutPosition { get { return m_putPosition; } }

    protected bool m_isCanPut;
    /// <summary>현재 놓을 위치에 놓을 수 있을경우 true를 반환</summary>
    public bool IsCanPut { get { return m_isCanPut; } }

    protected bool m_isPutEnd;
    /// <summary>놓기 로직이 끝났을 경우 true를 반환</summary>
    public bool IsPutEnd { get { return m_isPutEnd; } }

    protected virtual void Awake()
    {
        m_pickPut = GetComponent<Interaction_PickPut>();

        m_fadeObjectMaterial = m_fadeObject.GetComponent<MeshRenderer>().material;
    }

    /// <summary>오브젝트 놓기</summary>
    public virtual void Put() { }
    /// <summary>놓을 위치를 반환. 놓을 수 없는 위치일경우 (1, 1, 1)를 반환</summary>
    protected abstract Vector3 GetPutPosition();

    /// <summary>이 스크립트의 로직을 실행</summary>
    public void StartPutRogic()
    {
        StartCoroutine(PutRogic());
    }

    private IEnumerator PutRogic()
    {
        // 오브젝트를 잡고 있는 동안에만 실행
        while(m_pickPut.IsPick)
        {
            // 놓을 위치를 가져옴
            Vector3 putPosition = GetPutPosition();
            
            // 놓을 위치가 놓을 수 있는 위치일 경우 저장
            if (!putPosition.Equals(Vector3.one))
            {
                m_putPosition = putPosition;

                // 놓을 수 있는 위치인데 반투명 오브젝트가 비활성화 되어있다면 활성화 시켜줌
                if (!m_fadeObject.activeSelf)
                    m_fadeObject.SetActive(true);
            }

            CheckCanPut();
            DrawFadeObject();

            yield return null;
        }

        m_fadeObject.SetActive(false);
    }

    // 저장된 놓을 수 있는 위치에 놓을 수 있는지 체크
    private void CheckCanPut()
    {
        Vector3 thisPositionXZ = transform.position;
        thisPositionXZ.y = m_putPosition.y;

        // 놓을 위치의 높이가 제한 높이를 초과하지 않고 던질 거리가 최대 제한거리를 벗어나지 않을경우 던질 수 있다고 설정
        if (m_putPosition.y < PlayerManager.Instance.Hand.PickObjectPoint.position.y && Vector3.Distance(thisPositionXZ, m_putPosition) < m_maxPutDistacne)
            m_isCanPut = true;
        else
            m_isCanPut = false;
    }

    // 반투명 오브젝트를 그려줌
    private void DrawFadeObject()
    {
        m_fadeObject.transform.position = m_putPosition;

        Color newColor;

        if (m_isCanPut)
            newColor = Color.black;
        else
            newColor = Color.red;

        newColor.a = m_fadeObjectMaterial.color.a;
        m_fadeObjectMaterial.color = newColor;
    }
}
