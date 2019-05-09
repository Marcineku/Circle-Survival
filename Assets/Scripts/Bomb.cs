using UnityEngine;

public abstract class Bomb : MonoBehaviour
{
    public float destructionTimer;

    private CircleCollider2D circleCollider;
    private Camera mainCamera;

    protected virtual void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        mainCamera = Camera.main;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destructionTimer);
    }

    protected virtual void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && circleCollider.OverlapPoint(mainCamera.ScreenToWorldPoint(touch.position)))
            {
                OnTouch();
                Destroy(gameObject);
            }
        }
    }

    protected abstract void OnTouch();
}
