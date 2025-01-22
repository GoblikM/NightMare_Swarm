using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Define the different states of the game
    public enum GameState
    {
        Gameplay,
        Pause,
        GameOver
    }

    // Store the current state of the game
    public GameState currentState;

    public GameState previousState;

    [Header("UI")]
    public GameObject pauseMenu;
    public GameObject resultsScreen;

    // Current Stat displays
    public Text currentHealthText;
    public Text currentRecoveryText;
    public Text currentMoveSpeedText;
    public Text currentMightText;
    public Text currentProjectileSpeedText;
    public Text currentPickUpRangeText;

    public bool isGameOver = false;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of the GameManager exists
        if (instance == null) {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DisableScreens();
    }

    private void Update()
    {
        // Check the current state of the game and update accordingly
        switch (currentState)
        {
            case GameState.Gameplay:
                // Update the gameplay
                CheckForPauseAndResume();
                break;
            case GameState.Pause:
                // Update the pause menu
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                // Update the game over screen
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f; // Stop the game
                    Debug.Log("Game Over");
                    DisplayResults();
                }
                break;
            default:
                Debug.LogWarning("Invalid game state");
                break;
        }
    }

    /// <summary>
    /// Change the state of the game
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }


    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame()
    {
        if(currentState != GameState.Pause)
        {
            previousState = currentState;
            ChangeState(GameState.Pause);
            pauseMenu.SetActive(true);
            Time.timeScale = 0f; // Stop the game
            Debug.Log("Game Paused");
        }

    }

    /// <summary>
    /// Resume the game
    /// </summary>
    public void ResumeGame()
    {
       if(currentState == GameState.Pause)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; // Resume the game
            pauseMenu.SetActive(false);
            Debug.Log("Game Resumed");
        }
    }

    /// <summary>
    /// Check for pause and resume input
    /// </summary>
    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Gameplay)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }

        }
    }

    void DisableScreens()
    {
        pauseMenu.SetActive(false);
        resultsScreen.SetActive(false);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }


}
