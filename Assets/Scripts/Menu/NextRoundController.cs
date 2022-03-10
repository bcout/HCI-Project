using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextRoundController : MonoBehaviour
{
    [SerializeField]
    private Button start_button;
    // Start is called before the first frame update
    void Start()
    {
        start_button.onClick.AddListener(NextRound);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void NextRound()
    {
        GameData.game_state = GameData.state.LOADING;
        GameData.current_round++;
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
