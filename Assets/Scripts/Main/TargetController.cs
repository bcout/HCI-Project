using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private Vector2[] possible_vectors = { Vector2.right, Vector2.left, Vector2.down, Vector2.up };
    private Vector2 velocity;
    private float speed = 0.9f;
    private float width;
    private float height;

    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0, possible_vectors.Length);
        velocity = possible_vectors[index];

        SpriteRenderer spr_renderer = GetComponent<SpriteRenderer>();
        width = spr_renderer.bounds.size.x;
        height = spr_renderer.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.game_state == GameData.state.RUNNING)
        {
            //RandomlyChangeDirection();
        }
        
    }

    private void RandomlyChangeDirection()
    {
        int change = Random.Range(0, 200);
        if (change == 0)
        {
            velocity = new Vector2(-velocity.x, -velocity.y);
        }
        if (change == 1)
        {
            if (velocity.x != 0f)
            {
                velocity = new Vector2(0f, velocity.x);
            }
            else
            {
                // We can assume that if the ghost isn't moving horizontally, then they are moving vertically
                // and we can thus use velocity.y as a valid movement value for 'x'.
                velocity = new Vector2(velocity.y, 0f);
            }
        }

        if ((transform.position.x <= GameData.left_border_val + width / 2.0) && velocity.x < 0f)
        {
            velocity = Vector3.right;
        }
        if ((transform.position.x >= GameData.right_border_val - width / 2.0) && velocity.x > 0f)
        {
            velocity = Vector3.left;
        }
        if ((transform.position.y <= GameData.bottom_border_val + height / 2.0) && velocity.y < 0f)
        {
            velocity = Vector3.up;
        }
        if ((transform.position.y >= GameData.top_border_val - height / 2.0) && velocity.y > 0f)
        {
            velocity = Vector3.down;
        }
        transform.position = (Vector2)transform.position + velocity * Time.deltaTime * speed;
    }
}
