using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    #region - Singleton Pattern -
    public static CharacterManager Instance;
    private void Awake()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
    }
    #endregion

    [SerializeField, Range(1,1000)] private float _health       = 100f;
    [SerializeField, Range(1,1000)] private float _maxHealth    = 100f;

    //Public Data
    public float MaxHealth { get => MaxHealth; set => MaxHealth = value; }
    public float Health
    {
        get => _health;
        set
        {
            if (value > _maxHealth) _health = _maxHealth; 
            else if (value < _health) Die();
            else _health = value;
        } 
    }

    // ------------------------------------------ Methods ------------------------------------------ //

    void Start()
    {
        _health = _maxHealth;
    }
    
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {

    }
    private void Die()
    {

    }
}