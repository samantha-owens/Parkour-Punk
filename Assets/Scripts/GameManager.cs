using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;
    public Transform startingPoint;

    public AudioSource audioSource;
    public AudioClip audioClip1;
    public AudioClip audioClip2;

    public TextMeshProUGUI scoreTextField;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI highScoreTextField;
    public TextMeshProUGUI newHighScoreText;
    public Button restartButton;

    public bool gameOver = false;
    public bool introPlayed = false;

    public int score;
    public int highScore;

    public float lerpSpeed;

    void Start()
    {
        // reference to the player controller script
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // gets high score from previous game
        highScore = PlayerPrefs.GetInt("highscore", 0);

        // reset the value of the score and updates score UIs
        score = 0;
        scoreTextField.text = score.ToString();
        highScoreTextField.text = highScore.ToString();

        // sets up first song on the audio source
        audioSource.clip = audioClip1;
        audioSource.Play();

        // disable player motion controls for intro scene
        gameOver = true;
        StartCoroutine(PlayIntro());
    }

    // Update is called once per frame
    void Update()
    {
        AddScore();
    }

    // walk in animation for player
    IEnumerator PlayIntro()
    {
        // determine the start and end positions
        Vector3 startPos = playerController.transform.position;
        Vector3 endPos = startingPoint.position;

        // determine the distance between the start and end
        float journeyLength = Vector3.Distance(startPos, endPos);
        float startTime = Time.time;

        // determine the distance that has been covered so far
        float distanceCovered = (Time.time - startTime) * lerpSpeed;
        // deteremine the fraction of the distance over the length of the journey
        float fractionOfJourney = distanceCovered / journeyLength;

        // adjust the animation speed of the player
        playerController.GetComponent<Animator>().SetFloat("Speed_f", 0.4f);
        playerController.dirtParticle.Stop();

        // when fractionOfJourey variable is more than 1, we have reached the end of the intro/start of the game 
        while (fractionOfJourney < 1)
        {
            distanceCovered = (Time.time - startTime) * lerpSpeed;
            fractionOfJourney = distanceCovered / journeyLength;
            playerController.transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
            yield return null;
        }

        playerController.GetComponent<Animator>().SetFloat("Speed_f", 1.0f);
        playerController.dirtParticle.Play();
        gameOver = false;
        introPlayed = true;
    }

    // accumulates score based on how far player makes it in the game
    public void AddScore()
    {
        if (!gameOver)
        {
            if (playerController.doubleSpeed)
            {
                score += 2;
            }
            else
            {
                score++;
            }
            scoreTextField.text = score.ToString();
        }
    }

    // flashes high score text if new high score is achieved
    public IEnumerator FlashText()
    {
        while (gameOver && introPlayed)
        {
            newHighScoreText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            newHighScoreText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    // if the new score is higher than a previously saved one, the new score is the high score
    public void SaveScore()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
        }
    }

    // initiates game over and enables player to restart the game
    public void GameOver()
    {
        gameOver = true;
        audioSource.clip = audioClip2;
        audioSource.Play();

        SaveScore();
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        // if player achieves new high score, UI updates and high score animation plays
        if (score > highScore)
        {
            StartCoroutine(FlashText());
            highScoreTextField.text = score.ToString();
        }
    }

    // restarts the scene, attached to the RESTART button in UI
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
