using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    #region - Singleton Pattern -
    // ----------------------------------------------------------------------
    // Name: Singleton Pattern (Desing Pattern)
    // Desc: This statements represents an Singleton Pattern, an pattern that
    //       garantee that this class hava only one instance in the entire
    //       scene, also garantee that this class will have an global acess
    //       point.
    // ----------------------------------------------------------------------
    public static InputManager Instance;
    #endregion

    #region - Dependencies -
    //Class dependencies
    private PlayerInput playerInput => GetComponent<PlayerInput>();

    #endregion

    //Public Input Action Data
    public InputAction _moveAction          { get; private set; }
    public InputAction _lookAction          { get; private set; }
    public InputAction _shootAction         { get; private set; }
    public InputAction _reloadSceneAction   { get; private set; }

    //Public Input Data
    public Vector2 Move     { get; private set; }
    public Vector2 Look     { get; private set; }
    public bool Shoot       { get; private set; }
    public bool ReloadScene { get; private set; }

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
        _moveAction         = currentMap.FindAction("Move");
        _lookAction         = currentMap.FindAction("Look");
        _shootAction        = currentMap.FindAction("ShootAction");
        _reloadSceneAction  = currentMap.FindAction("ReloadScene");

        _shootAction.performed          += onShoot;
        _lookAction.performed           += onLook;
        _moveAction.performed           += onMove;
        _reloadSceneAction.performed    += onSceneReload;

        _moveAction.canceled            += onMove;
        _lookAction.canceled            += onLook;
        _shootAction.canceled           += onShoot;
        _reloadSceneAction.canceled     += onSceneReload;
    }
    #endregion

    #region - Input Gethering -
    //The below Statements activates and receive the input data on every inpu action using InputAction CallbackContexts.
    private void onMove(InputAction.CallbackContext context)            => Move         = context.ReadValue<Vector2>();
    private void onShoot(InputAction.CallbackContext context)           => Shoot        = context.ReadValueAsButton();
    private void onLook(InputAction.CallbackContext context)            => Look         = context.ReadValue<Vector2>();
    private void onSceneReload(InputAction.CallbackContext context)     => ReloadScene  = context.ReadValueAsButton();
    #endregion

    #region - Change Input Map -
    // ----------------------------------------------------------------------
    // Name: ChangeInputMap
    // Desc: This method change the current input map, setting up the correct
    //       map for Controller, Keyboard or future controls types.
    // ----------------------------------------------------------------------
    public void ChangeInputMap(string mapToSet)
    {
        playerInput.SwitchCurrentActionMap(mapToSet);
    }
    #endregion
}