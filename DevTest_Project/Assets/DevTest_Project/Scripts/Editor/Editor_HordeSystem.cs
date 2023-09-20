using UnityEditor;
using UnityEngine;
using static NekraByte.Utility.DataTypes;
using static NekraByte.Utility.Enumerators;

// --------------------------------------------------------------------------
// Name: Editor_HordeSystem (Editor Class)
// Desc: This class change editor settings on an certain class type, in this
//       case, the HordeData class.
// --------------------------------------------------------------------------
[CustomEditor(typeof(HordeData), true), CanEditMultipleObjects]
public class Editor_HordeSystem : Editor
{
    // ----------------------------------------------------------------------
    // Name: OnInspectorGUI (Override Method)
    // Desc: This method overrides the default Inspector GUI drawning,
    //       customazing some fields based on the object actual data.
    // ----------------------------------------------------------------------
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();// -> This statement draw the defaul inspector.

        //In this section the method try to get the actual data that he is modifying.
        var hordeData = target as HordeData;

        if (hordeData == null) return;

        //In the below section, some data fields are disable, and other are enable, modeling the inspector based on the current data type of interest.
        if (hordeData._hordeType == HordeType.Quad)// -> This modifications will be drawned only if the expression is true.
        {
            hordeData._height               = EditorGUILayout.FloatField("Quad Height", hordeData._height);
            hordeData._width                = EditorGUILayout.FloatField("Quad Width", hordeData._width);

            hordeData._customMeterThreshold = EditorGUILayout.Toggle("Custom Meter Threshold", hordeData._customMeterThreshold);

            if (hordeData._customMeterThreshold) //-> The field only will be drawned if the boolean is true.
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
            hordeData._circleResolution     = EditorGUILayout.IntField("Circle Resolution", hordeData._circleResolution);
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

    // ----------------------------------------------------------------------
    // Name: TextureField (Static Texture Method) 
    // Desc: This method creates an small texture window on the inspector.
    // ----------------------------------------------------------------------
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