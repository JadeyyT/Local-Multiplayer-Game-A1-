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

    void Start()
    {
        
        if (player1 != null)
            animator1 = player1.GetComponent<Animator>();
        if (player2 != null)
            animator2 = player2.GetComponent<Animator>();

        if (animator1 == null || animator2 == null)
            Debug.LogError("Animator components not found on players!");
    }

    void Update()
    {
        if (gameOver) return; // Stop game 

        if (isPlayer1Attacker)
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Player 1 attacks
            {
                Debug.Log("Player 1 Slash!");
                StartAttack(animator1, KeyCode.Return, animator2);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return)) // Player 2 attacks
            {
                Debug.Log("Player 2 Slash!");
                StartAttack(animator2, KeyCode.Space, animator1);
            }
        }
    }

    private void StartAttack(Animator attackerAnimator, KeyCode defendKey, Animator defenderAnimator)
    {
        if (attackerAnimator == null || defenderAnimator == null)
        {
            Debug.LogError("Animator not assigned!");
            return;
        }

        attackerAnimator.SetTrigger("Slash"); // Play attack animation
        StartCoroutine(CheckDefend(defendKey, defenderAnimator));
    }

    private IEnumerator CheckDefend(KeyCode defendKey, Animator defenderAnimator)
    {
        bool defended = false;
        float startTime = Time.time;

        while (Time.time < startTime + defendTimeWindow)
        {
            if (Input.GetKeyDown(defendKey))
            {
                Debug.Log("Defend key pressed!");
                defenderAnimator.SetTrigger("Clap"); // Play defend animation
                defended = true;
                break;
            }
            yield return null;
        }

        if (!defended)
        {
            Debug.Log("Defend failed!");
            EndGame(isPlayer1Attacker ? "Player 1" : "Player 2");
        }
        else
        {
            Debug.Log("Defend succeeded!");
            isPlayer1Attacker = !isPlayer1Attacker; // Swap attacker
        }
    }

    private void EndGame(string winner)
    {
        gameOver = true;
        Debug.Log(winner + " WINS!");
        // whoever is doing this Add game over logic here (e.g., show UI, restart game)
    }
}