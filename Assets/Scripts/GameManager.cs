using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    private void Awake()
    {
        DisablePauseMenu();
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

    void DisablePauseMenu()
    {
        pauseMenu.SetActive(false);
    }


}
