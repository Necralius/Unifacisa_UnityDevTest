using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private GameObject _target;

    [SerializeField, Range(0.001f, 1f)] private float _tickTime = 0.1f;
    private float _timer = 0f;

    [SerializeField, Range(0.1f, 10f)] private float _travelSpeed = 2f;

    [SerializeField] private float _health = 100f;

    private void OnEnable()
    {
        _target = CharacterManager.Instance.gameObject; 
    }

    private void Update()
    {
        if (_timer >= _tickTime)
        {
            UpdateTick();
            _timer = 0f;
        }
        else _timer += Time.deltaTime;

        FollowPlayer();
    }
    private void UpdateTick()
    {
        if (_health <= 0) Die();
    }

    protected void FollowPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, _target.transform.position, _travelSpeed * Time.deltaTime);
    }
    protected void Die()
    {

    }
}