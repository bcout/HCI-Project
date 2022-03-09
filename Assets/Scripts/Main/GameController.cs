using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private TargetSpawner target_spawner;

    [SerializeField]
    private TextMeshProUGUI score;

    private void Awake()
    {
        GameInputActions game_input_actions = new GameInputActions();
        game_input_actions.Enable();
        game_input_actions.Game.Quit.performed += QuitGame;
    }

    private void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        score.text = "Score: " + GameData.player_score;
    }

    private void QuitGame(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    // When the player successfully hits a target, this function is called
    public void Score(GameObject target)
    {
        Destroy(target);
        GameData.player_score++;
        target_spawner.SpawnTarget();
    }
}
