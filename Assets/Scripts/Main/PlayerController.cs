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

    // Used to initalize the event-driven input system
    private void Awake()
    {
        // PlayerInputActions is an auto-generated class containing all the input actions we defined in the editor
        PlayerInputActions player_input_actions = new PlayerInputActions();
        
        // They start off as disabled, so enable the player's input detection
        player_input_actions.Enable();

        // Subscribe to the "click" event. HandleClick() will be called when this input is detected.
        player_input_actions.Player.Click.performed += HandleClick;
    }

    private void Start()
    {
        cursor_spawned = false;
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
    }

    private void UpdateCursorPosition()
    {
        mouse_position = Input.mousePosition;
        mouse_position = Camera.main.ScreenToWorldPoint(mouse_position);
        mouse_position.z = 0;
        cursor.transform.position = new Vector3(mouse_position.x, mouse_position.y, 0);
    }

    private void HandleClick(InputAction.CallbackContext context)
    {
        int layer_mask = LayerMask.GetMask("Targets");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, .1f, layer_mask);

        if (hit.collider != null)
        {
            GameObject selected_target = hit.collider.gameObject;
            game_controller.Score(selected_target);
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
