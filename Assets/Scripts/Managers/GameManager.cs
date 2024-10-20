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
    }

    private void Start()
    {
        AudioManager.Instance.PlayLevelOneMusic();
    }

    public void OnPlayerDeath()
    {
        PlayerStatManager.Instance.ResetStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().name == "L1")
        {
            SceneManager.LoadScene("L2");
        } 
        else if (SceneManager.GetActiveScene().name == "L2")
        {
            SceneManager.LoadScene("L3");
        }
        else
        {
            SceneManager.LoadScene("EndGameCredits");
        }
    }
}
