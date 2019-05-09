using UnityEngine;
using UnityEngine.UI;

public class GreenBomb : Bomb
{
    public Image clockFillImage;

    private float lifeSpan;

    protected override void Start()
    {
        lifeSpan = 0.0f;
        Destroy(clockFillImage.gameObject, destructionTimer);

        base.Start();
    }

    protected override void Update()
    {
        lifeSpan += Time.deltaTime;
        clockFillImage.fillAmount = lifeSpan / destructionTimer;
        
        base.Update();
    }

    protected override void OnTouch()
    {
        Destroy(clockFillImage.gameObject);
    }
}
