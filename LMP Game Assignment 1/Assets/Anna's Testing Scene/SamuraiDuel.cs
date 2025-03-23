using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Health Settings")]
    public int maxHealth = 100;
    private int player1Health;
    private int player2Health;

    [Header("UI Elements")]
    public Text attackerText; // UI Text for attacker
    public Text defenderText; // UI Text for defender
    public Slider player1HealthBar; // Health bar for Player 1
    public Slider player2HealthBar; // Health bar for Player 2

    [Header("Sound Effects")]
    public AudioClip slashSound;
    public AudioClip dodgeSound;
    public AudioClip clapSound;
    public AudioClip winSound;

    private AudioSource audioSource1;
    private AudioSource audioSource2;

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
            Debug.LogError("AudioSource components not found on players!");

        // Initialize health
        player1Health = maxHealth;
        player2Health = maxHealth;

        // Update UI
        UpdateHealthBars();
        UpdateUIText();
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

        if (attackerAudio != null && slashSound != null)
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
            if (Input.GetKeyDown(defendKey))
            {
                Debug.Log("Defend (Clap) Successful!");
                defenderAnimator.SetTrigger("Clap");
                defended = true;
                isClap = true;

                if (audioSource2 != null && clapSound != null)
                    audioSource2.PlayOneShot(clapSound);

                break;
            }
            else if (Input.GetKeyDown(dodgeLeftKey))
            {
                Debug.Log($"Dodge Left Successful! Key pressed: {dodgeLeftKey}");
                defenderAnimator.SetTrigger("DodgeLeft");
                defended = true;

                if (audioSource2 != null && dodgeSound != null)
                    audioSource2.PlayOneShot(dodgeSound);

                break;
            }
            else if (Input.GetKeyDown(dodgeRightKey))
            {
                Debug.Log($"Dodge Right Successful! Key pressed: {dodgeRightKey}");
                defenderAnimator.SetTrigger("DodgeRight");
                defended = true;

                if (audioSource2 != null && dodgeSound != null)
                    audioSource2.PlayOneShot(dodgeSound);

                break;
            }
            yield return null;
        }

        if (!defended)
        {
            Debug.Log("Defend Failed!");
            DealDamage(isPlayer1Attacker ? "Player 1" : "Player 2");
        }
        else
        {
            if (isClap)
            {
                isPlayer1Attacker = !isPlayer1Attacker;
                Debug.Log("Roles Switched! New Attacker: " + (isPlayer1Attacker ? "Player 1" : "Player 2"));
                UpdateUIText();
            }
        }
    }

    private void DealDamage(string attacker)
    {
        if (attacker == "Player 1")
        {
            player2Health -= 10; // Reduce Player 2's health
            if (player2Health <= 0)
            {
                player2Health = 0;
                EndGame("Player 1");
            }
        }
        else
        {
            player1Health -= 10; // Reduce Player 1's health
            if (player1Health <= 0)
            {
                player1Health = 0;
                EndGame("Player 2");
            }
        }

        UpdateHealthBars();
    }

    private void UpdateHealthBars()
    {
        if (player1HealthBar != null)
            player1HealthBar.value = player1Health;

        if (player2HealthBar != null)
            player2HealthBar.value = player2Health;
    }

    private void UpdateUIText()
    {
        if (attackerText != null)
            attackerText.text = "Attacker: " + (isPlayer1Attacker ? "Player 1" : "Player 2");

        if (defenderText != null)
            defenderText.text = "Defender: " + (isPlayer1Attacker ? "Player 2" : "Player 1");
    }

    private void EndGame(string winner)
    {
        gameOver = true;
        Debug.Log(winner + " WINS!");

        AudioSource winnerAudio = (winner == "Player 1") ? audioSource1 : audioSource2;
        if (winnerAudio != null && winSound != null)
            winnerAudio.PlayOneShot(winSound);
    }
}