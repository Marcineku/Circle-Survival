using UnityEngine;

public class InputController : MonoBehaviour
{
    public delegate void TapAction(Vector2 tapPosition);
    public static event TapAction OnTap;

    public Camera mainCamera;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                OnTap?.Invoke(mainCamera.ScreenToWorldPoint(touch.position));
            }
        }
    }
}
