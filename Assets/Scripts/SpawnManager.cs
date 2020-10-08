using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject[] obstaclePrefab;

    private float startDelay = 2.0f;

    private Vector3 spawnPos = new Vector3(25, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        // reference to the game manager script
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        // starts obstacle spawning
        Invoke("SpawnObstacles", startDelay);
    }

    // spawns barriers at a random rate outside the sceneview while game is active
    void SpawnObstacles()
    {
        int index = Random.Range(0, obstaclePrefab.Length);

        if (!gameManager.gameOver)
        {
            Instantiate(obstaclePrefab[index], spawnPos, obstaclePrefab[index].transform.rotation);

            Invoke("SpawnObstacles", Random.Range(1.0f, 4.0f));
        }
    }
}
