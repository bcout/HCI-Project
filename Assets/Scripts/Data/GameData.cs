
using UnityEngine;
public static class GameData
{
    public static bool game_started { get; set; }
    public static float left_border_val { get; set; }
    public static float right_border_val { get; set; }
    public static float bottom_border_val { get; set; }
    public static float top_border_val { get; set; }

    static GameData()
    {
        game_started = false;
        float distance = (Vector3.zero - Camera.main.transform.position).z;
        left_border_val = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
        right_border_val = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x;
        bottom_border_val = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y;
        top_border_val = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y;
    }
}
