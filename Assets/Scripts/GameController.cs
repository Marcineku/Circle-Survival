using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Canvas canvas;
    public Transform spawnArea;
    public GreenBomb greenBomb;
    public Image clockFillImage;
    public Camera mainCamera;
    public Text gameTimeText;

    private float gameTime;
    
    void Start()
    {
        StartCoroutine("SpawnBombs");

        gameTime = 0.0f;
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        gameTimeText.text = gameTime.ToString("0.00");
    }

    IEnumerator SpawnBombs()
    {
        while (true)
        {
            Vector2 origin = spawnArea.position;
            Vector2 range = spawnArea.localScale / 2.0f;
            Vector2 randomRange = new Vector2(Random.Range(-range.x, range.x),
                                              Random.Range(-range.y, range.y));
            Vector2 spawnPosition = origin + randomRange;

            SpawnGreenBomb(spawnPosition);

            yield return new WaitForSeconds(1);
        }
    }

    void SpawnGreenBomb(Vector2 spawnPosition)
    {
        float destructionTimer = Random.Range(2.0f, 4.0f);
        GreenBomb greenBombClone = Instantiate(greenBomb, spawnPosition, Quaternion.identity);
        Image clockFillImageClone = Instantiate(clockFillImage, mainCamera.WorldToScreenPoint(spawnPosition), Quaternion.identity, canvas.transform);

        greenBombClone.destructionTimer = destructionTimer;
        greenBombClone.clockFillImage = clockFillImageClone;
    }
}
