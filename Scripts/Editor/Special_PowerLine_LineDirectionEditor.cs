using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Special_PowerLine_LineDirection))]
[CanEditMultipleObjects]
public sealed class Special_PowerLine_LineDirectionEditor : Editor
{
    private SerializedProperty m_isUpProp;
    private SerializedProperty m_isRightProp;
    private SerializedProperty m_isLeftProp;
    private SerializedProperty m_isDownProp;

    private void OnEnable()
    {
        m_isUpProp = serializedObject.FindProperty("m_isUp");
        m_isRightProp = serializedObject.FindProperty("m_isRight");
        m_isLeftProp = serializedObject.FindProperty("m_isLeft");
        m_isDownProp = serializedObject.FindProperty("m_isDown");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Space(10f);
        if(GUILayout.Button("오브젝트 90도 회전"))
        {
            Transform selectionTransform = Selection.activeTransform;

            selectionTransform.Rotate(Vector3.forward, 90f);

            bool oldUpValue = m_isUpProp.boolValue;
            bool oldRightValue = m_isRightProp.boolValue;
            bool oldLeftValue = m_isLeftProp.boolValue;
            bool oldDownValue = m_isDownProp.boolValue;

            bool newUpValue = false;
            bool newRightValue = false;
            bool newLeftValue = false;
            bool newDownValue = false;

            if (m_isUpProp.boolValue)
                newLeftValue = true;
            if (m_isRightProp.boolValue)
                newUpValue = true;
            if (m_isLeftProp.boolValue)
                newDownValue = true;
            if (m_isDownProp.boolValue)
                newRightValue = true;

            m_isUpProp.boolValue = newUpValue;
            m_isRightProp.boolValue = newRightValue;
            m_isLeftProp.boolValue = newLeftValue;
            m_isDownProp.boolValue = newDownValue;
        }

        GUILayout.Space(5f);
        EditorGUILayout.LabelField("Don't Touch", EditorStyles.boldLabel);
        // 인스펙터 변수
        m_isUpProp.boolValue = EditorGUILayout.Toggle("Is Up", m_isUpProp.boolValue);
        m_isRightProp.boolValue = EditorGUILayout.Toggle("Is Right", m_isRightProp.boolValue);
        m_isLeftProp.boolValue = EditorGUILayout.Toggle("Is Left", m_isLeftProp.boolValue);
        m_isDownProp.boolValue = EditorGUILayout.Toggle("Is Down", m_isDownProp.boolValue);

        serializedObject.ApplyModifiedProperties();
    }
}
