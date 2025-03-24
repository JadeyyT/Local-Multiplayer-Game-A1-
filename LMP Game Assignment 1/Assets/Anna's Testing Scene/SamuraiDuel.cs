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
    public Text attackerText;
    public Text defenderText;
    public Slider player1HealthBar;
    public Slider player2HealthBar;
    public Text resultText;
    public Text reactionText;

    [Header("Sound Effects")]
    public AudioClip slashSound;
    public AudioClip dodgeSound;
    public AudioClip clapSound;
    public AudioClip winSound;

    private AudioSource audioSource1;
    private AudioSource audioSource2;

    [Header("Controller Input")]
    public ControllerInput player1ControllerInput;
    public ControllerInput player2ControllerInput;

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

        
        player1Health = maxHealth;
        player2Health = maxHealth;
        UpdateHealthBars();
        UpdateUIText();
    }

    void Update()
    {
        if (gameOver) return;

        if (isPlayer1Attacker)
        {
            
            if (Input.GetKeyDown(KeyCode.Space) || (player1ControllerInput != null && player1ControllerInput.isSlashing))
            {
                Debug.Log("Player 1 Attacks!");
                StartAttack(animator1, KeyCode.Return, KeyCode.LeftArrow, KeyCode.RightArrow, animator2, audioSource1, player2ControllerInput);
            }
        }
        else
        {
            
            if (Input.GetKeyDown(KeyCode.Return) || (player2ControllerInput != null && player2ControllerInput.isSlashing))
            {
                Debug.Log("Player 2 Attacks!");
                StartAttack(animator2, KeyCode.Space, KeyCode.A, KeyCode.D, animator1, audioSource2, player1ControllerInput);
            }
        }
    }

    private void StartAttack(Animator attackerAnimator, KeyCode defendKey, KeyCode dodgeLeftKey, KeyCode dodgeRightKey, Animator defenderAnimator, AudioSource attackerAudio, ControllerInput defenderControllerInput)
    {
        if (attackerAnimator == null || defenderAnimator == null)
        {
            Debug.LogError("Animator not assigned!");
            return;
        }

        attackerAnimator.SetTrigger("Slash");

        if (attackerAudio != null && slashSound != null)
            attackerAudio.PlayOneShot(slashSound);

        StartCoroutine(CheckDefend(defendKey, dodgeLeftKey, dodgeRightKey, defenderAnimator, defenderControllerInput));
    }

    private IEnumerator CheckDefend(KeyCode defendKey, KeyCode dodgeLeftKey, KeyCode dodgeRightKey, Animator defenderAnimator, ControllerInput defenderControllerInput)
    {
        bool defended = false;
        bool isClap = false;
        float startTime = Time.time;

        reactionText.gameObject.SetActive(true);
        resultText.text = "DEFEND NOW!";
        resultText.color = Color.yellow;

        while (Time.time < startTime + defendTimeWindow)
        {
            float reactionTime = Time.time - startTime;
            reactionText.text = $"Reaction: {reactionTime:F2}s";
            reactionText.color = Color.Lerp(Color.green, Color.red, reactionTime / defendTimeWindow);

            // 检查输入
            var input = CheckDefendInput(defendKey, dodgeLeftKey, dodgeRightKey, defenderControllerInput);

            if (input.isDefending)
            {
                HandleSuccessfulDefense(defenderAnimator, true, defenderControllerInput);
                defended = isClap = true;
                break;
            }
            else if (input.isDodgingLeft || input.isDodgingRight)
            {
                HandleSuccessfulDefense(defenderAnimator, false, defenderControllerInput);
                defended = true;
                break;
            }

            yield return null;
        }

        reactionText.gameObject.SetActive(false);

        if (!defended)
        {
            HandleFailedDefense();
        }
        else
        {
            StartCoroutine(ClearResultText(1.5f));
            if (isClap)
            {
                isPlayer1Attacker = !isPlayer1Attacker;
                UpdateUIText();
            }
        }
    }

    private (bool isDefending, bool isDodgingLeft, bool isDodgingRight) CheckDefendInput(KeyCode defendKey, KeyCode dodgeLeftKey, KeyCode dodgeRightKey, ControllerInput defenderControllerInput)
    {
        bool isDefending = false;
        bool isDodgingLeft = false;
        bool isDodgingRight = false;

        
        if (Input.GetKeyDown(defendKey))
            isDefending = true;

        if (Input.GetKeyDown(dodgeLeftKey))
            isDodgingLeft = true;

        if (Input.GetKeyDown(dodgeRightKey))
            isDodgingRight = true;

        
        if (defenderControllerInput != null)
        {
            if (defenderControllerInput.isClapping)
                isDefending = true;

            if (defenderControllerInput.isDodgingLeft)
                isDodgingLeft = true;

            if (defenderControllerInput.isDodgingRight)
                isDodgingRight = true;
        }

        return (isDefending, isDodgingLeft, isDodgingRight);
    }

    private void HandleSuccessfulDefense(Animator defenderAnimator, bool isClap, ControllerInput defenderControllerInput)
    {
        if (isClap)
        {
            
            defenderAnimator.SetTrigger("Clap");
            resultText.text = "PERFECT BLOCK!";
            resultText.color = Color.cyan;
            audioSource2.PlayOneShot(clapSound);
        }
        else
        {
            // 闪避逻辑
            if ((defenderControllerInput != null && defenderControllerInput.isDodgingLeft) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                defenderAnimator.SetTrigger("DodgeLeft");
            }
            else if ((defenderControllerInput != null && defenderControllerInput.isDodgingRight) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                defenderAnimator.SetTrigger("DodgeRight");
            }

            resultText.text = "DODGE SUCCESS!";
            resultText.color = Color.green;
            audioSource2.PlayOneShot(dodgeSound);
        }
    }

    private void HandleFailedDefense()
    {
        resultText.text = "TOO SLOW!";
        resultText.color = Color.red;
        DealDamage(isPlayer1Attacker ? "Player 1" : "Player 2");
    }

    private IEnumerator ClearResultText(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultText.text = "";
    }

    private void DealDamage(string attacker)
    {
        if (attacker == "Player 1")
        {
            player2Health = Mathf.Clamp(player2Health - 10, 0, maxHealth);
            if (player2Health <= 0) EndGame("Player 1");
        }
        else
        {
            player1Health = Mathf.Clamp(player1Health - 10, 0, maxHealth);
            if (player1Health <= 0) EndGame("Player 2");
        }
        UpdateHealthBars();
    }

    private void UpdateHealthBars()
    {
        if (player1HealthBar != null)
            player1HealthBar.value = (float)player1Health / maxHealth;

        if (player2HealthBar != null)
            player2HealthBar.value = (float)player2Health / maxHealth;
    }

    private void UpdateUIText()
    {
        if (attackerText != null)
            attackerText.text = $"Attacker: {(isPlayer1Attacker ? "PLAYER 1" : "PLAYER 2")}";

        if (defenderText != null)
            defenderText.text = $"Defender: {(isPlayer1Attacker ? "PLAYER 2" : "PLAYER 1")}";
    }

    [Header("End Game UI")]
    public EndGameUI endGameUI;

    private void EndGame(string winner)
    {
        gameOver = true;
        resultText.text = $"{winner.ToUpper()} VICTORY!";
        AudioSource winnerAudio = winner == "Player 1" ? audioSource1 : audioSource2;

        if (winnerAudio != null && winSound != null)
            winnerAudio.PlayOneShot(winSound);

        if (endGameUI != null)
            endGameUI.ShowVictory(winner);
    }
}