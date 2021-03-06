
using UnityEngine;
public static class GameData
{
    private const int NO_ASSIST = 0;
    private const int AREA_ASSIST = 1;
    private const int GRAVITY_ASSIST = 2;

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
    public static int[][] latin_square { get; set; }
    public static int latin_square_row { get; set; }
    
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
        MAX_ROUNDS = 6;
        TIME_LIMIT = 30;

        scores = new int[MAX_ROUNDS];
        misses = new int[MAX_ROUNDS];

        FillLatinSquare();
        latin_square_row = -1;
    }

    private static void FillLatinSquare()
    {
        int[] row_0 = { NO_ASSIST, NO_ASSIST, GRAVITY_ASSIST, AREA_ASSIST, GRAVITY_ASSIST, AREA_ASSIST };
        int[] row_1 = { NO_ASSIST, AREA_ASSIST, NO_ASSIST, AREA_ASSIST, GRAVITY_ASSIST, GRAVITY_ASSIST };
        int[] row_2 = { AREA_ASSIST, AREA_ASSIST, NO_ASSIST, GRAVITY_ASSIST, NO_ASSIST, GRAVITY_ASSIST };
        int[] row_3 = { AREA_ASSIST, GRAVITY_ASSIST, AREA_ASSIST, GRAVITY_ASSIST, NO_ASSIST, NO_ASSIST };
        int[] row_4 = { GRAVITY_ASSIST, GRAVITY_ASSIST, AREA_ASSIST, NO_ASSIST, AREA_ASSIST, NO_ASSIST };
        int[] row_5 = { GRAVITY_ASSIST, NO_ASSIST, GRAVITY_ASSIST, NO_ASSIST, AREA_ASSIST, AREA_ASSIST };

        latin_square = new int[][] { row_0, row_1, row_2, row_3, row_4, row_5 };
    }
}
