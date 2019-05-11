using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public delegate void GameStateAction(bool isGameRunning);
    public static event GameStateAction OnGameStateChanged;

    public const float timeToStart = 1.0f;
    public const float timeBeforeCanLeaveGameOverScreen = 0.5f;
    public const float blackBombDestructionTimer = 3.0f;
    public const float blackBombAppearanceTimeFraction = 0.1f;
    public const int maxBombCount = 50;
    public const int maxRandomPositionIterations = 1000;

    public Text gameTimeText;
    public Animator gameTimeAnimator;
    public Transform spawnArea;
    public GreenBomb greenBomb;
    public BlackBomb blackBomb;
    public GameObject gameOverPanel; 
    public Text currentScore;
    public Text highScoreInfo;
    public AudioSource gameOverSound;
    
    public AnimationCurve bombSpawnIntervalCurve;
    public AnimationCurve greenBombDestructionTimeMinCurve;
    public AnimationCurve greenBombDestructionTimeMaxCurve;
    
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

    private Dictionary<int, GameObject> bombs;

    private int gameTimeAnimTrigger10;
    private int trigger10Hash;

    private void Awake()
    {
        gameTime = 0.0f;
        gameOverTime = 0.0f;
        isGameOver = false;
        isGameRunning = false;
        greenBombRadius = greenBomb.GetComponent<CircleCollider2D>().radius * greenBomb.transform.localScale.x;
        spawnAreaOrigin = spawnArea.position;
        spawnAreaRange = spawnArea.localScale / 2.0f;
        bombs = new Dictionary<int, GameObject>();
        gameTimeAnimTrigger10 = 10;
        trigger10Hash = Animator.StringToHash("Hit10");

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
        float deltaTime = Time.deltaTime;

        if (isGameOver)
        {
            gameOverTime += deltaTime;
        }

        if (IsGameRunning)
        {
            gameTime += deltaTime;
            gameTimeText.text = gameTime.ToString("0.00");

            if (gameTime >= gameTimeAnimTrigger10)
            {
                gameTimeAnimTrigger10 += 10;
                gameTimeAnimator.SetTrigger(trigger10Hash);
            }
        }
    }

    private IEnumerator SpawnBombs()
    {
        yield return new WaitForSeconds(timeToStart);
        IsGameRunning = true;

        while (true)
        {
            GameObject[] bombsArray = bombs.Values.ToArray();

            if (bombsArray.Length <= maxBombCount && IsGameRunning)
            {
                Vector2 spawnPosition = GetRandomSpawnPosition(bombsArray);

                if (Random.value <= blackBombAppearanceTimeFraction)
                {
                    SpawnBlackBomb(spawnPosition);
                }
                else
                {
                    SpawnGreenBomb(spawnPosition);
                }
            }
            
            yield return new WaitForSeconds(bombSpawnIntervalCurve.Evaluate(gameTime));
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
        float destructionTimer = Random.Range(greenBombDestructionTimeMinCurve.Evaluate(gameTime),
                                              greenBombDestructionTimeMaxCurve.Evaluate(gameTime));

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
        Bomb.OnCreated += Bomb_OnCreated;
        Bomb.OnDestroyed += Bomb_OnDestroyed;
        Bomb.OnOneExploded += Bomb_OnOneExploded;
        InputController.OnTap += InputController_OnTap;
    }

    private void OnDisable()
    {
        Bomb.OnCreated -= Bomb_OnCreated;
        Bomb.OnDestroyed -= Bomb_OnDestroyed;
        Bomb.OnOneExploded -= Bomb_OnOneExploded;
        InputController.OnTap -= InputController_OnTap;
    }
    
    private void Bomb_OnCreated(GameObject bomb)
    {
        bombs.Add(bomb.GetInstanceID(), bomb);
    }

    private void Bomb_OnDestroyed(GameObject bomb)
    {
        bombs.Remove(bomb.GetInstanceID());
    }

    private void Bomb_OnOneExploded()
    {
        if (!isGameOver)
        {
            IsGameRunning = false;
            isGameOver = true;

            float highScorevalue = PlayerPrefs.GetFloat("HighScore", 0.0f);
            highScorevalue = (float)System.Math.Round(highScorevalue, 2);
            gameTime = (float)System.Math.Round(gameTime, 2);

            currentScore.text = "Score: " + gameTime.ToString("0.00") + " s";

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

            gameOverSound.Play();
        }
    }
    
    private void InputController_OnTap(Vector2 tapPosition)
    {
        if (gameOverTime >= timeBeforeCanLeaveGameOverScreen)
        {
            SceneManager.LoadScene(0);
        }
    }
}
