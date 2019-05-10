public class BlackBomb : Bomb
{
    protected override void OnAwake()
    { }

    protected override void OnUpdate()
    { }

    protected override void OnTap()
    {
        if (!OneDidExploded)
        {
            OneDidExploded = true;
        }
    }
}
