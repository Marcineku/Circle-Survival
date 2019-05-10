using UnityEngine;
using UnityEngine.SceneManagement;

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
