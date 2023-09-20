using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NekraByte.Utility.DataTypes;
using static NekraByte.Utility.Procedural;
using static NekraByte.Utility.Enumerators;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    //Inspector Assingned Data
    [Header("Default Enemy Spawn System")]
    [SerializeField] private string                     _recursiveEnemyTag  = "EnemyTag";
    [SerializeField, Range(0.1f, 100f)] private float   _spawnRadius        = 2f;
    [SerializeField, Range(0.1f, 10f)] private float    _spawnTime          = 2f;

    [Header("Horde Spawning Data")]
    [SerializeField] private List<HordeData>    _hordes         = new List<HordeData>();
    [SerializeField] private GameObject         _player         = null;
    [SerializeField] private HordeData          _currentHorde   = null;

    //Private Data
    private List<Vector2> _positionsToSpawn = new List<Vector2>();

    [Header("Hordes Data")]

    //Spawn States
    [Space, Header("Spawn States")]
    [SerializeField] private bool _isInHorde = false;
    [SerializeField] private bool _debug = true;

    private float _timerSpawn = 0;

    // ------------------------------------------ Methods ------------------------------------------

    #region - BuildIn Methods
    // ----------------------------------------------------------------------
    // Name: Update (Method)
    // Desc: This method is called every frame, mainly the method manages the
    //       horde spawn system and spawn random enemies every certain time.
    // ----------------------------------------------------------------------
    private void Update()
    {
        VerifyHorde(GameManager.Instance._Timer);

        if (_timerSpawn >= _spawnTime)
        {
            SpawnEntity();
            _timerSpawn = 0;
        }
        else _timerSpawn += Time.deltaTime;
    }
    #endregion

    #region - Horde Spawning -
    // ----------------------------------------------------------------------
    // Name: SpawnHorde (Method)
    // Desc: This method spawns an random horde on the game, considering the
    //       horde structure and aspects.
    // ----------------------------------------------------------------------
    public void SpawnHorde()
    {
        //The below statements prevent any bugs or crashes on the system.
        if (_isInHorde)                     return;
        if (_hordes.Count <= 0)             return;
        if (_positionsToSpawn.Count <= 0)   return;

        //The below statement select an horde using an random index in the horde list limits.
        HordeData selectedHorde = _hordes[Random.Range(0, _hordes.Count)];

        if (selectedHorde == null)
        {
            Debug.LogWarning("Null horde finded.");
            return;
        }

        if (_currentHorde._hordeType == HordeType.Quad) StartCoroutine(Square_DrawHorde());
        else if (_currentHorde._hordeType == HordeType.Circle) StartCoroutine(Circle_DrawHorde());

        //The below statements loop on every SpawnPoints and spawn an enemy instance in this position.
        foreach (var point in _positionsToSpawn) 
            ObjectPooler.Instance.SpawnFromPool(selectedHorde._enemyTag, point, Quaternion.identity);

        //Later the horde are removed from the list
        _hordes.Remove(selectedHorde);
        _isInHorde = false;
    }

    // ----------------------------------------------------------------------
    // Name: SpawnHorde (Method override)
    // Desc: This method spawns an especific horde on the game, considering the
    //       horde structure and aspects.
    // ----------------------------------------------------------------------
    public void SpawnHorde(HordeData hordeData)
    {
        //The below statements prevent any bugs or crashes on the system.
        if (_isInHorde) return;
        if (_positionsToSpawn.Count <= 0) return;
        if (_hordes.Count <= 0) return;

        if (hordeData == null)
        {
            Debug.LogWarning("Null horde finded.");
            return;
        }

        if (_currentHorde._hordeType == HordeType.Quad)
        {
            _positionsToSpawn.Clear();
            _positionsToSpawn = DrawSquareHorde(_player.transform.position, _currentHorde, false);
        }
        else if (_currentHorde._hordeType == HordeType.Circle)
        {
            _positionsToSpawn.Clear();
            _positionsToSpawn = DrawCircleHorde(_player.transform.position, _currentHorde, false);
        }

        //The below statements loop on every SpawnPoints and spawn an enemy instance in this position.
        foreach (var point in _positionsToSpawn)
            ObjectPooler.Instance.SpawnFromPool(hordeData._enemyTag, point, Quaternion.identity);

        //Later the horde are removed from the list
        _hordes.Remove(hordeData);
        _isInHorde = false;
    }

    // ----------------------------------------------------------------------
    // Name: VerifyHorde (Methods)
    // Desc: This method verifies if an horde needs to be spawned on his
    //       certain time.
    // ----------------------------------------------------------------------
    private void VerifyHorde(float time)
    {
        if (_isInHorde) return;
        if (_hordes.Count == 0) return;

        foreach (var horde in _hordes)
        {
            if (time >= horde._spawnTime && !_isInHorde)
            {
                SpawnHorde(horde);
                break;
            }
        }
    }
    #endregion

    #region - Horde Draw -
    // ----------------------------------------------------------------------
    // Name: Square_DrawHorde (IEnumerator)
    // Desc: This method draw the square horde data on the screen using
    //       gizmos.
    // ----------------------------------------------------------------------
    IEnumerator Square_DrawHorde()
    {
        //First the SpawnPoints are cleaned and the coroutine call the DrawSquareHorde
        //method, passing the correct arguments, also, the DrawSquareHorde return data
        //are assing to the SpawnPoints.

        _positionsToSpawn.Clear();
        _positionsToSpawn = DrawSquareHorde(_player.transform.position, _currentHorde, _debug);
        yield return null;
    }
    // ----------------------------------------------------------------------
    // Name: Circle_DrawHorde (IEnumerator)
    // Desc: This method draw the circle horde data on the screen using
    //       gizmos.
    // ----------------------------------------------------------------------
    IEnumerator Circle_DrawHorde()
    {
        //First the SpawnPoints are cleaned and the coroutine call the DrawCircleHorde
        //method, passing the correct arguments, also, the DrawCircleHorde return data
        //are assing to the SpawnPoints.

        _positionsToSpawn.Clear();
        _positionsToSpawn = DrawCircleHorde(_player.transform.position, _currentHorde, _debug);

        yield return null;
    }
    // ----------------------------------------------------------------------
    // Name: OnDrawGizmos (Method)
    // Desc: This method draw the current horde format on the screen using
    //       gizmos, the method uses the horde data to draw the correct
    //       format.
    // ----------------------------------------------------------------------
    private void OnDrawGizmos()
    {
        if (_currentHorde._hordeType == HordeType.Quad)         StartCoroutine(Square_DrawHorde());
        else if (_currentHorde._hordeType == HordeType.Circle)  StartCoroutine(Circle_DrawHorde());
    }
    #endregion

    #region - Random Enemy Spawning -
    // ----------------------------------------------------------------------
    // Name: SpawnEntity (Method)
    // Desc: This method spawn an enemy entity on the scene, the enemy is
    //       spawned on an random position inside an circle, considering the
    //       spawn radius.
    // ----------------------------------------------------------------------
    private void SpawnEntity()
    {
        Vector3 spawnPoint = Random.insideUnitCircle * _spawnRadius;

        ObjectPooler.Instance.SpawnFromPool(_recursiveEnemyTag, spawnPoint, Quaternion.identity);
    }
    #endregion
}