using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
        WriteUserData();

        Application.Quit();
    }

    private void WriteUserData()
    {
        // Output file path
        string path = Application.dataPath + "/results.txt";

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "");
        }

        string results = "Experiment Results\n\n";
        results += GameData.username + "\n" + "Scores\n";
        for (int i = 0; i < GameData.MAX_ROUNDS; i++)
        {
            results += "Round " + i + ": " + GameData.scores[i];
        }

        File.WriteAllText(path, results);
    }
}
