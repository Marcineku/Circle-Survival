using UnityEngine;
using UnityEngine.UI;

public class GreenBomb : Bomb
{
    public Image clockFillImage;
    public ParticleSystem explosion;
    
    private bool isTapped;
    private Animator animator;
    private int progressHash;
    private AudioSource fuseSound;
    private AudioSource defuseSound;
    private AudioSource explosionSound;
    private GameObject clockFillCanvas;
    private Image clockFillImageClone;

    protected override void OnAwake()
    {
        isTapped = false;
        animator = GetComponent<Animator>();
        progressHash = Animator.StringToHash("Progress");
        fuseSound = GetComponent<AudioSource>();
        defuseSound = GameObject.FindGameObjectWithTag("BombGreenDefuse").GetComponent<AudioSource>();
        explosionSound = GameObject.FindGameObjectWithTag("BombGreenExplosion").GetComponent<AudioSource>();

        clockFillCanvas = GameObject.FindGameObjectWithTag("ClockFillCanvas");
    }
    
    protected override void OnStart()
    {
        clockFillImageClone = Instantiate(clockFillImage, transform.position, Quaternion.identity, clockFillCanvas.transform);
        fuseSound.Play();
    }

    protected override void OnUpdate()
    {
        float progress = LifeSpan / destructionTimer;

        clockFillImageClone.fillAmount = progress;
        animator.SetFloat(progressHash, progress);
    }

    protected override void OnKill()
    {
        if (clockFillImageClone != null)
        {
            Destroy(clockFillImageClone.gameObject);
        }

        if (!isTapped && !OneDidExploded)
        {
            OneDidExploded = true;

            explosionSound.Play();
            ParticleSystem explosionClone = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(explosionClone, explosionClone.main.duration);
        }
    }

    protected override void OnTap()
    {
        isTapped = true;
        defuseSound.Play();
    }
}
