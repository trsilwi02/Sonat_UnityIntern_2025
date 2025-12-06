using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    //[Header("Config")]
    //public Transform levelContainer;

    [Header("Levels List (Kéo Prefab Level vào ?ây)")]
    public List<GameObject> allLevels;

    [Header("Runtime Info")]
    public int currentLevelIndex = 0;
    private GameObject currentLevelObject;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= allLevels.Count)
        {
            currentLevelIndex = 0;
        }
        LoadCurrentLevel();
    }

    public void LoadCurrentLevel()
    {
        ClearLevel();

        DOTween.KillAll();

        if (currentLevelIndex >= allLevels.Count) return;

        GameObject levelPrefab = allLevels[currentLevelIndex];

        currentLevelObject = Instantiate(levelPrefab);
        currentLevelObject.transform.localPosition = Vector3.zero;
        Time.timeScale = 1.0f;
    }

    public void ReplayLevel()
    {
        LoadCurrentLevel();
    }

    public void ClearLevel()
    {
        if (currentLevelObject != null)
        {
            Destroy(currentLevelObject);
        }
    }
}