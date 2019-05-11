using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class made for resetting any static variables,
/// that can't be resetted automatically during scene swap
/// </summary>
public class StaticController : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Bomb.ResetStaticVariables();
    }
}
