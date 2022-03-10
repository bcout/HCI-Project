using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI round_text, timer_text, score_text;
    // Start is called before the first frame update

    public void UpdateHUD(float time_left)
    {
        UpdateRound();
        UpdateTimer(time_left);
        UpdateScore();
    }
    private void UpdateRound()
    {
        round_text.text = "Round: " + GameData.current_round + " / " + GameData.MAX_ROUNDS;
    }

    private void UpdateTimer(float time_left)
    {
        timer_text.text = time_left.ToString("00.00");
    }

    private void UpdateScore()
    {
        score_text.text = "Score: " + GameData.player_score;
    }
}
