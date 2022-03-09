using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject cursor_object;

    private GameObject cursor;
    private Vector3 mouse_position;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        // Instantiate the cursor for the player
        cursor = Instantiate(cursor_object, transform.position, transform.rotation, transform);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCursorPosition();
        
    }

    private void UpdateCursorPosition()
    {
        mouse_position = Input.mousePosition;
        mouse_position = Camera.main.ScreenToWorldPoint(mouse_position);
        mouse_position.z = 0;
        cursor.transform.position = new Vector3(mouse_position.x, mouse_position.y, 0);
    }
}
