using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private TextController text_controller;
    private TargetSpawner target_spawner;
    private AudioSource audio_source;

    private bool timer_started;
    private float start_time;
    private float time_elapsed;
    private float time_left;

    private void Start()
    {
        start_time = 0;
        time_elapsed = 0;
        time_left = GameData.TIME_LIMIT;
        timer_started = false;

        text_controller = GetComponent<TextController>();
        target_spawner = GameObject.Find("TargetSpawner").GetComponent<TargetSpawner>();
        audio_source = GetComponent<AudioSource>();
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
                GameData.game_state = GameData.state.UNLOADING;
            }
        }

        if (GameData.game_state == GameData.state.DONE)
        {
            GameData.scores[GameData.current_round - 1] = GameData.player_score;
            GameData.misses[GameData.current_round - 1] = GameData.player_misses;
            GameData.player_score = 0;
            GameData.player_misses = 0;

            if (GameData.current_round >= GameData.MAX_ROUNDS)
            {
                SceneManager.LoadScene("End", LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene("BetweenRounds", LoadSceneMode.Single);
            }
        }

        text_controller.UpdateHUD(time_left);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    // When the player successfully hits a target, this function is called
    public void Score(GameObject target)
    {
        audio_source.Play(0);
        Destroy(target);
        GameData.player_score++;
        if (target_spawner == null)
        {
            target_spawner = GameObject.Find("TargetSpawner").GetComponent<TargetSpawner>();
        }
        target_spawner.SpawnTarget();
    }

    public void Miss()
    {
        GameData.player_misses++;
    }
}
