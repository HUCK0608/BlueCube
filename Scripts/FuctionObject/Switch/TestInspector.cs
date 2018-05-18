using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInspector : MonoBehaviour
{
    public float a;
    public bool b;
    public int c;

    private string dddd;
    public string D { get { return dddd; } set { dddd = value; } }

    private void Awake()
    {
        Debug.Log(dddd);
    }
}
