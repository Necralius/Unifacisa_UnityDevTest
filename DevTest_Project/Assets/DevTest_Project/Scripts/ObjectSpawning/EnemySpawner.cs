using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NekraByte.Utility.DataTypes;
using static NekraByte.Utility.Procedural;
using System;

public class EnemySpawner : MonoBehaviour
{
    public HordeData _currentHorde;

    public GameObject _player;

    private List<Vector2> positionsToSpawn = new List<Vector2>();

    IEnumerator DrawCubeHorde()
    {
        positionsToSpawn.Clear();
        positionsToSpawn = DrawSquareHorde(_player, _currentHorde);
        yield return null;
    }
    private void OnDrawGizmos()
    {
        StartCoroutine(DrawCubeHorde());
    }
}