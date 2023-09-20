using static NekraByte.Utility.DataTypes;
using UnityEngine;

// --------------------------------------------------------------------------
// Name: EnemyBase (BaseClass)
// Desc: This class represents the enemy base class, implementing every base
//       actions for every enemy in the game.
// --------------------------------------------------------------------------
public class EnemyBase : MonoBehaviour, IPooledObject
{
    //Inspector Assingned Data
    [Header("Movement Settings")]
    [Tooltip("Movement Update Time")]
    [SerializeField, Range(0.1f, 30f)] private float        _travelSpeed    = 2f;

    [Header("Health Settings")]
    [SerializeField, Range(1f, 100f)] private float         _health         = 20f;
    [SerializeField, Range(1f, 100f)] private float         _maxHealth      = 20f;

    [Header("Damage Settings")]
    [Tooltip("Damage Per Second")]
    [SerializeField, Range(0.001f, 10f)] private float      _damageForce    = 0.1f;

    //Class Dependencies
    private Rigidbody2D _rb;
    private GameObject _target;

    //Public Encapsulated Data
    public float Health 
    { 
        //This data assing model is used for better data management.
        get => _health;
        set
        {
            if (value <= 0)
            {
                _health = 0;
                Die();
            }
            else _health = value;
        }
    }

    //Protected -> That word represents an access modifier, means that, only the inherited classes will have access to this class or variable.
    //Virtual   -> That word represents an method declaration modifier, means that, this method can be overrided in the future.

    // ------------------------------------------ Methods ------------------------------------------

    #region - BuildIn Methods -
    // ----------------------------------------------------------------------
    // Name: Start (Method)
    // Desc: This method is called on the game start, mainly the method get
    //       the necessary components and reset the enemy health.
    // ----------------------------------------------------------------------
    protected virtual void Start()
    {
        _rb         = GetComponent<Rigidbody2D>();
        _target     = GameManager.Instance._playerInstance;
        Health      = _maxHealth;
    }

    // ----------------------------------------------------------------------
    // Name: Update (Method)
    // Desc: This method is called on every frame update, mainly the method
    //       calls the follow player actions every frame.
    // ----------------------------------------------------------------------
    protected virtual void Update()
    {
        FollowPlayer();
    }

    // ----------------------------------------------------------------------
    // Name: OnCollisionStay2D (BuiltIn Method)
    // Desc: This method detects every time that this object collider
    //       collides with an object with an trigger collider and stay in
    //       this collision, also the method get infromation from this
    //       collision.
    // ----------------------------------------------------------------------
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Mainly the method detects bullet hits on the enemy body.
        if (collision.transform.CompareTag("Bullet"))
        {
            collision.gameObject.SetActive(false);
            TakeDamage(12);
        }
    }

    // ----------------------------------------------------------------------
    // Name: OnCollisionStay2D (BuiltIn Method)
    // Desc: This method detects every time that this object collider
    //       collides with an object and stay in this collision, also the
    //       method get infromation from this collision.
    // ----------------------------------------------------------------------
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        //Mainly the method detects if the player is near enough to the enemy to give him damage,
        //also, the damage is gived in a per second context, so the damage is divided by 60.
        if (collision.transform.CompareTag("Player"))
            collision.transform.GetComponentInParent<CharacterManager>().TakeDamage(_damageForce / 60);   
    }
    #endregion

    #region - Enemy Behavior -
    // ----------------------------------------------------------------------
    // Name: FollowPlayer (Method)
    // Desc: This method manage the enemy follow system, making the enemy
    //       follow the player recursively.
    // ----------------------------------------------------------------------
    protected virtual void FollowPlayer()
    {
        Vector3 direction = transform.position - _target.transform.position;
        _rb.velocity = -direction * _travelSpeed;
    }

    // ----------------------------------------------------------------------
    // Name: Die (Method)
    // Desc: This method executes the enemy death action, only deactivating
    //       it.
    // ----------------------------------------------------------------------
    protected virtual void Die()
    {
        gameObject.SetActive(false);
    }

    // ----------------------------------------------------------------------
    // Name: TakeDamage (Method)
    // Desc: This method realizes an damage on the enemy, also spawning the
    //       needed visual feedbacks.
    // ----------------------------------------------------------------------
    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        GameManager.Instance.SpawnValuePopup(transform.position, damage, true);
    }
    #endregion

    #region - Pooled Actions -
    // ----------------------------------------------------------------------
    // Name: OnActivate
    // Desc: This method is an implement of the IPooled interface, that
    //       execute some actions on the Object Pooler class, the method is
    //       called on the object activation.
    // ----------------------------------------------------------------------
    public void OnActivate()
    {
        _target = CharacterManager.Instance.gameObject;
    }

    // ----------------------------------------------------------------------
    // Name: OnDeactivate
    // Desc: This method is an implement of the IPooled interface, that
    //       execute some actions on the Object Pooler class, the method is
    //       called on the object deactivation.
    // ----------------------------------------------------------------------
    public void OnDeactivate()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}