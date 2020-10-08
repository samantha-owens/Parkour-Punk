using System.Collections;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;
    private Animator npcAnim;

    [SerializeField] float speed = 15f;
    [SerializeField] float leftBound = -15;
    

    private void Start()
    {
        // reference to the game manager and player controller scripts
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // reference to the NPC animator component
        npcAnim = GetComponent<Animator>();
    }

    void Update()
    {
        NPCMove();
        DestroyNPC();

        // game over animation for NPC
        if (gameManager.gameOver)
        {
            npcAnim.SetInteger("Animation_int", 2);

            // Alt NPC death animation
            // objectAnim.SetBool("Death_b", true);
            // objectAnim.SetInteger("DeathType_int", 1);
        }
    }

    // NPC will run to the left constantly
    void NPCMove()
    {
        if (!gameManager.gameOver)
        {
            // if the player is using double speed, NPC will move at double speed too
            if (playerController.doubleSpeed)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * (speed * 2));
                npcAnim.SetFloat("Speed_f", 0.6f);
            }
            else
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                npcAnim.SetFloat("Speed_f", 0.3f);
            }
        }
    }

    // destroys obstacles once they exit sceneview
    void DestroyNPC()
    {
        if (transform.position.x < leftBound)
        {
            Destroy(gameObject);
        }
    }
}
