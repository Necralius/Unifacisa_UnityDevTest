using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// --------------------------------------------------------------------------
// Name: GameManager (Class)
// Desc: This class represents an GameManager class, that manages some
//       universal data and systems like the timer and the UI system of the
//       game.
// --------------------------------------------------------------------------
public class GameManager : MonoBehaviour
{
    #region - Singleton Pattern -
    // ----------------------------------------------------------------------
    // Name: Singleton Pattern (Desing Pattern)
    // Desc: This statements represents an Singleton Pattern, an pattern that
    //       garantee that this class hava only one instance in the entire
    //       scene, also garantee that this class will have an global acess
    //       point.
    // ----------------------------------------------------------------------
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
    }
    #endregion

    //Private References
    [Header("Timer System")]
    [SerializeField] private TextMeshProUGUI    _timerText          = null;

    [Header("UI References")]
    [SerializeField] private GameObject         _deathScreen        = null;
    [SerializeField] private string             _damagePopupTag     = null;
    public GameObject mainCanvas;

    [Header("Health System")]
    [SerializeField] private Slider             _healthSlider       = null;

    //Public References
    [HideInInspector] public GameObject         _playerInstance     = null;

    //Private Data
    private float   _timer          = 0f;

    private float   _seconds        = 0f;
    private float   _minutes        = 0f;

    private bool    _playerIsDead   = false;
    public float    _Timer { get => _timer; }
    // ------------------------------------------ Methods ------------------------------------------//

    #region - BuiltIn Methods -
    // ----------------------------------------------------------------------
    // Name: Start
    // Desc: This method is called on the game start, and mainly get the
    //       class dependencies, and reset the timer on the game start.
    // ----------------------------------------------------------------------
    void Start()
    {
        _timer = 0f;
        _playerInstance = CharacterManager.Instance.gameObject;
        _playerIsDead = false;
    }

    // ----------------------------------------------------------------------
    // Name: Update (BuiltIn Method)
    // Desc: This method is called every frame, and mainly manages the game
    //       timer systems.
    // ----------------------------------------------------------------------
    void Update()
    {
        UpdateTick();       
    }
    #endregion

    #region - Timer System -
    // ----------------------------------------------------------------------
    // Name: TimerCounter (Method)
    // Desc: This method execute the timer counter, also, the method convert
    //       the time to minutes and seconds and apply it to an text.
    // ----------------------------------------------------------------------
    private void TimerCounter()
    {
        _timer += Time.deltaTime;

        _timerText.text = string.Format("Time: {0:00}:{1:00}", _minutes, _seconds);

        _minutes = Mathf.FloorToInt(_timer / 60);
        _seconds = Mathf.FloorToInt(_timer % 60);
    }
    #endregion

    #region - Update Actions -
    // ----------------------------------------------------------------------
    // Name: UpdateTick (Method)
    // Desc: For an better code cleaning purpose, this method join some
    //       actions that where called in every update call, uniting them in
    //       an single method, and cleaning the main Update method.
    // ----------------------------------------------------------------------
    private void UpdateTick()
    {
        if (_timerText.text == null) return;
       
        _minutes = Mathf.FloorToInt(_timer / 60);
        _seconds = Mathf.FloorToInt(_timer % 60);

        TimerCounter();

        if (InputManager.Instance._reloadSceneAction.WasPressedThisFrame()) ReloadScene();
    }
    #endregion

    #region - Value Popup System -
    // ----------------------------------------------------------------------
    // Name: SpawnValuePopup
    // Desc: This method spawns an value popup in the position passed as
    //       argument, also, the method setup an color if is an damage value,
    //       or an cure value.
    // ----------------------------------------------------------------------
    public void SpawnValuePopup(Vector3 position, float value, bool isDamage)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(position);
        DamagePopup dm = ObjectPooler.Instance.SpawnFromPool(_damagePopupTag, screenPos, Quaternion.identity).GetComponentInChildren<DamagePopup>();
        dm.SetUp((int)value, isDamage);
    }
    #endregion

    #region - Health Slider Update -
    // ----------------------------------------------------------------------
    // Name: SetHealthSlider
    // Desc: This method updates the health slider.
    // ----------------------------------------------------------------------
    public void SetHealthSlider(float healthValue, float healthMaxValue)
    {
        _healthSlider.value = healthValue;
        _healthSlider.maxValue = healthMaxValue;
    }
    #endregion

    #region - Death System -
    // ----------------------------------------------------------------------
    // Name: PlayerDeath (Method)
    // Desc: This method executes the death actions.
    // ----------------------------------------------------------------------
    public void PlayerDeath()
    {
        _playerIsDead    = true;
        Time.timeScale  = 0f;
        _deathScreen.SetActive(true);
    }
    #endregion

    #region - Scene Management System -

    // ----------------------------------------------------------------------
    // Name: LoadScene (Method)
    // Desc: This method load an scene based on the index passed as argument.
    // ----------------------------------------------------------------------
    public void LoadScene(int SceneToLoad)
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    // ----------------------------------------------------------------------
    // Name: Reload Scene (Method)
    // Desc: This method basically reloads the current game scene.
    // ----------------------------------------------------------------------
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}