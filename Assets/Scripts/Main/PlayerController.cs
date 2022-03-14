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
    private GameObject target_collided;

    private AudioSource audioSource;

    private enum assist_mode
    {
        NONE,
        GRAVITY,
        AREA
    }

    private assist_mode current_assist_mode;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        cursor_spawned = false;
        GameData.player_misses = 0;

        // Round 1: warmup
        // Round 2: control 
        // Round 3: gravity
        // Round 4: gravity
        // Round 5: control
        // Round 6: area
        // Round 7: area
        if (GameData.current_round == 3 || GameData.current_round == 4)
        {
            current_assist_mode = assist_mode.GRAVITY;
        }
        else if (GameData.current_round == 1 || GameData.current_round == 7)
        {
            current_assist_mode = assist_mode.AREA;
        }
        else
        {
            current_assist_mode = assist_mode.NONE;
        }

        target_collided = null;
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
                    audioSource.Play(0);
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
                    audioSource.Play(0);
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
                if (target_collided != null)
                {
                    game_controller.Score(target_collided);
                }
                else
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
}
