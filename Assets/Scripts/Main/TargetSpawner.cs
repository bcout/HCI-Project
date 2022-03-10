using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
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
        // Spawn all the targets before the player's cursor spawns (as a way to let the player get ready)
        if (GameData.game_state == GameData.state.LOADING && num_targets_spawned < MAX_TARGETS)
        {
            if (coroutine_available)
            {
                StartCoroutine("SpawnTargetLoop");
                coroutine_available = false;
            }
        }
        else
        {
            if (GameData.game_state == GameData.state.LOADING)
            {
                GameData.game_state = GameData.state.RUNNING;
            }
        }
        //

        // Despawn all the targets when the level finishes, to give the player a breather before going to the menu
        if (GameData.game_state == GameData.state.UNLOADING)
        {
            if (transform.childCount > 0)
            {
                foreach (Transform target in transform)
                {
                    if (coroutine_available)
                    {
                        StartCoroutine("DespawnTargetLoop", target);
                        coroutine_available = false;
                    }
                }
            }
            else
            {
                GameData.game_state = GameData.state.DONE;
            }
        }
    }

    private IEnumerator SpawnTargetLoop()
    {
        SpawnTarget();
        
        yield return new WaitForSeconds(0.1f);

        coroutine_available = true;
        num_targets_spawned++;
    }

    public void SpawnTarget()
    {
        float spawn_x = Random.Range(GameData.left_border_val, GameData.right_border_val);
        float spawn_y = Random.Range(GameData.bottom_border_val, GameData.top_border_val);
        Vector2 spawn_point = new Vector2(spawn_x, spawn_y);
        Instantiate(target_prefab, spawn_point, Quaternion.identity, transform);
    }

    private IEnumerator DespawnTargetLoop(Transform target)
    {
        target.parent = null;
        Destroy(target.gameObject);

        yield return new WaitForSeconds(0.1f);

        coroutine_available = true;
    }
}
