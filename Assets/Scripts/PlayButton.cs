using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Added delay on when player can hit start and play the game,
/// due to situation when player could hit play automatically after
/// accepting game over information
/// </summary>
public class PlayButton : MonoBehaviour
{
    public const float timeAfterCanPlay = 0.5f;

    private float timeInMainMenu;

    public void LoadSceneByIndex(int index)
    {
        if (timeInMainMenu > timeAfterCanPlay)
        {
            SceneManager.LoadScene(index);
        }
    }

    private void Awake()
    {
        timeInMainMenu = 0.0f;
    }

    private void Update()
    {
        timeInMainMenu += Time.deltaTime;
    }
}
