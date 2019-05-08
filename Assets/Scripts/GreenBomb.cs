using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBomb : MonoBehaviour
{
    public float destructionTimer;

    private CircleCollider2D collider;
    private Camera camera;

    void Start()
    {
        Destroy(gameObject, destructionTimer);

        collider = GetComponent<CircleCollider2D>();
        camera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && collider.OverlapPoint(camera.ScreenToWorldPoint(touch.position)))
            {
                Destroy(gameObject);
            }
        }
    }
}
