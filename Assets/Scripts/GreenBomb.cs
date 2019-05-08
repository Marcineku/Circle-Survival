using UnityEngine;
using UnityEngine.UI;

public class GreenBomb : MonoBehaviour
{
    public float destructionTimer;
    public Image clockFillImage;

    private CircleCollider2D circleCollider;
    private Camera mainCamera;

    private float lifeSpan;

    void Start()
    {
        Destroy(gameObject, destructionTimer);
        Destroy(clockFillImage.gameObject, destructionTimer);

        circleCollider = GetComponent<CircleCollider2D>();
        mainCamera = Camera.main;

        lifeSpan = 0.0f;
    }

    void Update()
    {
        lifeSpan += Time.deltaTime;

        clockFillImage.fillAmount = lifeSpan / destructionTimer;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && circleCollider.OverlapPoint(mainCamera.ScreenToWorldPoint(touch.position)))
            {
                Destroy(gameObject);
                Destroy(clockFillImage.gameObject);
            }
        }
    }
}
