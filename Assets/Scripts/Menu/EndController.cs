using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndController : MonoBehaviour
{
    [SerializeField]
    private Button quit_button;

    // Start is called before the first frame update
    void Start()
    {
        quit_button.onClick.AddListener(QuitGame);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void QuitGame()
    {
        // Write player's data to an output file
        print(GameData.username);
        for (int i = 0; i < GameData.MAX_ROUNDS; i++)
        {
            print("Round " + i + ": " + GameData.scores[i]);
        }
        Application.Quit();
    }
}
