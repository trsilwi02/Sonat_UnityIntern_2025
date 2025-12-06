using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panel")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject homePanel;

    [Header("Buttons")]
    [SerializeField] private Button pauseButton;
    //[SerializeField] private Button resumeButton;
    //[SerializeField] private Button homeButton;
    //[SerializeField] private Button restartButton;
    //[SerializeField] private Button nxlvButton;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        // Gán s? ki?n cho các nút (Best Practice thay vì kéo th? trong Inspector)
        //if (pauseButton) pauseButton.onClick.AddListener(PausePanelOn);
        //if (resumeButton) resumeButton.onClick.AddListener(PausePanelOff);
        //if (homeButton) homeButton.onClick.AddListener(HomeClicked);
        //if (restartButton) restartButton.onClick.AddListener(RestartClicked);
        //if (nxlvButton) nxlvButton.onClick.AddListener(NextLevelClicked);
    }

    public void PausePanelOn()
    {
        pausePanel.SetActive(true);
    }

    public void PausePanelOff()
    {
        pausePanel.SetActive(false);
    }

    public void HomeClicked()
    {
        Time.timeScale = 1;
        homePanel.SetActive(true);
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        LevelManager.Instance.ClearLevel();
    }

    public void RestartClicked()
    {
        Time.timeScale = 1;
        LevelManager.Instance.LoadCurrentLevel();
        pausePanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);
    }

    public void NextLevelClicked()
    {
        if (winPanel) winPanel.SetActive(false);
        LevelManager.Instance.LoadNextLevel();
    }

    public void WinPanelOn()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void LosePanelOn()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Play()
    {
        Time.timeScale = 1;
        LevelManager.Instance.LoadCurrentLevel();
        homePanel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }
}
