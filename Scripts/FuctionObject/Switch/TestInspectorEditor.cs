using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestInspector))]
public class TestInspectorEditor : Editor
{
    string ss;

    public override void OnInspectorGUI()
    {
        PropertyInfo[] propertyInfo = typeof(TestInspector).GetProperties();

        int num = 0;

        GameObject[] selectionObjects = Selection.gameObjects;

        TestInspector selectObjectInComponent = null;

        for (int i = 0; i < selectionObjects.Length; i++)
            selectObjectInComponent = selectionObjects[i].GetComponent<TestInspector>();

        for (int i = 0; i < propertyInfo.Length; i++)
        {
            if (propertyInfo[i].ReflectedType.Equals(typeof(TestInspector)))
            {
                if (propertyInfo[i].PropertyType.Equals(typeof(System.String)))
                {
                    ss = EditorGUILayout.TextField(propertyInfo[i].Name, ss);
                    propertyInfo[i].SetValue(selectObjectInComponent, ss, null);
                    EditorGUILayout.TextField(this.ToString());
                    EditorGUILayout.TextField("DeclaringType : " + propertyInfo[i].DeclaringType);
                    EditorGUILayout.TextField("MemberType : " + propertyInfo[i].MemberType);
                    EditorGUILayout.TextField("PropertyType : " + propertyInfo[i].PropertyType);
                    EditorGUILayout.TextField("ReflectedType : " + propertyInfo[i].ReflectedType);
                }
            }
        }

        //PropertyInfo propertyInfo1 = typeof(TestInspector).GetProperty("D");

        //EditorGUILayout.TextField("DeclaringType : " + propertyInfo1.DeclaringType);
        //EditorGUILayout.TextField("MemberType : " + propertyInfo1.MemberType);
        //EditorGUILayout.TextField("PropertyType : " + propertyInfo1.PropertyType);
        //EditorGUILayout.TextField("ReflectedType : " + propertyInfo1.ReflectedType);
    }
}
