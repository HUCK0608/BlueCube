using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestInspector))]
public sealed class TestInspectorEditor : Editor
{
    private TestInspector targetScript;

    private void Awake()
    {
        targetScript = target as TestInspector;
    }

    public override void OnInspectorGUI()
    {
        GameObject[] selectionObjects = Selection.gameObjects;
        for (int i = 0; i < selectionObjects.Length; i++)
            targetScript = selectionObjects[i].GetComponent<TestInspector>();

        PropertyInfo[] propertyInfo = targetScript.GetType().GetProperties();

        for (int i = 0; i < propertyInfo.Length; i++)
        {
            if (propertyInfo[i].ReflectedType.Equals(typeof(TestInspector)))
            {
                if (propertyInfo[i].PropertyType.Equals(typeof(System.String)) && (propertyInfo[i].Name == ("name") || propertyInfo[i].Name == ("tag") ? false : true))
                {
                    string value = EditorGUILayout.TextField(propertyInfo[i].Name, propertyInfo[i].GetValue(targetScript, null) as string);
                    propertyInfo[i].SetValue(targetScript, value, null);
                    EditorGUILayout.TextField(value);
                    EditorGUILayout.TextField(propertyInfo[i].ReflectedType.ToString());
                    EditorGUILayout.TextField(propertyInfo[i].MemberType.ToString());
                    EditorGUILayout.TextField(propertyInfo[i].PropertyType.ToString());
                    EditorGUILayout.TextField(propertyInfo[i].ReflectedType.ToString());
                }
            }
        }

        if (GUI.changed)
            EditorUtility.SetDirty(targetScript);
    }
}
