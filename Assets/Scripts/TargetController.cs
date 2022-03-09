using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField]
    private GameObject target_prefab;
    private const int MAX_TARGETS = 35;
    private int num_targets_spawned;
    private bool coroutine_available;

    // Start is called before the first frame update
    void Start()
    {
        coroutine_available = true;
        num_targets_spawned = 0;
    }

    private void Update()
    {
        if (num_targets_spawned < MAX_TARGETS)
        {
            if (coroutine_available)
            {
                StartCoroutine("SpawnTarget");
                coroutine_available = false;
            }
        }
        else
        {
            if (!GameData.game_started)
            {
                GameData.game_started = true;
            }
        }
    }

    private IEnumerator SpawnTarget()
    {
        float padding = 0.5f;
        float spawn_x = Random.Range(GameData.left_border_val + padding, GameData.right_border_val - padding);
        float spawn_y = Random.Range(GameData.bottom_border_val + padding, GameData.top_border_val - padding);
        Vector2 spawn_point = new Vector2(spawn_x, spawn_y);
        print(spawn_point);
        Instantiate(target_prefab, spawn_point, Quaternion.identity, transform);
        
        yield return new WaitForSeconds(0.1f);

        coroutine_available = true;
        num_targets_spawned++;
    }
}
