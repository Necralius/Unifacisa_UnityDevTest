using UnityEngine;
using static NekraByte.Utility.Enumerators;

// --------------------------------------------------------------
// Name: HordeData (Class)
// Desc: This method declares the horde data that holds the some
//       datas, and use using some editor changes.
// --------------------------------------------------------------
public class HordeData : MonoBehaviour
{
    [Header("Horde General Settings")]
    public string _hordeTag = "HordeName";
    public string _enemyTag = "EnemyPoolTag";
    public HordeType _hordeType = HordeType.Circle;

    [Tooltip("This float represent the time on the timer, that the horde will be spawned")]
    public float _spawnTime = 0f;

    //Random settings -> TODO (Not Implemented)
    [HideInInspector] public float _randomRadius = 5f;

    //Quad settings
    [HideInInspector] public float _width = 1f;
    [HideInInspector] public float _height = 1f;

    //Circle settings
    [HideInInspector] public float _circleRadius = 1f;
    [HideInInspector] public int _circleResolution = 20;

    //Texture settings -> TODO (Not Implemented)
    [HideInInspector] public Texture _hordeTexture = null;

    //General Horde Settings
    [HideInInspector] public bool _customMeterThreshold = false;
    [HideInInspector] public bool _customMeshLineThickness = false;

    [HideInInspector] public float _spawnEachMeter = 1f;
    [HideInInspector] public float _meshLineThickness = 1f;
}