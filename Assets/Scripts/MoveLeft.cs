using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    GameManager gameManager;
    PlayerController playerController;

    [SerializeField] float speed = 25.0f;
    [SerializeField] float leftBound = -15;

    private void Start()
    {
        // reference to the game manager and player controller scripts
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        MoveObstacle();
        DestroyObstacle();
    }

    // obstacles and background will move to the left constantly
    void MoveObstacle()
    {
        if (!gameManager.gameOver)
        {
            // if the player is using double speed, obstacles and background will move at double speed too
            if (playerController.doubleSpeed)
            {
                transform.Translate(Vector3.left * Time.deltaTime * (speed * 2));
            }
            else
            {
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
        }
    }

    // destroys obstacles once they exit sceneview
    void DestroyObstacle()
    {
        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
