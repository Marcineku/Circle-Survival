using UnityEngine;

public class BlackBomb : Bomb
{
    private GameController gameController;

    protected override void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        base.Awake();
    }

    protected override void Start()
    {

        base.Start();
    }

    protected override void Update()
    {

        base.Update();
    }

    protected override void OnTouch()
    {
        gameController.GameOver();
    }
}
