using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Door_Pivot { Right = -1, Left = 1 }

public sealed class Door : MonoBehaviour
{
    private Activate m_activate;

    private Transform m_door2D;
    private Transform m_door3D;

    [SerializeField]
    private Door_Pivot m_pivot;

    [SerializeField]
    private float m_rotationSpeed;

    private void Awake()
    {
        m_activate = GetComponent<Activate>();

        m_door2D = transform.Find("2D");
        m_door3D = transform.Find("3D");

        SetPivot();

        StartCoroutine(CheckActivate());
    }

    // 피벗 설정
    private void SetPivot()
    {
        float halfWidth = m_door3D.GetComponent<MeshFilter>().mesh.bounds.extents.x * m_door3D.localScale.x;

        transform.position += new Vector3(halfWidth * (int)m_pivot * -1, 0, 0);

        m_door2D.localPosition = new Vector3(halfWidth * (int)m_pivot, 0, 0);
        m_door3D.localPosition = new Vector3(halfWidth * (int)m_pivot, 0, 0);
    }

    private IEnumerator CheckActivate()
    {
        while(true)
        {
            if (m_activate.IsActivate)
                break;

            yield return null;
        }

        StartCoroutine(Open());
    }

    private IEnumerator Open()
    {
        Quaternion rotation = new Quaternion();

        if (m_pivot == Door_Pivot.Right)
            rotation = Quaternion.Euler(0, 90, 0);
        else if (m_pivot == Door_Pivot.Left)
            rotation = Quaternion.Euler(0, -90, 0);

        while (true)
        {

            transform.rotation =  Quaternion.RotateTowards(transform.rotation, rotation, m_rotationSpeed);

            if (rotation == transform.rotation)
                break;

            yield return new WaitForFixedUpdate();
        }

        m_door2D.GetComponent<Collider2D>().isTrigger = true;
    }
}
