using UnityEngine;

/// <summary>
/// Abstract class that every bomb type should inherit from,
/// fires events when bomb is created, destroyed or if bomb exploded,
/// checks if bomb has been tapped, destroys the bomb after destruction time passed and after tap.
/// Deactivates bomb timer when game is not running (listens for game state change event from Game Controller).
/// </summary>
public abstract class Bomb : MonoBehaviour
{
    public delegate void ExplosionAction();
    public static event ExplosionAction OnOneExploded;

    public delegate void CreationAction(GameObject bomb);
    public static event CreationAction OnCreated;

    public delegate void DestructionAction(GameObject bomb);
    public static event DestructionAction OnDestroyed;

    private static bool oneDidExploded = false;
    protected static bool OneDidExploded
    {
        get { return oneDidExploded; }
        set
        {
            oneDidExploded = value;
            if (value)
            {
                OnOneExploded?.Invoke();
            }
        }
    }

    public static void ResetStaticVariables()
    {
        oneDidExploded = false;
    }

    [HideInInspector]
    public float destructionTimer;

    protected float LifeSpan { get; private set; }
    protected bool IsActive { get; private set; }

    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void OnUpdate();
    protected abstract void OnKill();
    protected abstract void OnTap();

    private CircleCollider2D circleCollider;

    private bool isDestroyed;

    protected void Awake()
    {
        LifeSpan = 0.0f;
        IsActive = true;
        isDestroyed = false;

        circleCollider = GetComponent<CircleCollider2D>();

        OnCreated?.Invoke(gameObject);

        OnAwake();
    }

    protected void Start()
    {
        OnStart();
    }

    protected void Update()
    {
        if (IsActive)
        {
            LifeSpan += Time.deltaTime;

            if (LifeSpan >= destructionTimer)
            {
                isDestroyed = true;
                Destroy(gameObject);
            }
            else
            {
                OnUpdate();
            }
        }
    }

    protected void OnDestroy()
    {
        if (isDestroyed)
        {
            OnDestroyed?.Invoke(gameObject);

            OnKill();
        }
    }

    private void OnEnable()
    {
        InputController.OnTap += InputController_OnTap;
        GameController.OnGameStateChanged += GameController_OnGameStateChanged;
    }

    private void OnDisable()
    {
        InputController.OnTap -= InputController_OnTap;
        GameController.OnGameStateChanged -= GameController_OnGameStateChanged;
    }

    private void InputController_OnTap(Vector2 tapPosition)
    {
        if (circleCollider.OverlapPoint(tapPosition))
        {
            OnTap();
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    private void GameController_OnGameStateChanged(bool isGameRunning)
    {
        IsActive = isGameRunning;
    }
}
