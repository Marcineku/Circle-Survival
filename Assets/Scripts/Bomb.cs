using UnityEngine;

public abstract class Bomb : MonoBehaviour
{
    public delegate void ExplosionAction();
    public static event ExplosionAction OnOneExploded;

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
    protected abstract void OnUpdate();
    protected abstract void OnTap();

    private CircleCollider2D circleCollider;

    protected void Awake()
    {
        LifeSpan = 0.0f;
        IsActive = true;

        circleCollider = GetComponent<CircleCollider2D>();

        OnAwake();
    }

    protected void Update()
    {
        if (IsActive)
        {
            LifeSpan += Time.deltaTime;

            if (LifeSpan >= destructionTimer)
            {
                Destroy(gameObject);
            }
            else
            {
                OnUpdate();
            }
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
            Destroy(gameObject);
        }
    }

    private void GameController_OnGameStateChanged(bool isGameRunning)
    {
        IsActive = isGameRunning;
    }
}
