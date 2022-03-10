﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    

    [SerializeField]
    private TextMeshProUGUI score;

    private TextController text_controller;
    private TargetSpawner target_spawner;

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
        target_spawner = GameObject.Find("TargetSpawner").GetComponent<TargetSpawner>();
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

        if (GameData.game_state == GameData.state.DONE)
        {
            GameData.scores[GameData.current_round - 1] = GameData.player_score;
            GameData.player_score = 0;

            if (GameData.current_round >= GameData.MAX_ROUNDS)
            {
                SceneManager.LoadScene("End");
            }
            else
            {
                SceneManager.LoadScene("BetweenRounds");
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
        if (target_spawner == null)
        {
            target_spawner = GameObject.Find("TargetSpawner").GetComponent<TargetSpawner>();
        }
        target_spawner.SpawnTarget();
    }
}
