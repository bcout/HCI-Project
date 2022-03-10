using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Button start_button;
    // Start is called before the first frame update
    void Start()
    {
        start_button.onClick.AddListener(StartGame);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartGame()
    {
        GameData.game_state = GameData.state.LOADING;
        GameData.current_round++;
        SceneManager.LoadScene("Main");
    }
}
