using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private const float MAX_CURSOR_WIDTH = 0.2f;

    [SerializeField] private GameObject cursor_prefab;
    [SerializeField] private GameController game_controller;

    private GameObject cursor;
    private Vector3 mouse_position;
    private bool cursor_spawned;
    private GameObject target_collided;
    private float area_assist_radius;
    private Vector3 last_mouse_position;

    private Vector3 mouse_delta { get { return Input.mousePosition - last_mouse_position; } }

    private enum assist_mode
    {
        NONE,
        AREA,
        GRAVITY
    }

    private assist_mode current_assist_mode;

    private void Start()
    {
        cursor_spawned = false;
        GameData.player_misses = 0;

        current_assist_mode = (assist_mode)GameData.latin_square[GameData.LATIN_SQUARE_ROW][GameData.current_round-1];
        print(current_assist_mode);

        last_mouse_position = Input.mousePosition;
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

        if (current_assist_mode == assist_mode.AREA)
        {
            UpdateCursorSize();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }

        Debug.Log(mouse_delta.sqrMagnitude);

        last_mouse_position = Input.mousePosition;
    }

    public void HandleCollision(GameObject other)
    {
        target_collided = other;
    }

    private void UpdateCursorPosition()
    {
        // This is the normal mouse movement with no gravity assistance applied
        mouse_position = Input.mousePosition;
        mouse_position = Camera.main.ScreenToWorldPoint(mouse_position);
        mouse_position.z = 0;
        cursor.transform.position = new Vector3(mouse_position.x, mouse_position.y, 0);

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

            int layer_mask2 = LayerMask.GetMask("Targets");
            RaycastHit2D cc = Physics2D.CircleCast(cursor.transform.position, 0.3f, Vector2.zero, 0.5f, layer_mask2);
            if (cc.collider != null)
            {
                cursor.transform.position = cc.point;
            }
        }
    }

    private void HandleClick()
    {
        if (GameData.game_state == GameData.state.RUNNING)
        {
            int layer_mask = LayerMask.GetMask("Targets");
            
            if (current_assist_mode == assist_mode.NONE)
            {
                // This does normal checking, where we check if there is an object below the cursor.
                // It only checks a single point, and no aim assist is applied.

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
            }
            else if (current_assist_mode == assist_mode.GRAVITY)
            {
                var collider = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.25f, layer_mask);
                if (collider != null)
                {
                    GameObject targ = collider.gameObject;
                    game_controller.Score(targ);
                }
                else if (!collider)
                {
                    game_controller.Miss();
                }
            }
            else if (current_assist_mode == assist_mode.AREA)
            {
                RaycastHit2D hit = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), area_assist_radius, Vector2.zero, .1f, layer_mask);
                if (hit.collider != null)
                {
                    GameObject selected_target = hit.collider.gameObject;
                    game_controller.Score(selected_target);
                }
                else if (!hit)
                {
                    game_controller.Miss();
                }
            }
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

    private void UpdateCursorSize()
    {

    }
}
