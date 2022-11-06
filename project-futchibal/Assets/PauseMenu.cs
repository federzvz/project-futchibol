using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu, gameplayCanvas;
    public GameObject btnResumeGame;
    public EventSystem eventSystem;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        gameplayCanvas.SetActive(false);
        pauseMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(btnResumeGame);
    }

    public void ResumeGame() {
        pauseMenu.SetActive(false);
        gameplayCanvas.SetActive(true);
        Time.timeScale = 1f;
    }

    public void BackToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
