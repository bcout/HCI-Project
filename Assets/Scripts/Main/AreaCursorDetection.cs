using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCursorDetection : MonoBehaviour
{
    private PlayerController player_controller;

    private void Start()
    {
        player_controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            player_controller.HandleCollision(other.gameObject);
        }
    }
}
