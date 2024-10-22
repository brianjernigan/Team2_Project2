using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene sceneName, LoadSceneMode mode)
    {
        PlayerController.Instance.SetSpawnPoint();
    }

    public void OnPlayerDeath()
    {
        PlayerStatManager.Instance.ResetStats();
        SceneManager.LoadScene("L1");
    }

    public void OnPlayerVictory()
    {
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "L1":
                SceneManager.LoadScene("L2");
                break;
            case "L2":
                SceneManager.LoadScene("L3");
                break;
            default:
                TransitionToEndCredits();
                break;
        }
    }

    private void TransitionToEndCredits()
    {
        SceneManager.LoadScene("EndGameCredits");
    }
}
