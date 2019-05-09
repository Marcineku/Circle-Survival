using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
}
