using UnityEngine;
using UnityEngine.UI;

public class GreenBomb : Bomb
{
    public Image clockFillImage;
    
    private bool isTapped;
    private GameObject clockFillCanvas;
    private Image clockFillImageClone;

    protected override void OnAwake()
    {
        isTapped = false;

        clockFillCanvas = GameObject.FindGameObjectWithTag("ClockFillCanvas");
    }

    private void Start()
    {
        clockFillImageClone = Instantiate(clockFillImage, transform.position, Quaternion.identity, clockFillCanvas.transform);
    }

    protected override void OnUpdate()
    {
        clockFillImageClone.fillAmount = LifeSpan / destructionTimer;
    }

    protected override void OnTap()
    {
        isTapped = true;
    }

    private void OnDestroy()
    {
        if (clockFillImageClone != null)
        {
            Destroy(clockFillImageClone.gameObject);
        }

        if (!isTapped && !OneDidExploded)
        {
            OneDidExploded = true;
        }
    }
}
