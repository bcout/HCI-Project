
using UnityEngine;
public static class GameData
{
    public enum state
    {
        LOADING,
        RUNNING,
        UNLOADING,
        DONE
    }
    public static state game_state { get; set; }
    public static float left_border_val { get; set; }
    public static float right_border_val { get; set; }
    public static float bottom_border_val { get; set; }
    public static float top_border_val { get; set; }
    public static int player_score { get; set; }
    public static int player_misses { get; set; }
    public static int current_round { get; set; }
    public static int MAX_ROUNDS { get; }
    public static float TIME_LIMIT { get; }
    public static int[] scores { get; set; }
    public static int[] misses { get; set; }
    public static string username { get; set; }
    public static float min_cursor_radius { get; }
    public static float max_cursor_radius { get; }
    
    static GameData()
    {
        game_state = state.DONE;
        float padding = 0.5f;
        float distance = (Vector3.zero - Camera.main.transform.position).z;
        left_border_val = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x + padding;
        right_border_val = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x - padding;
        bottom_border_val = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y + padding;
        top_border_val = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y - (2 * padding);

        player_score = 0;
        player_misses = 0;
        current_round = 0;
        MAX_ROUNDS = 7;
        TIME_LIMIT = 30;

        scores = new int[MAX_ROUNDS];
        misses = new int[MAX_ROUNDS];

        min_cursor_radius = 0.01f;
        max_cursor_radius = 0.5f;
    }
}
