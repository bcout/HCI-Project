using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    private void Awake()
    {
        GameInputActions game_input_actions = new GameInputActions();
        game_input_actions.Enable();
        game_input_actions.Game.Quit.performed += QuitGame;
    }

    private void QuitGame(InputAction.CallbackContext context)
    {
        Application.Quit();
    }
}
