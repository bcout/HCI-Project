using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject cursor_prefab;

    [SerializeField]
    private GameController game_controller;

    private GameObject cursor;
    private Vector3 mouse_position;
    private bool cursor_spawned;

    private enum assist_mode
    {
        NONE,
        GRAVITY,
        AREA
    }

    private assist_mode current_assist_mode;

    // Used to initalize the event-driven input system
    private void Awake()
    {
        /*
        // PlayerInputActions is an auto-generated class containing all the input actions we defined in the editor
        player_input_actions = new PlayerInputActions();
        
        // They start off as disabled, so enable the player's input detection
        player_input_actions.Enable();

        // Subscribe to the "click" event. HandleClick() will be called when this input is detected.
        player_input_actions.Player.Click.performed -= HandleClick;
        player_input_actions.Player.Click.performed += HandleClick;
        */
    }

    private void Start()
    {
        cursor_spawned = false;
        GameData.player_misses = 0;

        // Round 1: warmup
        // Round 2: control 
        // Round 3: gravity
        // Round 4: control
        // Round 5: area
        if (GameData.current_round == 3)
        {
            current_assist_mode = assist_mode.GRAVITY;
        }
        else if (GameData.current_round == 5)
        {
            current_assist_mode = assist_mode.AREA;
        }
        else
        {
            current_assist_mode = assist_mode.NONE;
        }
    }

    // Update is called once per frame
    void Update()
    {
        LockCursorToGameWindow();
        if (GameData.game_state == GameData.state.RUNNING)
        {
            if (!cursor_spawned)
            {
                SpawnCursor();
            }
            UpdateCursorPosition();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    private void UpdateCursorPosition()
    {
        // This is the normal mouse movement with no gravity assistance applied
        mouse_position = Input.mousePosition;
        mouse_position = Camera.main.ScreenToWorldPoint(mouse_position);
        mouse_position.z = 0;
        cursor.transform.position = new Vector3(mouse_position.x, mouse_position.y, 0);

        /*
         * Uncomment this to enable the aim assist modes
         * 
        if (current_assist_mode == assist_mode.NONE || current_assist_mode == assist_mode.AREA)
        {
            // This is the normal mouse movement with no gravity assistance applied
            mouse_position = Input.mousePosition;
            mouse_position = Camera.main.ScreenToWorldPoint(mouse_position);
            mouse_position.z = 0;
            cursor.transform.position = new Vector3(mouse_position.x, mouse_position.y, 0);
        }
        else if (current_assist_mode == assist_mode.GRAVITY)
        {
            // Use gravity to assist the player's aim
        }
        */
    }

    private void HandleClick()
    {
        if (GameData.game_state == GameData.state.RUNNING)
        {
            int layer_mask = LayerMask.GetMask("Targets");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, .1f, layer_mask);
            if (hit.collider != null)
            {
                GameObject selected_target = hit.collider.gameObject;
                game_controller.Score(selected_target);
            }
            else if (!hit)
            {
                game_controller.Miss();
            }

            /*
             * Uncomment this to enable the aim assist modes
             * 
            if (current_assist_mode == assist_mode.NONE || current_assist_mode == assist_mode.GRAVITY)
            {
                // This does normal checking, where we check if there is an object below the cursor.
                // It only checks a single point, and no aim assist is applied.
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, .1f, layer_mask);
                if (hit.collider != null)
                {
                    GameObject selected_target = hit.collider.gameObject;
                    game_controller.Score(selected_target);
                }
                else
                {
                    GameData.player_misses++;
                }

                //
            }
            else if (current_assist_mode == assist_mode.AREA)
            {
                // Use the area cursor to detect when the player has selected a target
            }
            */
        }



    }

    public void SpawnCursor()
    {
        // Instantiate the cursor for the player
        cursor = Instantiate(cursor_prefab, transform.position, transform.rotation, transform);
        cursor.name = "Cursor";
        cursor_spawned = true;
    }

    private void LockCursorToGameWindow()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}
