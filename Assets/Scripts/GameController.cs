using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public delegate void GameStateAction(bool isGameRunning);
    public static event GameStateAction OnGameStateChanged;

    public const float timeToStart = 1.0f;
    public const float timeBeforeCanLeaveGameOverScreen = 1.0f;
    public const float blackBombDestructionTimer = 3.0f;
    public const float blackBombAppearanceTimeFraction = 0.1f;
    public const int maxBombCount = 50;
    public const int maxRandomPositionIterations = 1000;

    public Text gameTimeText;
    public Transform spawnArea;
    public GreenBomb greenBomb;
    public BlackBomb blackBomb;
    public GameObject gameOverPanel; 
    public Text currentScore;
    public Text highScore;
    public Text highScoreInfo;
    
    public AnimationCurve bombSpawnIntervalCurve;
    public AnimationCurve greenBombDestructionTimeMinCurve;
    public AnimationCurve greenBombDestructionTimeMaxCurve;

    private float bombSpawnInterval;
    private float greenBombDestructionTimeMin;
    private float greenBombDestructionTimeMax;
    private float gameTime;
    private float gameOverTime;
    private float greenBombRadius;

    private bool isGameOver;

    private bool isGameRunning;
    private bool IsGameRunning
    {
        get { return isGameRunning; }
        set
        {
            isGameRunning = value;
            OnGameStateChanged?.Invoke(value);
        }
    }

    private Vector2 spawnAreaOrigin;
    private Vector2 spawnAreaRange;

    private void Awake()
    {
        gameTime = 0.0f;
        gameOverTime = 0.0f;
        isGameOver = false;
        isGameRunning = false;
        greenBombRadius = greenBomb.GetComponent<CircleCollider2D>().radius * greenBomb.transform.localScale.x;
        spawnAreaOrigin = spawnArea.position;
        spawnAreaRange = spawnArea.localScale / 2.0f;

        gameTimeText.text = gameTime.ToString("0.00");
    }

    private void Start()
    {
        StartCoroutine(SpawnBombs());
        
        gameTimeText.gameObject.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (isGameOver)
        {
            // TODO: Get input from Input Controller 
            gameOverTime += Time.deltaTime;
            if (Input.touchCount > 0 && gameOverTime >= timeBeforeCanLeaveGameOverScreen)
            {
                SceneManager.LoadScene(0);
            }
        }

        if (IsGameRunning)
        {
            gameTime += Time.deltaTime;
            gameTimeText.text = gameTime.ToString("0.00");
        }
    }

    private IEnumerator SpawnBombs()
    {
        yield return new WaitForSeconds(timeToStart);
        IsGameRunning = true;

        while (true)
        {
            GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb");

            if (bombs.Length <= maxBombCount && IsGameRunning)
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

            bombSpawnInterval = bombSpawnIntervalCurve.Evaluate(gameTime);
            yield return new WaitForSeconds(bombSpawnInterval);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bombs"></param>
    /// <returns></returns>
    private Vector2 GetRandomSpawnPosition(GameObject[] bombs)
    {
        int iterations = 0;
        Vector2 spawnPosition;
        bool isPositionInvalid;
        do
        {
            ++iterations;
            Vector2 randomRange = new Vector2(Random.Range(-spawnAreaRange.x + greenBombRadius, spawnAreaRange.x - greenBombRadius),
                                              Random.Range(-spawnAreaRange.y + greenBombRadius, spawnAreaRange.y - greenBombRadius));
            spawnPosition = randomRange + spawnAreaOrigin;
            isPositionInvalid = bombs.Any(bomb => Vector2.Distance(bomb.transform.position, spawnPosition) < greenBombRadius * 2);
        } while (isPositionInvalid && iterations <= maxRandomPositionIterations);

        return spawnPosition;
    }

    private void SpawnGreenBomb(Vector2 spawnPosition)
    {
        greenBombDestructionTimeMin = greenBombDestructionTimeMinCurve.Evaluate(gameTime);
        greenBombDestructionTimeMax = greenBombDestructionTimeMaxCurve.Evaluate(gameTime);
        float destructionTimer = Random.Range(greenBombDestructionTimeMin, greenBombDestructionTimeMax);

        GreenBomb greenBombClone = Instantiate(greenBomb, spawnPosition, Quaternion.identity);
        greenBombClone.destructionTimer = destructionTimer;
    }

    private void SpawnBlackBomb(Vector2 spawnPosition)
    {
        BlackBomb blackBombClone = Instantiate(blackBomb, spawnPosition, Quaternion.identity);
        blackBombClone.destructionTimer = blackBombDestructionTimer;
    }

    private void OnEnable()
    {
        Bomb.OnOneExploded += Bomb_OnOneExploded;
    }

    private void OnDisable()
    {
        Bomb.OnOneExploded -= Bomb_OnOneExploded;
    }

    private void Bomb_OnOneExploded()
    {
        IsGameRunning = false;

        float highScorevalue = PlayerPrefs.GetFloat("HighScore", 0.0f);
        currentScore.text = "Your score: " + gameTime.ToString("0.00") + " s";
        highScore.text = "High score: " + highScorevalue.ToString("0.00") + " s";

        gameTime = (float)System.Math.Round(gameTime, 2);
        highScorevalue = (float)System.Math.Round(highScorevalue, 2);

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

        isGameOver = true;
    }
}
