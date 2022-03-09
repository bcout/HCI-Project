using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject cursor_object;

    private GameObject cursor;
    private Vector3 mouse_position;

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

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the cursor for the player
        cursor = Instantiate(cursor_object, transform.position, transform.rotation, transform);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        UpdateCursorPosition();
        
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
        print("Clicked!");
    }
}
