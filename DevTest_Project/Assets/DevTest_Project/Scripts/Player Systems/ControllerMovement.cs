using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class ControllerMovement : MonoBehaviour
{
    //Dependencies
    private Animator        _anim           => GetComponent<Animator>();
    private Rigidbody2D     _rb             => GetComponent<Rigidbody2D>();
    private InputManager _inptManager;

    [SerializeField, Range(0.1f, 20f)] private float playerSpeed = 3f;
    private bool _isWalking = false;

    //Animation Hashes
    private int yAxisHash       = Animator.StringToHash("InputY");
    private int xAxisHash       = Animator.StringToHash("InputX");
    private int isWalkingHash   = Animator.StringToHash("IsWalking");

    //Private Data
    Vector2 _moveInput = Vector2.zero;

    // ----------------------------------------------------------------------
    // Name: Start
    // Desc: This method 
    // ----------------------------------------------------------------------
    void Start()
    {
        _inptManager = InputManager.Instance;
    }

    //
    // Name: MovementHandler
    // Desc: Update
    //
    void Update()
    {
        MovementHandler();
        AnimatorHandler();
        UpdateCalls();
    }
    private void UpdateCalls()
    {
        _moveInput = _inptManager.Move;

        _isWalking = !(_moveInput == Vector2.zero);
    }

    // ----------------------------------------------------------------------
    // Name: MovementHandler
    // Desc: This method manage the player movement using the input vector
    //       multiplied by the player speed and setting it to the current
    //       rigidbody
    // ----------------------------------------------------------------------
    private void MovementHandler()
    {
        if (_rb == null) return;

        if (_moveInput == Vector2.zero)
        {
            _rb.velocity = new Vector2(0, 0);
            return;
        }
        _rb.velocity = _moveInput.normalized * playerSpeed;
    }
    private void AnimatorHandler()
    {
        _anim.SetFloat(xAxisHash, _moveInput.x);
        _anim.SetFloat(yAxisHash, _moveInput.y);
        _anim.SetBool(isWalkingHash, _isWalking);
    }
}
