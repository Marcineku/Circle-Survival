using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform spawnArea;
    public GreenBomb greenBomb;
    
    void Start()
    {
        StartCoroutine("SpawnBombs");
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

            GreenBomb clone = Instantiate(greenBomb, spawnPosition, Quaternion.identity);
            clone.destructionTimer = Random.Range(2.0f, 4.0f);

            yield return new WaitForSeconds(1);
        }
    }
}
