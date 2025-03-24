using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject endGamePanel;    
    public Text victoryText;           
    public Button restartButton;       
    public Button menuButton;          
    public Button exitButton;          

    [Header("Settings")]
    public string MenuScene = "Menu"; 

    void Start()
    {
        
        endGamePanel.SetActive(false);

        
        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(ReturnToMenu);
        exitButton.onClick.AddListener(ExitGame);
    }

    
    public void ShowVictory(string winnerName)
    {
        endGamePanel.SetActive(true);
        victoryText.text = $"{winnerName.ToUpper()} VICTORY!";
        Time.timeScale = 0f; 
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ReturnToMenu()
    {
        Debug.Log($"Attempting to load scene: {MenuScene}");
        Time.timeScale = 1f;
        SceneManager.LoadScene(MenuScene); 
    }

    private void ExitGame()
    {
        Application.Quit();

        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}