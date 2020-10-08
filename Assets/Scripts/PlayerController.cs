using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;
    private GameManager gameManager;

    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSound;
    public AudioClip crashSound;

    public float jumpForce;
    public float doubleJumpForce;

    public bool isOnGround = true;
    public bool doubleJumpUsed = false;
    public bool doubleSpeed = false;

    // Start is called before the first frame update
    void Start()
    {
        // reference to the game manager script
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        // preference to the player's rigidbody, animator, and audio source components
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        PlayerJump();
        DoubleSpeed();
    }

    void PlayerJump()
    {
        // if space is pressed & player is on the ground, they will jump
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameManager.gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // sets isOnGround bool to false so player can't jump infinitely in the air
            isOnGround = false;

            // sets doublejumpused bool to false so player can jump once more while in the air
            doubleJumpUsed = false;

            // stops dirt particles and starts jump animation and sound
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
        // if space is pressed again while the player is in the air, they will double jump
        else if (Input.GetKeyDown(KeyCode.Space) && !doubleJumpUsed && !isOnGround && !gameManager.gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // sets doublejumpused bool to true so player cannot jump more than twice
            doubleJumpUsed = true;

            // starts jump animation and sound
            playerAnim.SetTrigger("Jump_trig");
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
    }

    // if left shift is held down, player is able to run at double speed
    void DoubleSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            doubleSpeed = true;
            playerAnim.SetFloat("Speed_Multiplier", 2.0f);
        }
        else if (doubleSpeed)
        {
            doubleSpeed = false;
            playerAnim.SetFloat("Speed_Multiplier", 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // marks when player returns to the ground so they will be able to jump again
        if (collision.gameObject.CompareTag("Ground") && !gameManager.gameOver)
        {
            isOnGround = true;
            dirtParticle.Play();
        }
        // declares "game over" when player runs into an obstacle
        else if (collision.gameObject.CompareTag("Obstacle") && !gameManager.gameOver)
        {
            gameManager.GameOver();

            // stops running dirt particle and starts player death animations and sound
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }
}
