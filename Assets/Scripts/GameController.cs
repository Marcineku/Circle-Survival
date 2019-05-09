using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public const float timeToStart = 1.0f;
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
    public GameObject gameOverPanel; 
    public Text currentScore;
    public Text highScore;
    public Text highScoreInfo;
    
    public AnimationCurve bombSpawnIntervalCurve;
    public AnimationCurve greenBombDestructionTimeMinCurve;
    public AnimationCurve greenBombDestructionTimeMaxCurve;

    private float gameTime;

    private float bombSpawnInterval;
    private float greenBombDestructionTimeMin;
    private float greenBombDestructionTimeMax;

    private bool isGameRunning;
    private bool isGameOver;
    
    public void GameOver()
    {
        isGameOver = true;
        isGameRunning = false;
        float highScorevalue = PlayerPrefs.GetFloat("HighScore", 0.0f);
        currentScore.text = "Your score: " + gameTime.ToString("0.00") + " s";
        highScore.text = "High score: " + highScorevalue.ToString("0.00") + " s";

        if (gameTime > highScorevalue)
        {
            PlayerPrefs.SetFloat("HighScore", gameTime);
            highScoreInfo.text = "New record!";
        }
        else
        {
            highScoreInfo.text = "Not quite the record.";
        }

        gameOverPanel.SetActive(true);
        gameTimeText.gameObject.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine("SpawnBombs");

        gameTime = 0.0f;
        isGameRunning = false;
        isGameOver = false;

        bombSpawnInterval = bombSpawnIntervalCurve.Evaluate(gameTime);
        greenBombDestructionTimeMin = greenBombDestructionTimeMinCurve.Evaluate(gameTime);
        greenBombDestructionTimeMax = greenBombDestructionTimeMaxCurve.Evaluate(gameTime);

        gameTimeText.text = gameTime.ToString("0.00");

        gameTimeText.gameObject.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (isGameOver)
        {
            if (Input.touchCount > 0)
            {
                SceneManager.LoadScene(0);
            }
        }

        if (isGameRunning)
        {
            gameTime += Time.deltaTime;

            bombSpawnInterval = bombSpawnIntervalCurve.Evaluate(gameTime);
            greenBombDestructionTimeMin = greenBombDestructionTimeMinCurve.Evaluate(gameTime);
            greenBombDestructionTimeMax = greenBombDestructionTimeMaxCurve.Evaluate(gameTime);

            gameTimeText.text = gameTime.ToString("0.00");
        }
    }

    private IEnumerator SpawnBombs()
    {
        yield return new WaitForSeconds(timeToStart);
        isGameRunning = true;

        while (true)
        {
            GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");

            if (bombs.Length <= maxBombCount && isGameRunning)
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

    private Vector2 GetRandomSpawnPosition(GameObject[] bombs)
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

    private void SpawnGreenBomb(Vector2 spawnPosition)
    {
        float destructionTimer = Random.Range(greenBombDestructionTimeMin, greenBombDestructionTimeMax);
        GreenBomb greenBombClone = Instantiate(greenBomb, spawnPosition, Quaternion.identity);
        Image clockFillImageClone = Instantiate(clockFillImage, spawnPosition, Quaternion.identity, clockFillCanvas.transform);

        greenBombClone.destructionTimer = destructionTimer;
        greenBombClone.clockFillImage = clockFillImageClone;
    }

    private void SpawnBlackBomb(Vector2 spawnPosition)
    {
        BlackBomb blackBombClone = Instantiate(blackBomb, spawnPosition, Quaternion.identity);
        blackBombClone.destructionTimer = blackBombDestructionTimer;
    }
}
