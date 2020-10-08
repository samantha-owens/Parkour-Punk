using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    Animator playerAnim;
    Animator npc1Anim;
    Animator npc2Anim;

    public bool isGameActive = false;

    // Start is called before the first frame update
    void Start()
    {
        // reference to the player and NPC icons animator components
        playerAnim = GameObject.Find("Player Icon").GetComponent<Animator>();
        npc1Anim = GameObject.Find("NPC1 Icon").GetComponent<Animator>();
        npc2Anim = GameObject.Find("NPC2 Icon").GetComponent<Animator>();

        playerAnim.SetFloat("Speed_f", 0.0f);
        npc1Anim.SetFloat("Speed_f", 0.0f);
        npc2Anim.SetFloat("Speed_f", 0.0f);

        // player the player and NPC animations for the title screen
        StartCoroutine(PlayerIdle());
        StartCoroutine(NPC1Idle());
        StartCoroutine(NPC2Idle());
    }

    // idle animation for player icon
    IEnumerator PlayerIdle()
    {
        while(!isGameActive)
        {
            playerAnim.SetInteger("Animation_int", 8);
            yield return new WaitForSeconds(6);
            playerAnim.SetInteger("Animation_int", 5);
            yield return new WaitForSeconds(3);
        }
    }

    // idle animation for NPC 1 Icon
    IEnumerator NPC1Idle()
    {
        while (!isGameActive)
        {
            npc1Anim.SetInteger("Animation_int", 1);
            yield return new WaitForSeconds(3);
            npc1Anim.SetInteger("Animation_int", 3);
            yield return new WaitForSeconds(1.5f);
        }
    }

    // idle animation for NPC 2 Icon
    IEnumerator NPC2Idle()
    {
        while (!isGameActive)
        {
            npc2Anim.SetInteger("Animation_int", 2);
            yield return new WaitForSeconds(4.5f);
            npc2Anim.SetInteger("Animation_int", 7);
            yield return new WaitForSeconds(1.5f);
        }
    }

    // loads next scene and begins game, attached to START button in UI
    public void StartGame()
    {
        isGameActive = true;
        SceneManager.LoadScene("Prototype 3");
    }
}
