using UnityEngine;

// --------------------------------------------------------------------------
// Name: CharacterManager (Class)
// Desc: This class represents an character atributtes manage, mainly the
//       class manages the player health system and the player aspects.
// --------------------------------------------------------------------------
public class CharacterManager : MonoBehaviour
{
    #region - Singleton Pattern -
    // ----------------------------------------------------------------------
    // Name: Singleton Pattern (Desing Pattern)
    // Desc: This statements represents an Singleton Pattern, an pattern that
    //       garantee that this class hava only one instance in the entire
    //       scene, also garantee that this class will have an global acess
    //       point.
    // ----------------------------------------------------------------------
    public static CharacterManager Instance;
    private void Awake()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
    }
    #endregion

    //Inspector Assingned Data
    [Header("Health Data")]
    [SerializeField, Range(1,1000)] private float _health       = 100f;
    [SerializeField, Range(1,1000)] private float _maxHealth    = 100f;

    //Public Encapsulated Data
    public float Health
    {
        get => _health;
        set
        {
            if (value > _maxHealth) _health = _maxHealth; 
            else if (value <= 0)
            {
                _health = 0;
                Die();
            }
            else _health = value;
            GameManager.Instance.SetHealthSlider(Health, _maxHealth);
        } 
    }

    // ------------------------------------------ Methods ------------------------------------------ //

    #region - BuildIn Methods -
    // ----------------------------------------------------------------------
    // Name: Start (Method)
    // Desc: This method is called on the game start, mainly the method reset
    //       the player health and update the health slider.
    // ----------------------------------------------------------------------
    void Start()
    {
        Health = _maxHealth;
        GameManager.Instance.SetHealthSlider(Health, _maxHealth);
    }
    #endregion

    #region - Player Behavior -
    // ----------------------------------------------------------------------
    // Name: TakeDamage (Method)
    // Desc: This method manage the player damage take action, updating the
    //       health values and the health slider.
    // ----------------------------------------------------------------------
    public void TakeDamage(float damage)
    {
        Health -= damage;
        GameManager.Instance.SetHealthSlider(_health, _maxHealth);
    }

    // ----------------------------------------------------------------------
    // Name: Die (Method)
    // Desc: This method manage the Death action of the player.
    // ----------------------------------------------------------------------
    private void Die()
    {
        Time.timeScale = 0f;
        GameManager.Instance.PlayerDeath();
    }
    #endregion
}