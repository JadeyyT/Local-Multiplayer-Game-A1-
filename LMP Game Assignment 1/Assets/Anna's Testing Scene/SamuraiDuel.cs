using System.Collections;
using UnityEngine;

public class SamuraiDuel : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private Animator animator1;
    private Animator animator2;
    private bool isPlayer1Attacker = true;
    private bool gameOver = false;

    [Header("Game Settings")]
    public float defendTimeWindow = 0.5f;

    //Sound System
    private AudioSource audioSource1;
    private AudioSource audioSource2;

    [Header("Sound Effects")] // Sound Clips
    public AudioClip slashSound;
    public AudioClip dodgeSound;
    public AudioClip clapSound;
    public AudioClip winSound;

    void Start()
    {
        if (player1 != null)
        {
            animator1 = player1.GetComponent<Animator>();
            audioSource1 = player1.GetComponent<AudioSource>(); 
        }
        if (player2 != null)
        {
            animator2 = player2.GetComponent<Animator>();
            audioSource2 = player2.GetComponent<AudioSource>();
        }

        if (animator1 == null || animator2 == null)
            Debug.LogError("Animator components not found on players!");

        if (audioSource1 == null || audioSource2 == null)
            Debug.LogError("AudioSource components not found  on players!"); 
    }

    void Update()
    {
        if (gameOver) return;

        if (isPlayer1Attacker)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Player 1 Attacks!");
                StartAttack(animator1, KeyCode.Return, KeyCode.LeftArrow, KeyCode.RightArrow, animator2, audioSource1);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Player 2 Attacks!");
                StartAttack(animator2, KeyCode.Space, KeyCode.A, KeyCode.D, animator1, audioSource2);
            }
        }
    }

    private void StartAttack(Animator attackerAnimator, KeyCode defendKey, KeyCode dodgeLeftKey, KeyCode dodgeRightKey, Animator defenderAnimator, AudioSource attackerAudio)
    {
        if (attackerAnimator == null || defenderAnimator == null)
        {
            Debug.LogError("Animator not assigned!");
            return;
        }

        attackerAnimator.SetTrigger("Slash");

        if (attackerAudio != null && slashSound != null) //Play Slash Sound
            attackerAudio.PlayOneShot(slashSound);

        StartCoroutine(CheckDefend(defendKey, dodgeLeftKey, dodgeRightKey, defenderAnimator));
    }

    private IEnumerator CheckDefend(KeyCode defendKey, KeyCode dodgeLeftKey, KeyCode dodgeRightKey, Animator defenderAnimator)
    {
        bool defended = false;
        bool isClap = false;
        float startTime = Time.time;

        Debug.Log($"Defender needs to press: [Defend] {defendKey}, [Dodge Left] {dodgeLeftKey}, [Dodge Right] {dodgeRightKey}");

        while (Time.time < startTime + defendTimeWindow)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    Debug.Log($"Key pressed: {key}");
                }
            }

            if (Input.GetKeyDown(defendKey))
            {
                Debug.Log("Defend (Clap) Successful!");
                defenderAnimator.SetTrigger("Clap");
                defended = true;
                isClap = true;

                if (audioSource2 != null && clapSound != null) // Play Clap Sound
                    audioSource2.PlayOneShot(clapSound);

                break;
            }
            else if (Input.GetKeyDown(dodgeLeftKey))
            {
                Debug.Log($"Dodge Left Successful! Key pressed: {dodgeLeftKey}");
                defenderAnimator.SetTrigger("DodgeLeft");
                defended = true;

                if (audioSource2 != null && dodgeSound != null) //Play Dodge Sound
                    audioSource2.PlayOneShot(dodgeSound);

                break;
            }
            else if (Input.GetKeyDown(dodgeRightKey))
            {
                Debug.Log($"Dodge Right Successful! Key pressed: {dodgeRightKey}");
                defenderAnimator.SetTrigger("DodgeRight");
                defended = true;

                if (audioSource2 != null && dodgeSound != null) //Play Dodge Sound
                    audioSource2.PlayOneShot(dodgeSound);

                break;
            }
            yield return null;
        }

        if (!defended)
        {
            Debug.Log("Defend Failed!");
            EndGame(isPlayer1Attacker ? "Player 1" : "Player 2");
        }
        else
        {
            if (isClap)
            {
                isPlayer1Attacker = !isPlayer1Attacker;
                Debug.Log("Roles Switched! New Attacker: " + (isPlayer1Attacker ? "Player 1" : "Player 2"));
            }
        }
    }

    private void EndGame(string winner)
    {
        gameOver = true;
        Debug.Log(winner + " WINS!");

        AudioSource winnerAudio = (winner == "Player 1") ? audioSource1 : audioSource2;
        
        if (winnerAudio != null && winSound != null) // Play Win Sound
            winnerAudio.PlayOneShot(winSound);
    }
}
