using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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

    [Header("Screens")]
    public GameObject pauseMenu;
    public GameObject resultsScreen;

    // Current Stat displays
    [Header("Current Stats")]
    public Text currentHealthText;
    public Text currentRecoveryText;
    public Text currentMoveSpeedText;
    public Text currentMightText;
    public Text currentProjectileSpeedText;
    public Text currentPickUpRangeText;

    [Header("Results Screen Text")]
    public Image chosenCharacterImage;
    public Text chosenCharacterName;
    public Text levelReachedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);



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

    public void AssignChosenCharacterUI(CharacterSO character)
    {
        chosenCharacterImage.sprite = character.Icon;
        chosenCharacterName.text = character.Name;
    }

    public void AssignLevelReachedUI(int levelReached)
    {
        levelReachedDisplay.text = levelReached.ToString();
    }

    public void AssignChosenWeaponsAndPassiveItemsUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemData)
    {
        if (chosenWeaponsData.Count != chosenWeaponsUI.Count || chosenPassiveItemData.Count != chosenPassiveItemsUI.Count)
        {
            Debug.Log("Chosen weapons and passive items data lists have differennt lenghts");
            return;
        }

        // Assign chosen weapons data to chosenWeaponsUI
        for (int i = 0; i < chosenWeaponsUI.Count; i++)
        {
            // Check if the weapon has a sprite
            if (chosenWeaponsData[i].sprite)
            {
                // Enable corresponding UI image and assign the sprite
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;
            }
            else
            {
                // If the sprite is null, disable the UI image
                chosenWeaponsUI[i].enabled = false;
            }
        }

        // Assign chosen weapons data to chosenPassiveItemsUI
        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            // Check if the passive item has a sprite
            if (chosenPassiveItemData[i].sprite)
            {
                // Enable corresponding UI image and assign the sprite
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemData[i].sprite;
            }
            else
            {
                // If the sprite is null, disable the UI image
                chosenPassiveItemsUI[i].enabled = false;
            }
        }

    }
}
