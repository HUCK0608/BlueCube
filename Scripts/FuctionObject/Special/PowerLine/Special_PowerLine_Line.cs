using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Special_PowerLine_Line : MonoBehaviour
{
    [System.Serializable]
    private struct LineDirection
    {
        [SerializeField]
        private bool m_isUp;
        [SerializeField]
        private bool m_isRight;
        [SerializeField]
        private bool m_isLeft;
        [SerializeField]
        private bool m_isDown;
    }

    [SerializeField]
    private LineDirection m_lineDirection;

    private void Update()
    {
        //if(!Application.isPlaying)
    }

    private void ChangeLineDirection()
    {

    }
}
