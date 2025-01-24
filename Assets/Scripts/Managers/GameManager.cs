using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Define the different states of the game
    public enum GameState
    {
        Gameplay,
        Pause,
        GameOver,
        LevelUp
    }

    // Store the current state of the game
    public GameState currentState;

    public GameState previousState;

    [Header("Damage Text Settings")]
    public Canvas damageTextCanvas;
    public float textFontSize = 20;
    public TMP_FontAsset textFont;
    public Camera referenceCamera;

    [Header("Screens")]
    public GameObject pauseMenu;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    // Current Stat displays
    [Header("Current Stats")]
    public TMP_Text currentHealthText;
    public TMP_Text currentRecoveryText;
    public TMP_Text currentMoveSpeedText;
    public TMP_Text currentMightText;
    public TMP_Text currentProjectileSpeedText;
    public TMP_Text currentPickUpRangeText;

    [Header("Results Screen Text")]
    public Image chosenCharacterImage;
    public TMP_Text chosenCharacterName;
    public TMP_Text levelReachedDisplay;
    public TMP_Text timeSurvivedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float timeLimit; // The time limit in seconds
    float stopwatchTime; // The current time on the stopwatch
    public TMPro.TMP_Text stopWatchDisplay;

    // flag to check if the game is over
    public bool isGameOver = false;

    // Flag to check if the player is choosing their upgrades
    public bool choosingUpgrade;

    // Reference to the player object
    public GameObject playerObject;

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
                UpdateStopWatch();
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
                    MusicManager.instance.PlayMusic(instance.resultsScreen.GetComponent<AudioSource>().clip);
                }
                break;
            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f; // Pause the game
                    Debug.Log("Level Up");
                    levelUpScreen.SetActive(true);
                    SoundFXManager.instance.PlaySoundFX(instance.levelUpScreen.GetComponent<AudioSource>().clip, instance.levelUpScreen.transform);
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
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopWatchDisplay.text;
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

    void UpdateStopWatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopWatchDisplay();

        if (stopwatchTime >= timeLimit)
        {
            playerObject.SendMessage("Die");
        }
    }

    void UpdateStopWatchDisplay()
    {
        // calculate the number if minutes and seconds that have elapsed
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);
        // Update the stopwatch display
        stopWatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f; // Resume the game
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }


    IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, float duration = 1f, float speed = 50f)
    {
        GameObject textObject = new GameObject("Floating Text");
        RectTransform rect = textObject.AddComponent<RectTransform>();
        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.horizontalAlignment = HorizontalAlignmentOptions.Center;
        textComponent.verticalAlignment = VerticalAlignmentOptions.Middle;
        textComponent.fontSize = textFontSize;
        if (textFont) textComponent.font = textFont;

        // Zkontrolujeme existenci targetu
        if (target != null)
            rect.position = referenceCamera.WorldToScreenPoint(target.position);
        else
            rect.position = Vector3.zero;

        textObject.transform.SetParent(instance.damageTextCanvas.transform);

        WaitForEndOfFrame wait = new();
        float t = 0;
        float yOffset = 0;

        while (t < duration)
        {
            yield return wait;
            t += Time.deltaTime;

            // Ovìø, zda textObject nebyl znièen
            if (rect == null)
                yield break; // Ukonèí coroutine, pokud je objekt znièen

            // Zmìna barvy textu
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1 - t / duration);

            // Kontrola existence targetu
            if (target != null)
            {
                yOffset += speed * Time.deltaTime;
                rect.position = referenceCamera.WorldToScreenPoint(target.position + new Vector3(0, yOffset));
            }
        }

        // Na konci zniè textObject (pokud už nebyl znièen)
        if (textObject != null)
            Destroy(textObject);
    }

    public static void GenerateFloatingText(string text, Transform target, float duration = 1f, float speed = 1f)
    {
        if (!instance.damageTextCanvas) return;

        if(!instance.referenceCamera) instance.referenceCamera = Camera.main;

        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(
            text, target, duration, speed
            ));
    }
}
