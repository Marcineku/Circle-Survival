using UnityEngine;

public class BlackBomb : Bomb
{
    public ParticleSystem explosion;

    private AudioSource explosionSound;

    protected override void OnAwake()
    {
        explosionSound = GameObject.FindGameObjectWithTag("BombBlackExplosion").GetComponent<AudioSource>();
    }
    
    protected override void OnStart()
    { }

    protected override void OnUpdate()
    { }

    protected override void OnKill()
    { }

    protected override void OnTap()
    {
        if (!OneDidExploded)
        {
            OneDidExploded = true;
            explosionSound.Play();
            ParticleSystem explosionClone = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(explosionClone, explosionClone.main.duration);
        }
    }
}
