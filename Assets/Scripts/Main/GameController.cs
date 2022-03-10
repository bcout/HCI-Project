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

    private TextController text_controller;

    private bool timer_started;
    private float start_time;
    private float time_elapsed;
    private float time_left;

    private void Awake()
    {
        GameInputActions game_input_actions = new GameInputActions();
        game_input_actions.Enable();
        game_input_actions.Game.Quit.performed += QuitGame;
    }

    private void Start()
    {
        start_time = 0;
        time_elapsed = 0;
        time_left = GameData.TIME_LIMIT;
        timer_started = false;

        text_controller = GetComponent<TextController>();
    }

    private void Update()
    {
        if (GameData.game_state == GameData.state.RUNNING && !timer_started)
        {
            timer_started = true;
            start_time = Time.timeSinceLevelLoad;
        }

        // Calculate the time remaining and update the HUD
        if (GameData.game_state == GameData.state.RUNNING && timer_started)
        {
            time_elapsed = Time.timeSinceLevelLoad - start_time;
            time_left = GameData.TIME_LIMIT - time_elapsed;
            if (time_left <= 0)
            {
                time_left = 0.0f;
                GameData.game_state = GameData.state.DONE;
            }
        }

        text_controller.UpdateHUD(time_left);
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
