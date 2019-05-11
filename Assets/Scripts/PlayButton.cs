using UnityEngine;
using UnityEngine.SceneManagement;

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
