using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    #region - Singleton Pattern -
    public static InputManager Instance;
    #endregion

    #region - Dependencies -
    private PlayerInput playerInput => GetComponent<PlayerInput>();

    #endregion

    public InputAction _moveAction { get; private set; }

    public Vector2 Move { get; private set; }

    //Private Data
    private InputActionMap currentMap;

    private string keyboardMap = "Keyboard";
    private string joystickMap = "Joystick";

    // ------------------------------------------ Methods ------------------------------------------ //

    #region - BuiltIn Methods -
    // ----------------------------------------------------------------------
    // Name : Awake
    // Desc : This method its called in the very first application frame,
    //        also this method get all the map actions and translate them
    //        in to literal game inputs.
    // ----------------------------------------------------------------------
    private void Awake()
    {
        //Singleton Pattern
        if (Instance != null) Destroy(this);
        Instance = this;

        currentMap = playerInput.currentActionMap;

        //Movment Actions
        _moveAction = currentMap.FindAction("Move");


        _moveAction.performed   += onMove;

        _moveAction.canceled    += onMove;
    }
    #endregion

    #region - Input Gethering -
    private void onMove(InputAction.CallbackContext context) => Move = context.ReadValue<Vector2>();
    #endregion

    #region - Change Input Map -
    public void ChangeInputMap(string mapToSet)
    {
        playerInput.SwitchCurrentActionMap(mapToSet);
    }
    #endregion
}