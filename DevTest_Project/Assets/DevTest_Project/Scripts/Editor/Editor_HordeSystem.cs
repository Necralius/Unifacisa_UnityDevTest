using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static NekraByte.Utility.DataTypes;
using static NekraByte.Utility.Enumerators;

[CustomEditor(typeof(HordeData), true), CanEditMultipleObjects]
public class Editor_HordeSystem : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var hordeData = target as HordeData;

        if (hordeData == null) return;

        if (hordeData._hordeType == HordeType.Quad)
        {
            hordeData._height               = EditorGUILayout.FloatField("Quad Height", hordeData._height);
            hordeData._width                = EditorGUILayout.FloatField("Quad Width", hordeData._width);

            hordeData._customMeterThreshold = EditorGUILayout.Toggle("Custom Meter Threshold", hordeData._customMeterThreshold);

            if (hordeData._customMeterThreshold)
                hordeData._spawnEachMeter = EditorGUILayout.FloatField("Spawn Each Meter", hordeData._spawnEachMeter);
            else hordeData._spawnEachMeter = 1f;

            hordeData._customMeshLineThickness = EditorGUILayout.Toggle("Custom Mesh Line Thickness", hordeData._customMeshLineThickness);

            if (hordeData._customMeshLineThickness)
                hordeData._meshLineThickness = EditorGUILayout.FloatField("Mesh Line Thickness", hordeData._meshLineThickness);
            else hordeData._meshLineThickness = 1f;
        }
        else if (hordeData._hordeType == HordeType.Circle)
        {
            hordeData._circleRadius         = EditorGUILayout.FloatField("Circle Radius", hordeData._circleRadius);
            hordeData._meshLineThickness    = EditorGUILayout.FloatField("MeshLineThickness", hordeData._meshLineThickness);

            hordeData._customMeterThreshold = EditorGUILayout.Toggle("Custom Meter Threshold", hordeData._customMeterThreshold);

            if (hordeData._customMeterThreshold)
                hordeData._spawnEachMeter = EditorGUILayout.FloatField("Spawn Each Meter", hordeData._spawnEachMeter);
            else hordeData._spawnEachMeter = 1f;

            hordeData._customMeshLineThickness = EditorGUILayout.Toggle("Custom Mesh Line Thickness", hordeData._customMeshLineThickness);

            if (hordeData._customMeshLineThickness)
                hordeData._meshLineThickness = EditorGUILayout.FloatField("Mesh Line Thickness", hordeData._meshLineThickness);
            else hordeData._meshLineThickness = 1f;
        }
        else if (hordeData._hordeType == HordeType.Texture)
        {
            hordeData._hordeTexture         = TextureField("Horde Texture", hordeData._hordeTexture);
            
            hordeData._customMeterThreshold = EditorGUILayout.Toggle("Custom Meter Threshold", hordeData._customMeterThreshold);

            if (hordeData._customMeterThreshold)
                hordeData._spawnEachMeter = EditorGUILayout.FloatField("Spawn Each Meter", hordeData._spawnEachMeter);
            else hordeData._spawnEachMeter = 1f;

            hordeData._customMeshLineThickness = EditorGUILayout.Toggle("Custom Mesh Line Thickness", hordeData._customMeshLineThickness);

            if (hordeData._customMeshLineThickness)
                hordeData._meshLineThickness = EditorGUILayout.FloatField("Mesh Line Thickness", hordeData._meshLineThickness);
            else hordeData._meshLineThickness = 1f;
        }
        else if (hordeData._hordeType == HordeType.Random)
        {
            hordeData._randomRadius = EditorGUILayout.FloatField("Random Randius", hordeData._randomRadius);
        }
    }

    private static Texture TextureField(string name, Texture texture)
    {
        GUILayout.BeginVertical();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperCenter;
        style.fixedWidth = 90;
        GUILayout.Label(name, style);
        var result = (Texture)EditorGUILayout.ObjectField(texture, typeof(Texture), false, GUILayout.Width(70), GUILayout.Height(70));
        GUILayout.EndVertical();
        return result;
    }
}