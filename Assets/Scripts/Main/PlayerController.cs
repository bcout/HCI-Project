using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private const float CURSOR_RADIUS = 0.2f;

    [SerializeField] private GameObject cursor_prefab;
    [SerializeField] private GameController game_controller;
    [SerializeField] private Sprite target_sprite, cursor_sprite;

    private GameObject cursor;
    private Rigidbody2D cursor_rb;
    private Vector3 mouse_position;
    private bool cursor_spawned;

    private float G = 11f;

    private GameObject[] targets;

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

        current_assist_mode = (assist_mode)GameData.latin_square[GameData.latin_square_row][GameData.current_round-1];
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
            
            if (Input.GetMouseButtonDown(0))
            {
                HandleClick();
            }
        }
    }

    private void UpdateCursorPosition()
    {

        // This is the normal mouse movement with no gravity assistance applied
        
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

            mouse_position = Input.mousePosition;
            mouse_position = Camera.main.ScreenToWorldPoint(mouse_position);
            mouse_position.z = 0;
            //cursor.transform.position = new Vector3(mouse_position.x, mouse_position.y, 0);

            cursor_rb.gravityScale = 12f;
            cursor_rb.mass = 1f;

            targets = GameObject.FindGameObjectsWithTag("Target");

            Vector3 wipi = new Vector3(mouse_position.x, mouse_position.y, 0);
            Vector3 wi_sum = new Vector3(1, 1, 0);

            foreach (GameObject tgt in targets)
            {
                Rigidbody2D tgt_rb = tgt.GetComponent<Rigidbody2D>();

                tgt_rb.mass = 4;

                Vector3 x = (mouse_position - tgt_rb.transform.position);
                
                if (x.x < 0)
                {
                    x.x = -1 * x.x;
                }

                if (x.y < 0)
                {
                    x.y = -1 * x.y;
                }

                Vector3 wi = new Vector3( G / (Mathf.Pow(x.x,5) + 1), G / (Mathf.Pow(x.y, 5) + 1), 0);

                wipi.x += (wi.x * tgt_rb.transform.position.x);
                wipi.y += (wi.y * tgt_rb.transform.position.y);

                wi_sum.x += wi.x;
                wi_sum.y += wi.y;
            }

            Vector3 c_pos = new Vector3(wipi.x/wi_sum.x, wipi.y / wi_sum.y, 0);

            //cursor.transform.position = c_pos;

            cursor.transform.position = Vector3.MoveTowards(cursor.transform.position, c_pos, 0.1f);

        }
    }

    private void HandleClick()
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

            var collider = Physics2D.OverlapCircle(cursor.transform.position, 0.25f, layer_mask);
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
            RaycastHit2D hit = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), CURSOR_RADIUS, Vector2.zero, .1f, layer_mask);
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

    public void SpawnCursor()
    {
        // Instantiate the cursor for the player
        cursor = Instantiate(cursor_prefab, transform.position, transform.rotation, transform);
        cursor_rb = cursor.AddComponent<Rigidbody2D>(); 
        cursor.name = "Cursor";
        cursor_spawned = true;
    }

    private void LockCursorToGameWindow()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}
