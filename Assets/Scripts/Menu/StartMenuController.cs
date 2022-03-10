using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenuController : MonoBehaviour
{
    [SerializeField]
    private Button start_button;

    [SerializeField]
    private TMP_InputField username_field;

    private const string DIGITS = "0123456789";
    private const int USER_ID_LENGTH = 10;

    // Start is called before the first frame update
    void Start()
    {
        start_button.onClick.AddListener(StartGame);
        start_button.onClick.AddListener(SetUsername);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void StartGame()
    {
        GameData.game_state = GameData.state.LOADING;
        GameData.current_round++;
        SceneManager.LoadScene("Main");
    }

    public void SetUsername()
    {
        if (username_field.text.Length > 0)
        {
            GameData.username = username_field.text;
        }
        else
        {
            GameData.username = "";
            // Pick a random UID
            for(int i = 0; i < USER_ID_LENGTH; i++)
            {
                GameData.username += DIGITS[Random.Range(0, DIGITS.Length)];
            }
        }

        print(GameData.username);
    }
}
