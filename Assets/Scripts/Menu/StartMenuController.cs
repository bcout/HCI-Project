using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private Button start_button, instruction_label;
    [SerializeField] private Button[] latin_square_choices;
    [SerializeField] private TMP_InputField username_field;

    private Button selected_button;

    private const string DIGITS = "0123456789";
    private const int USER_ID_LENGTH = 10;

    // Start is called before the first frame update
    void Start()
    {
        start_button.onClick.AddListener(StartGame);
        start_button.onClick.AddListener(SetUsername);
        start_button.gameObject.SetActive(false);

        instruction_label.enabled = false;

        foreach (Button btn in latin_square_choices)
        { 
            btn.onClick.AddListener(SetLatinSquareRow);
        }

        selected_button = null;

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
    }

    private void SetLatinSquareRow()
    {
        if (selected_button != null)
        {
            selected_button.gameObject.GetComponent<Image>().color = Color.white;
        }
        selected_button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        selected_button.gameObject.GetComponent<Image>().color = Color.yellow;

        GameData.latin_square_row = selected_button.GetComponent<LatinSquareChoice>().row;

        start_button.gameObject.SetActive(true);
    }
}
