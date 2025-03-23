using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject endGamePanel;    // 结束面板
    public Text victoryText;           // 胜利文本
    public Button restartButton;       // 重新开始按钮
    public Button menuButton;          // 主菜单按钮
    public Button exitButton;          // 退出按钮

    [Header("Settings")]
    public string MenuScene = "Menu"; // 主菜单场景名称

    void Start()
    {
        // 初始化隐藏面板
        endGamePanel.SetActive(false);

        // 绑定按钮事件
        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(ReturnToMenu);
        exitButton.onClick.AddListener(ExitGame);
    }

    // 显示结束界面（在其他脚本中调用）
    public void ShowVictory(string winnerName)
    {
        endGamePanel.SetActive(true);
        victoryText.text = $"{winnerName.ToUpper()} VICTORY!";
        Time.timeScale = 0f; // 暂停游戏
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
        SceneManager.LoadScene(MenuScene); // 加载主菜单场景
    }

    private void ExitGame()
    {
        Application.Quit();

        // 编辑器模式下退出
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}