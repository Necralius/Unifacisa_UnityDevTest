using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NekraByte.Utility.Enumerators;

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
    }
}