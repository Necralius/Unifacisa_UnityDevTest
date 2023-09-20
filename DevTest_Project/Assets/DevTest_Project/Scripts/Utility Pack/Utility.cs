using System;
using System.Collections.Generic;
using UnityEngine;
using static NekraByte.Utility.DataTypes;
using static NekraByte.Utility.Enumerators;

namespace NekraByte
{
    // ----------------------------------------------------------------------
    // Name: NekraByte (Static Class)
    // Desc: This static class bring some new DataTypes, Enumerations and
    //       tools to improve the current project development.
    // ----------------------------------------------------------------------
    public static class Utility
    {
        #region - Data Types -
        // --------------------------------------------------------------
        // Name: DataTypes (Static Class)
        // Desc: This class declares some DataTypes that are required to
        //       the project development.
        // --------------------------------------------------------------
        public static class DataTypes
        {
            #region - Pool Data -
            // --------------------------------------------------------------
            // Name: PoolData (Class)
            // Desc: This class declare the pool data for the object pooler.
            // --------------------------------------------------------------
            [Serializable]
            public class PoolData
            {
                public string       tag;
                public int          size;
                public GameObject   prefab;
                public bool isAnCanvasObject = false;
            }
            #endregion

            // --------------------------------------------------------------
            // Name: IPooledObject (Interface)
            // Desc: This inteface implements two required methods that
            //       manage actions on the pooled objects. 
            // --------------------------------------------------------------
            public interface IPooledObject
            {
                public void OnActivate();
                public void OnDeactivate();
            }
        }
        #endregion

        #region - Enumerators Types -
        // --------------------------------------------------------------
        // Name: Enumerators (Static Class)
        // Desc: This class storages an enumerators package to the
        //       current project.
        // --------------------------------------------------------------
        public static class Enumerators
        {
            public enum ControllerType { Joystick, Keyboard, Mouse }
            public enum HordeType { Random, Texture, Circle, Quad }
        }
        #endregion

        #region - Procedural Data Generation -
        // --------------------------------------------------------------
        // Name: Procedural (Static Class)
        // Desc: This class storages methods that mainly produces
        //       procedural data and return this data.
        // --------------------------------------------------------------
        public static class Procedural
        {
            #region - Square Horde Positions Generation -
            // --------------------------------------------------------------
            // Name: DrawSquareHorde
            // Desc: This method uses vetorial math to draw an square,
            //       considering the current target as it center, also draw
            //       the horde thickness, spawnpoints and later, the method
            //       return all the spawnpoints(Vector2) of this square.
            // --------------------------------------------------------------
            public static List<Vector2> DrawSquareHorde(Vector2 targetCenter, HordeData hordeData, bool debug)
            {
                List<Vector2> spawnPoints = new List<Vector2>();

                float xPos    = targetCenter.x;
                float yPos    = targetCenter.y;

                float height  = hordeData._height;
                float width   = hordeData._width;

                Vector2 vec01 = new Vector2(-width + xPos, height + yPos);
                Vector2 vec02 = new Vector2(width + xPos, height + yPos);
                Vector2 vec03 = new Vector2(width + xPos, -height + yPos);
                Vector2 vec04 = new Vector2(-width + xPos, -height + yPos);

                spawnPoints.Add(vec01);
                spawnPoints.Add(vec02);
                spawnPoints.Add(vec03);
                spawnPoints.Add(vec04);

                if (debug)
                {
                    Gizmos.DrawLine(vec01, vec02);
                    Gizmos.DrawLine(vec02, vec03);
                    Gizmos.DrawLine(vec03, vec04);
                    Gizmos.DrawLine(vec04, vec01);
                }

                float upDistance        = Vector2.Distance(vec01, vec02);
                float rightDistance     = Vector2.Distance(vec02, vec03);
                float bottomDistance    = Vector2.Distance(vec03, vec04);
                float leftDistance      = Vector2.Distance(vec04, vec01);

                float meterSpawn        = hordeData._spawnEachMeter;

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

                if (!hordeData._customMeshLineThickness) hordeData._meshLineThickness = 0f;
                if (hordeData._customMeshLineThickness && hordeData._meshLineThickness > 0.3f)
                {
                    float lineThickness = hordeData._meshLineThickness;
                    width   += lineThickness;
                    height  += lineThickness;

                    Vector2 vec001 = new Vector2(-width + xPos, height + yPos);
                    Vector2 vec002 = new Vector2(width + xPos , height + yPos);
                    Vector2 vec003 = new Vector2(width + xPos , -height + yPos);
                    Vector2 vec004 = new Vector2(-width + xPos, -height + yPos);

                    spawnPoints.Add(vec001);
                    spawnPoints.Add(vec002);
                    spawnPoints.Add(vec003);
                    spawnPoints.Add(vec004);

                    if (debug)
                    {
                        Gizmos.DrawLine(vec001, vec002);
                        Gizmos.DrawLine(vec002, vec003);
                        Gizmos.DrawLine(vec003, vec004);
                        Gizmos.DrawLine(vec004, vec001);
                    }

                    float upDistance1       = Vector2.Distance(vec001, vec002);
                    float rightDistance1    = Vector2.Distance(vec002, vec003);
                    float bottomDistance1   = Vector2.Distance(vec003, vec004);
                    float leftDistance1     = Vector2.Distance(vec004, vec001);

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
                }

                //Gizmos Draw
                if (debug) Gizmos.color = Color.yellow;
                if (debug) 
                    foreach (var vec in spawnPoints) Gizmos.DrawSphere(vec, 0.1f); //Only active if the boolean Debug is true

                return spawnPoints;
            }
            #endregion

            #region - Circle Horde Positions Generation -
            // --------------------------------------------------------------
            // Name: DrawCircleHorde (Static Method)
            // Desc: This method uses vetorial math to draw an circle,
            //       considering the current target as it center, also draw
            //       the horde thickness, spawnpoints and later, the method
            //       return all the SpawnPoints(Vector2) of this square.
            // --------------------------------------------------------------
            public static List<Vector2> DrawCircleHorde(Vector2 targetCenter, HordeData hordeData, bool debug)
            {
                List<Vector2> spawnPoints = new List<Vector2>();

                float xPos = targetCenter.x;
                float yPos = targetCenter.y;

                //Circle positions generation considering the player pos as center.
                for (int i = 0; i < hordeData._circleResolution; i++)
                {
                    float circumferenceProgress = (float)i/ hordeData._circleResolution;

                    float currentRadian = circumferenceProgress * 2 * Mathf.PI;

                    float xScaled = Mathf.Cos(currentRadian);
                    float yScaled = Mathf.Sin(currentRadian);

                    float x = xScaled * hordeData._circleRadius;
                    float y = yScaled * hordeData._circleRadius;

                    Vector2 newVec = new Vector2(x + xPos, y + yPos);
                    spawnPoints.Add(newVec);
                }

                //Gizmos circle rendering -> Only active if the boolean Debug is true
                if (debug) Gizmos.color = Color.yellow;
                if (debug)
                {
                    for (int i = 0; i < spawnPoints.Count; i++)
                    {
                        if (i == spawnPoints.Count - 1) Gizmos.DrawLine(spawnPoints[i], spawnPoints[0]);
                        else Gizmos.DrawLine(spawnPoints[i], spawnPoints[i + 1]);

                        Gizmos.DrawSphere(spawnPoints[i], 0.1f);
                    }
                }
                return spawnPoints;
            }
            #endregion
        }
        #endregion
    }
}