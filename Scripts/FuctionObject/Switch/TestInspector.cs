using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestInspector : MonoBehaviour
{
    [SerializeField]
    private string dddd;
    public string D { get { return dddd; } set { dddd = value; } }

    private void Awake()
    {
        Debug.Log(dddd);
    }

    private void Update()
    {
        Debug.Log(dddd);
    }
}
