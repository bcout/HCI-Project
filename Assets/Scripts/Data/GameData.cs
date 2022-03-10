
using UnityEngine;
public static class GameData
{
    public static bool game_started { get; set; }
    public static float left_border_val { get; set; }
    public static float right_border_val { get; set; }
    public static float bottom_border_val { get; set; }
    public static float top_border_val { get; set; }
    public static int player_score { get; set; }

    static GameData()
    {
        game_started = false;
        float padding = 0.5f;
        float distance = (Vector3.zero - Camera.main.transform.position).z;
        left_border_val = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x + padding;
        right_border_val = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x - padding;
        bottom_border_val = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y + padding;
        top_border_val = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y - (2 * padding);

        player_score = 0;
    }
}
