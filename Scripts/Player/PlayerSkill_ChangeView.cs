using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerSkill_ChangeView : MonoBehaviour
{
    private PlayerManager m_manager;

    private GameObject m_changeViewRect;

    // 최대 사이즈
    [SerializeField]
    private int m_maxSizeX, m_maxSizeY, m_maxSizeZ;

    // 증가 수치 퍼센트
    [SerializeField]
    private float m_increaseSizePer;

    // 블루큐브 사이즈
    private Vector3 m_blueCubeSize;

    // 증가수치
    private float m_increaseValueX;
    private float m_increaseValueY;
    private float m_increaseValueZ;

    // 증가수치 벡터
    private Vector3 m_increaseValue;

    private bool m_isChnaging;

    private void Awake()
    {
        m_manager = GetComponent<PlayerManager>();

        m_changeViewRect = transform.Find("ChangeViewRect").gameObject;
        m_changeViewRect.SetActive(false);

        m_blueCubeSize = GameManager.Instance.BlueCubeManager.transform.localScale;

        m_increaseValueX = (m_maxSizeX - m_blueCubeSize.x) * m_increaseSizePer * 0.01f;
        m_increaseValueY = (m_maxSizeY - m_blueCubeSize.y) * m_increaseSizePer * 0.01f;
        m_increaseValueZ = (m_maxSizeZ - m_blueCubeSize.z) * m_increaseSizePer * 0.01f;

        m_increaseValue = new Vector3(m_increaseValueX, m_increaseValueY, m_increaseValueZ);
    }

    private void Update()
    {
        // 시점 변경중이 아니고 카메라 변경 키를 눌렀을 때
        if (!m_isChnaging && Input.GetKeyDown(m_manager.ChangeViewKey))
        {
            // 현재 시점이 3D이면 2D로 변경
            if (GameManager.Instance.ViewType == E_ViewType.View3D)
                StartCoroutine(ChangeView2D());
        }
    }

    // 2D로 변경
    private IEnumerator ChangeView2D()
    {
        m_isChnaging = true;
        m_changeViewRect.transform.localScale = m_blueCubeSize;
        m_changeViewRect.transform.position = GameManager.Instance.BlueCubeManager.transform.position;
        m_changeViewRect.SetActive(true);


        while (true)
        {
            m_changeViewRect.transform.localScale += m_increaseValue;

            if (m_changeViewRect.transform.localScale.x >= m_maxSizeX)
                break;

            yield return new WaitForFixedUpdate();
        }

        yield return StartCoroutine(GameManager.Instance.CameraManager.MovingWork3D());


        m_changeViewRect.SetActive(false);
        m_isChnaging = false;

    }
}
