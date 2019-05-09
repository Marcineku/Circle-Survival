using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public const float bombInitialSpawnInterval = 2.0f;
    public const float greenBombInitialDestructionTimeMin = 2.0f;
    public const float greenBombInitialDestructionTimeMax = 4.0f;
    public const float blackBombDestructionTimer = 3.0f;
    public const float blackBombAppearanceTimeFraction = 0.1f;
    public const int maxBombCount = 50;
    public const int maxRandomPositionIterations = 1000;

    public Text gameTimeText;

    public Transform spawnArea;
    public GreenBomb greenBomb;
    public Image clockFillImage;
    public Canvas clockFillCanvas;
    public BlackBomb blackBomb;
    
    public AnimationCurve bombSpawnIntervalCurve;
    public AnimationCurve greenBombDestructionTimeMinCurve;
    public AnimationCurve greenBombDestructionTimeMaxCurve;

    private float gameTime;

    private float bombSpawnInterval;
    private float greenBombDestructionTimeMin;
    private float greenBombDestructionTimeMax;

    void Start()
    {
        StartCoroutine("SpawnBombs");

        gameTime = 0.0f;
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        bombSpawnInterval = bombSpawnIntervalCurve.Evaluate(gameTime);
        greenBombDestructionTimeMin = greenBombDestructionTimeMinCurve.Evaluate(gameTime);
        greenBombDestructionTimeMax = greenBombDestructionTimeMaxCurve.Evaluate(gameTime);

        gameTimeText.text = gameTime.ToString("0.00");
    }

    IEnumerator SpawnBombs()
    {
        while (true)
        {
            GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");

            if (bombs.Length <= maxBombCount)
            {
                Vector2 spawnPosition = GetRandomSpawnPosition(bombs);

                if (Random.value <= blackBombAppearanceTimeFraction)
                {
                    SpawnBlackBomb(spawnPosition);
                }
                else
                {
                    SpawnGreenBomb(spawnPosition);
                }
            }

            yield return new WaitForSeconds(bombSpawnInterval);
        }
    }

    Vector2 GetRandomSpawnPosition(GameObject[] bombs)
    {
        float bombRadius = greenBomb.GetComponent<CircleCollider2D>().radius;
        Vector2 origin = spawnArea.position;
        Vector2 range = spawnArea.localScale / 2.0f;

        int iterations = 0;
        Vector2 spawnPosition;
        int count;
        do
        {
            ++iterations;
            Vector2 randomRange = new Vector2(Random.Range(-range.x + bombRadius, range.x - bombRadius),
                                              Random.Range(-range.y + bombRadius, range.y - bombRadius));
            spawnPosition = randomRange + origin;
            count = bombs.Count(bomb => Vector2.Distance(bomb.transform.position, spawnPosition) < bombRadius * 2);
        } while (count > 0 && iterations < maxRandomPositionIterations);

        return spawnPosition;
    }

    void SpawnGreenBomb(Vector2 spawnPosition)
    {
        float destructionTimer = Random.Range(greenBombDestructionTimeMin, greenBombDestructionTimeMax);
        GreenBomb greenBombClone = Instantiate(greenBomb, spawnPosition, Quaternion.identity);
        Image clockFillImageClone = Instantiate(clockFillImage, spawnPosition, Quaternion.identity, clockFillCanvas.transform);

        greenBombClone.destructionTimer = destructionTimer;
        greenBombClone.clockFillImage = clockFillImageClone;
    }

    void SpawnBlackBomb(Vector2 spawnPosition)
    {
        BlackBomb blackBombClone = Instantiate(blackBomb, spawnPosition, Quaternion.identity);
        blackBombClone.destructionTimer = blackBombDestructionTimer;
    }
}
