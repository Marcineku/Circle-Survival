using UnityEngine;
using UnityEngine.UI;

public class GreenBomb : Bomb
{
    public Image clockFillImage;

    private float lifeSpan;
    private bool isTouched;
    private GameController gameController;

    protected override void Start()
    {
        lifeSpan = 0.0f;
        isTouched = false;
        Destroy(clockFillImage.gameObject, destructionTimer);
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

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
        isTouched = true;
        Destroy(clockFillImage.gameObject);
    }

    private void OnDestroy()
    {
        if (!isTouched)
        {
            gameController.GameOver();
        }
    }
}
