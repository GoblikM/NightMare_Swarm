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

    private void Update()
    {
        //TestSwitchState();
        // Check the current state of the game and update accordingly
        switch (currentState)
        {
            case GameState.Gameplay:
                // Update the gameplay
                break;
            case GameState.Pause:
                // Update the pause menu
                break;
            case GameState.GameOver:
                // Update the game over screen
                break;
            default:
                Debug.LogWarning("Invalid game state");
                break;
        }
    }

    //private void TestSwitchState()
    //{
    //    if(Input.GetKeyDown(KeyCode.E))
    //    {
    //        currentState++;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Q))
    //    {
    //        currentState--;
    //    }
    //}

}
