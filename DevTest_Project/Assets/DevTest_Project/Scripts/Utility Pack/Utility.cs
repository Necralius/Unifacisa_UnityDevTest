using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NekraByte.Utility.DataTypes;
using static NekraByte.Utility.Enumerators;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace NekraByte
{
    // ----------------------------------------------------------------------
    // Name: NekraByte
    // Desc: This static class bring some new DataTypes, Enumerations and
    //       tools to improve the current project development.
    // ----------------------------------------------------------------------
    public static class Utility
    {
        public static class DataTypes
        {
            //
            // Name: PoolData (Class)
            // Desc:
            //
            [Serializable]
            public class PoolData
            {
                public string       tag;
                public int          size;
                public GameObject   prefab;
            }

            [Serializable]
            [CreateAssetMenu(fileName = "New Horde", menuName = "Unifacisa Test/Hordes/New Horde")]
            public class HordeData : ScriptableObject
            {
                public HordeType _hordeType;
                [Tooltip("True means that the horde format will have total gizmos.")] 
                public bool Debug = false;
                [HideInInspector] public Vector3 _center = Vector3.zero;


                //Random
                [HideInInspector] public float  _randomRadius = 5f;

                //Quad
                [HideInInspector] public float _width               = 1f;
                [HideInInspector] public float _height              = 1f;

                //Circle
                [HideInInspector] public float _circleRadius        = 1f;

                //Texture
                [HideInInspector] public Texture _hordeTexture          = null;

                //General Settings
                [HideInInspector] public bool _customMeterThreshold     = false;
                [HideInInspector] public bool _customMeshLineThickness  = false;

                [HideInInspector] public float _spawnEachMeter      = 1f;
                [HideInInspector] public float _meshLineThickness   = 1f;
            }
        }
        public static class Enumerators
        {
            public enum ControllerType { Joystick, Keyboard, Mouse }
            public enum HordeType { Random, Texture, Circle, Quad }
        }

        public static class Procedural
        {
            #region - Square Horde Positions Generation -
            public static List<Vector2> DrawSquareHorde(GameObject playerObject, HordeData hordeData)
            {
                List<Vector2> spawnPoints = new List<Vector2>();
                if (playerObject == null)
                {
                    Debug.LogWarning("The horde drawning need the current player position, and the player GameObject is null!");
                    return null;
                }

                if (hordeData.Debug) Gizmos.color = Color.yellow;
                Vector3 targetPos = playerObject.transform.position;

                float xPos = targetPos.x;
                float yPos = targetPos.y;
                float height = hordeData._height;
                float width = hordeData._width;

                Vector2 vec01 = new Vector2(-width + xPos, height + yPos);
                Vector2 vec02 = new Vector2(width + xPos, height + yPos);
                Vector2 vec03 = new Vector2(width + xPos, -height + yPos);
                Vector2 vec04 = new Vector2(-width + xPos, -height + yPos);

                spawnPoints.Add(vec01);
                spawnPoints.Add(vec02);
                spawnPoints.Add(vec03);
                spawnPoints.Add(vec04);

                float upDistance = Vector2.Distance(vec01, vec02);
                float rightDistance = Vector2.Distance(vec02, vec03);
                float bottomDistance = Vector2.Distance(vec03, vec04);
                float leftDistance = Vector2.Distance(vec04, vec01);

                float meterSpawn = hordeData._spawnEachMeter;

                for (int i = 0; i < 4; i++)
                {
                    //Up vectors generations
                    int upIterations = (int)Mathf.Round(upDistance / hordeData._spawnEachMeter);
                    for (int u = 0; u <= upIterations; u++)
                    {
                        Vector2 newVec = new Vector2(vec01.x + (meterSpawn * u), vec01.y);
                        if (newVec.x >= vec02.x) break;
                        else spawnPoints.Add(newVec);
                    }

                    //Right vectors generation
                    int rightIterations = (int)Mathf.Round(rightDistance / hordeData._spawnEachMeter);
                    for (int r = 0; r <= rightIterations; r++)
                    {
                        Vector2 newVec = new Vector2(vec02.x, vec02.y - (meterSpawn * r));
                        if (newVec.y <= vec03.y) break;
                        else spawnPoints.Add(newVec);
                    }

                    //Bottom vectors generation
                    int bottomIterations = (int)Mathf.Round(bottomDistance / hordeData._spawnEachMeter);
                    for (int b = 0; b <= bottomIterations; b++)
                    {
                        Vector2 newVec = new Vector2(vec03.x - (meterSpawn * b), vec03.y);
                        if (newVec.x <= vec04.x) break;
                        else spawnPoints.Add(newVec);
                    }

                    //Left vectors generation
                    int leftIterations = (int)Mathf.Round(leftDistance / hordeData._spawnEachMeter);
                    for (int l = 0; l <= leftIterations; l++)
                    {
                        Vector2 newVec = new Vector2(vec04.x, vec04.y + (meterSpawn * l));
                        if (newVec.y >= vec01.y) break;
                        else spawnPoints.Add(newVec);
                    }
                }

                if (hordeData._customMeshLineThickness)
                {
                    float lineThickness = hordeData._meshLineThickness;
                    width += lineThickness;
                    height += lineThickness;

                    Vector2 vec001 = new Vector2(-width + xPos, height + yPos);
                    Vector2 vec002 = new Vector2(width + xPos, height + yPos);
                    Vector2 vec003 = new Vector2(width + xPos, -height + yPos);
                    Vector2 vec004 = new Vector2(-width + xPos, -height + yPos);

                    spawnPoints.Add(vec001);
                    spawnPoints.Add(vec002);
                    spawnPoints.Add(vec003);
                    spawnPoints.Add(vec004);

                    float upDistance1 = Vector2.Distance(vec001, vec002);
                    float rightDistance1 = Vector2.Distance(vec002, vec003);
                    float bottomDistance1 = Vector2.Distance(vec003, vec004);
                    float leftDistance1 = Vector2.Distance(vec004, vec001);

                    for (int i = 0; i < 4; i++)
                    {
                        int upIterations = (int)Mathf.Round(upDistance1 / hordeData._spawnEachMeter);
                        for (int u = 0; u <= upIterations; u++)
                        {
                            Vector2 newVec = new Vector2(vec001.x + (meterSpawn * u), vec001.y);
                            if (newVec.x >= vec002.x) break;
                            else spawnPoints.Add(newVec);
                        }

                        int rightIterations = (int)Mathf.Round(rightDistance1 / hordeData._spawnEachMeter);

                        for (int r = 0; r <= rightIterations; r++)
                        {
                            Vector2 newVec = new Vector2(vec002.x, vec002.y - (meterSpawn * r));
                            if (newVec.y <= vec003.y) break;
                            else spawnPoints.Add(newVec);
                        }

                        int bottomIterations = (int)Mathf.Round(bottomDistance1 / hordeData._spawnEachMeter);
                        for (int b = 0; b <= bottomIterations; b++)
                        {
                            Vector2 newVec = new Vector2(vec003.x - (meterSpawn * b), vec003.y);
                            if (newVec.x <= vec004.x) break;
                            else spawnPoints.Add(newVec);
                        }

                        int leftIterations = (int)Mathf.Round(leftDistance1 / hordeData._spawnEachMeter);

                        for (int l = 0; l <= leftIterations; l++)
                        {
                            Vector2 newVec = new Vector2(vec004.x, vec004.y + (meterSpawn * l));
                            if (newVec.y >= vec001.y) break;
                            else spawnPoints.Add(newVec);
                        }
                    }

                    if (hordeData.Debug)
                    {
                        Gizmos.DrawLine(vec001, vec002);
                        Gizmos.DrawLine(vec002, vec003);
                        Gizmos.DrawLine(vec003, vec004);
                        Gizmos.DrawLine(vec004, vec001);
                    }
                }

                if (hordeData.Debug)
                {
                    Gizmos.DrawLine(vec01, vec02);
                    Gizmos.DrawLine(vec02, vec03);
                    Gizmos.DrawLine(vec03, vec04);
                    Gizmos.DrawLine(vec04, vec01);
                }

                if (hordeData.Debug) foreach (var vec in spawnPoints) Gizmos.DrawSphere(vec, 0.1f);
                return spawnPoints;
            }
            #endregion


        }
    }
}