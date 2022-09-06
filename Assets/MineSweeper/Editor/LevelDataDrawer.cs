using UnityEngine;
using UnityEditor;

/// <summary>LevelDataArray�����ɓ�Փx�f�[�^�̓Y��������ύX���� </summary>
[CustomPropertyDrawer(typeof(LevelDataArray))]
public class LevelDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
            EditorGUI.PropertyField(position, property, new GUIContent(((LevelDataArray)attribute)._indexName[pos]));

        }
        catch
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
