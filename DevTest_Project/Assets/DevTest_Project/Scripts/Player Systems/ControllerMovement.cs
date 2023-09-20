using System.Collections;
using UnityEngine;

//RequireComponent -> This statement makes this class direcly dependent of
//the seted component, making that the component be obligatorily added to the object  

// --------------------------------------------------------------------------
// Name: DamagePopup (Class)
// Desc: This method represents the player movement controlling system,
//       mainly controlling the player body on the scene.
// --------------------------------------------------------------------------
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class ControllerMovement : MonoBehaviour
{
    //Dependencies
    private Animator        _anim           => GetComponent<Animator>();
    private Rigidbody2D     _rb             => GetComponent<Rigidbody2D>();
    private InputManager    _inptManager    => InputManager.Instance;

    //Inspector Assingned Data

    //Player Settings
    [Header("Player Settings")]
    [SerializeField, Range(0.1f, 20f)] private float _playerSpeed = 3f;

    //Bullet Settings
    [Header("Bullet Settings")]
    [SerializeField, Range(0.1f, 10f)] private float _bulletSpeed = 2f;
    [SerializeField, Range(0.1f, 5f)]  private float _rateOfFire = 0.1f;

    [SerializeField] private GameObject _bulletSpawner = null;
    [SerializeField] private string _bulletTag = "";

    //Private Data

    //Player State 
    private bool _isWalking = false;
    private bool _canShoot = true;

    //Animation Hashes
    private int yAxisHash       = Animator.StringToHash("InputY");
    private int xAxisHash       = Animator.StringToHash("InputX");
    private int isWalkingHash   = Animator.StringToHash("IsWalking");

    //Private Data
    Vector2 _moveInput = Vector2.zero;
    Vector2 _mousePos = Vector2.zero;

    // ------------------------------------------ Methods ------------------------------------------ //

    #region - BuiltIn Methods -
    // ----------------------------------------------------------------------
    // Name: Update (Method)
    // Desc: This method is called every frame, mainly the method manages
    //       some actions that need to be updated every frame, and manages
    //       the player animator.
    // ----------------------------------------------------------------------
    void Update()
    {
        AnimatorHandler();
        UpdateTick();
    }

    // ----------------------------------------------------------------------
    // Name: FixedUpdate (Method)
    // Desc: This method is called an fixed times in a frame, is usefull for
    //       physics movements and updates, in this case, the aim, and the
    //       player movement actions where made using rigidbody, so is more
    //       efficient and reliable to call this updates in an FixedUpdate
    //       method, that call they in an default Update Method.
    // ----------------------------------------------------------------------
    private void FixedUpdate()
    {
        MovementHandler();
        AimHandler();
    }
    #endregion

    #region - Update Calls -
    // ----------------------------------------------------------------------
    // Name: UpdateTick (Method)
    // Desc: For an better code cleaning purpose, this method join some
    //       actions that where called in every update call, uniting them in
    //       an single method, and cleaning the main Update method.
    // ----------------------------------------------------------------------
    private void UpdateTick()
    {
        _moveInput = _inptManager.Move.normalized;

        _isWalking = !(_moveInput == Vector2.zero);

        if (_inptManager._shootAction.IsPressed() && _canShoot)
        {
            _canShoot = false;           
            StartCoroutine(ShootAction());
        }
    }
    #endregion

    #region - Movement Handler -
    // ----------------------------------------------------------------------
    // Name: MovementHandler
    // Desc: This method manage the player movement using the input vector
    //       multiplied by the player speed and setting it to the current
    //       rigidbody.
    // ----------------------------------------------------------------------
    private void MovementHandler()
    {
        if (_rb == null) return;

        if (_moveInput == Vector2.zero)
        {
            _rb.velocity = new Vector2(0, 0);
            return;
        }
        
        _rb.velocity = new Vector2(_moveInput.x * _playerSpeed, _moveInput.y * _playerSpeed);
        _bulletSpawner.GetComponent<Rigidbody2D>().position = _rb.position;
    }

    // ----------------------------------------------------------------------
    // Name: AimHandler (Method)
    // Desc: this method manages the aim direction of the shoot system,
    //       mainly, the system calculates the angle of the shoot based on
    //       the mouse position, also, the method translate this direction in
    //       an bulletSpawner object, that will be used later for the bullet
    //       spawning following the mouse position.
    // ----------------------------------------------------------------------
    private void AimHandler()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 aimDirection = _mousePos - _bulletSpawner.GetComponent<Rigidbody2D>().position;

        float aimAngle = Mathf.Atan2(aimDirection.x, aimDirection.y) * Mathf.Rad2Deg - 90f;
        _bulletSpawner.GetComponent<Rigidbody2D>().rotation = -aimAngle;
    }
    #endregion

    #region - Animation Handler -
    // ----------------------------------------------------------------------
    // Name: AnimatorHandler (Method)
    // Desc: This method handle the animation system, setting the correct
    //       parameters values on the animator component.
    // ----------------------------------------------------------------------
    private void AnimatorHandler()
    {
        _anim.SetFloat(xAxisHash, _moveInput.x);
        _anim.SetFloat(yAxisHash, _moveInput.y);
        _anim.SetBool(isWalkingHash, _isWalking);
    }
    #endregion

    #region - Shoot System -
    // ----------------------------------------------------------------------
    // Name: Shoot System (Coroutine)
    // Desc: This Coroutine spawns an bullet on the bullet spawner, and throws
    //       it in the mouse direction, also, the coroutine limit this shots
    //       on an rate of fire.
    // ----------------------------------------------------------------------
    IEnumerator ShootAction()
    {
        GameObject bullet = ObjectPooler.Instance.SpawnFromPool(_bulletTag, _bulletSpawner.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(_bulletSpawner.transform.right * _bulletSpeed / 100, ForceMode2D.Impulse);

        yield return new WaitForSeconds(_rateOfFire);
        _canShoot = true;
    }
    #endregion
}