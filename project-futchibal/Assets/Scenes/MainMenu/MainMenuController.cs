using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using System;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController instance;

    public GameObject multijugadorLocal, multijugadorOnline, estadisticas, opciones, salir, btnConfiguracionesVolverMainMenu;
    public GameObject partidoRapido, volverMainMenu, campeonato;

    public GameObject partidoRapidoPanel, configuracionesPanel;
    public GameObject btnElegirCancha1, btnElegirCancha2;

    public EventSystem eventSystem;
    private GameObject eventSystemlastSelectedGameObject;

    private bool isSystemListeningNewPlayerKeyInput = false;
    private string buttonPressed = "";

    private string stadiumSelected = "";

    public CustomPlayerPrefs customPlayerPrefs = new CustomPlayerPrefs();

    public Text btnTextPlayer1Right, btnTextPlayer1Left, btnTextPlayer1Down, btnTextPlayer1Up, btnTextPlayer1Kick;
    public Text btnTextPlayer2Right, btnTextPlayer2Left, btnTextPlayer2Down, btnTextPlayer2Up, btnTextPlayer2Kick;

    public void Awake()
    {
        if (instance == null) {
            instance = this;
        }

        //If playerprefs.txt file does not exist, create one with default values
        if (!File.Exists(Application.dataPath + "/playerprefs.json"))
        {
            string strOutput = JsonUtility.ToJson(customPlayerPrefs);
            File.WriteAllText(Application.dataPath + "/playerprefs.json", strOutput);
        }
        else {
            string json = File.ReadAllText(Application.dataPath + "/playerprefs.json");
            customPlayerPrefs = JsonUtility.FromJson<CustomPlayerPrefs>(json);
        }
        getDefaultKeysAndChangeButtonsText();
    }

    public void Update()
    {
        Event e = Event.current;
        //Si no hay ningún botón seleccionado(Porque clickeó el backround), se guarda y se setea el último botón que estaba seleccionado.
        if (eventSystem.currentSelectedGameObject != null)
        {
            eventSystemlastSelectedGameObject = eventSystem.currentSelectedGameObject;
        }
        else {
            eventSystem.SetSelectedGameObject(eventSystemlastSelectedGameObject);
        }

        //if player has been pressed some of the buttons to change the settings
        if (isSystemListeningNewPlayerKeyInput) {
            //change the button listening to the player input
            updatePlayerControls();
        }
    }

    public void OnMultijugadorLocalClick() {
        partidoRapido.SetActive(true);
        campeonato.SetActive(true);
    }

    public void OnPartidoRapidoClick() {
        partidoRapido.SetActive(false);
        campeonato.SetActive(false);
        multijugadorLocal.SetActive(false);
        estadisticas.SetActive(false);
        multijugadorOnline.SetActive(false);
        opciones.SetActive(false);
        salir.SetActive(false);

        partidoRapidoPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(btnElegirCancha1);
    }

    public void OnBtnCancha1Selected() {
        stadiumSelected = "FedeScene";
        btnElegirCancha2.GetComponentInChildren<Text>().color = Color.white;
        btnElegirCancha1.GetComponentInChildren<Text>().color = new Color(47f / 255f, 247f / 255f, 170f / 255f);
    }
    public void OnBtnCancha2Selected()
    {
        stadiumSelected = "MiltonScene";
        btnElegirCancha1.GetComponentInChildren<Text>().color = Color.white;
        btnElegirCancha2.GetComponentInChildren<Text>().color = new Color(47f / 255f, 247f / 255f, 170f / 255f);
    }

    public void OnBtnPlayMatch() {
        if (stadiumSelected.Equals("")) {
            int rInt = UnityEngine.Random.Range(0, 1);
            if (rInt == 0)
            {
                stadiumSelected = "FedeScene";
            }
            else {
                stadiumSelected = "MiltonScene";
            }
        }
        SceneManager.LoadScene(stadiumSelected);
    }

    public void OnQuitClick() {
        Application.Quit();
    }

    public void OnBtnVolverMainMenu() {
        partidoRapidoPanel.SetActive(false);

        multijugadorLocal.SetActive(true);
        estadisticas.SetActive(true);
        multijugadorOnline.SetActive(true);
        opciones.SetActive(true);
        salir.SetActive(true);
        eventSystem.SetSelectedGameObject(multijugadorLocal);
        configuracionesPanel.SetActive(false);
    }

    public void OnBtnSettings() {
        configuracionesPanel.SetActive(true);

        partidoRapidoPanel.SetActive(false);
        multijugadorLocal.SetActive(false);
        estadisticas.SetActive(false);
        multijugadorOnline.SetActive(false);
        opciones.SetActive(false);
        salir.SetActive(false);
        eventSystem.SetSelectedGameObject(btnConfiguracionesVolverMainMenu);
    }

    public void listenPlayer1LeftKeyInput() {
        isSystemListeningNewPlayerKeyInput = true;
        buttonPressed = "player1Left";
        btnConfiguracionesVolverMainMenu.SetActive(false);
    }

    public void listenPlayer1RightKeyInput()
    {
        isSystemListeningNewPlayerKeyInput = true;
        buttonPressed = "player1Right";
        btnConfiguracionesVolverMainMenu.SetActive(false);
    }

    public void listenPlayer1DownKeyInput()
    {
        isSystemListeningNewPlayerKeyInput = true;
        buttonPressed = "player1Down";
        btnConfiguracionesVolverMainMenu.SetActive(false);
    }

    public void listenPlayer1UpKeyInput()
    {
        isSystemListeningNewPlayerKeyInput = true;
        buttonPressed = "player1Up";
        btnConfiguracionesVolverMainMenu.SetActive(false);
    }

    public void listenPlayer1KickKeyInput()
    {
        isSystemListeningNewPlayerKeyInput = true;
        buttonPressed = "player1Kick";
        btnConfiguracionesVolverMainMenu.SetActive(false);
    }

    public void listenPlayer2LeftKeyInput()
    {
        isSystemListeningNewPlayerKeyInput = true;
        buttonPressed = "player2Left";
        btnConfiguracionesVolverMainMenu.SetActive(false);
    }

    public void listenPlayer2RightKeyInput()
    {
        isSystemListeningNewPlayerKeyInput = true;
        buttonPressed = "player2Right";
        btnConfiguracionesVolverMainMenu.SetActive(false);
    }

    public void listenPlayer2DownKeyInput()
    {
        isSystemListeningNewPlayerKeyInput = true;
        buttonPressed = "player2Down";
        btnConfiguracionesVolverMainMenu.SetActive(false);
    }

    public void listenPlayer2UpKeyInput()
    {
        isSystemListeningNewPlayerKeyInput = true;
        buttonPressed = "player2Up";
        btnConfiguracionesVolverMainMenu.SetActive(false);
    }

    public void listenPlayer2KickKeyInput()
    {
        isSystemListeningNewPlayerKeyInput = true;
        buttonPressed = "player2Kick";
        btnConfiguracionesVolverMainMenu.SetActive(false);
    }

    public void updatePlayerControls() {
        string strOutput = "";
        //Iterate every keycode existent
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            //if key pressed is one of the existent keys
            if (Input.GetKeyDown(kcode) && kcode != KeyCode.Return)
            {
                //detect which button the user pressed to change that specific button action
                if (kcode == KeyCode.Backspace || kcode == KeyCode.Escape) {
                    isSystemListeningNewPlayerKeyInput = false;
                    btnConfiguracionesVolverMainMenu.SetActive(true);
                    return;
                }
                switch (buttonPressed) {
                    case "player1Left":
                        Debug.Log(buttonPressed);
                        Debug.Log("KeyCode down: " + kcode);
                        isSystemListeningNewPlayerKeyInput = false;
                        customPlayerPrefs.player1Left = kcode;
                        strOutput = JsonUtility.ToJson(customPlayerPrefs);
                        File.WriteAllText(Application.dataPath + "/playerprefs.json", strOutput);
                        btnTextPlayer1Left.text = kcode.ToString();
                        btnConfiguracionesVolverMainMenu.SetActive(true);

                        break;
                    case "player1Right":
                        Debug.Log(buttonPressed);
                        Debug.Log("KeyCode down: " + kcode);
                        isSystemListeningNewPlayerKeyInput = false;
                        customPlayerPrefs.player1Right = kcode;
                        strOutput = JsonUtility.ToJson(customPlayerPrefs);
                        File.WriteAllText(Application.dataPath + "/playerprefs.json", strOutput);
                        btnTextPlayer1Right.text = kcode.ToString();
                        btnConfiguracionesVolverMainMenu.SetActive(true);
                        break;
                    case "player1Down":
                        Debug.Log(buttonPressed);
                        Debug.Log("KeyCode down: " + kcode);
                        isSystemListeningNewPlayerKeyInput = false;
                        customPlayerPrefs.player1Down = kcode;
                        strOutput = JsonUtility.ToJson(customPlayerPrefs);
                        File.WriteAllText(Application.dataPath + "/playerprefs.json", strOutput);
                        btnTextPlayer1Down.text = kcode.ToString();
                        btnConfiguracionesVolverMainMenu.SetActive(true);
                        break;
                    case "player1Up":
                        Debug.Log(buttonPressed);
                        Debug.Log("KeyCode down: " + kcode);
                        isSystemListeningNewPlayerKeyInput = false;
                        customPlayerPrefs.player1Up = kcode;
                        strOutput = JsonUtility.ToJson(customPlayerPrefs);
                        File.WriteAllText(Application.dataPath + "/playerprefs.json", strOutput);
                        btnTextPlayer1Up.text = kcode.ToString();
                        btnConfiguracionesVolverMainMenu.SetActive(true);
                        break;
                    case "player1Kick":
                        Debug.Log(buttonPressed);
                        Debug.Log("KeyCode down: " + kcode);
                        isSystemListeningNewPlayerKeyInput = false;
                        customPlayerPrefs.player1Kick = kcode;
                        strOutput = JsonUtility.ToJson(customPlayerPrefs);
                        File.WriteAllText(Application.dataPath + "/playerprefs.json", strOutput);
                        btnTextPlayer1Kick.text = kcode.ToString();
                        btnConfiguracionesVolverMainMenu.SetActive(true);
                        break;
                    case "player2Left":
                        Debug.Log(buttonPressed);
                        Debug.Log("KeyCode down: " + kcode);
                        isSystemListeningNewPlayerKeyInput = false;
                        customPlayerPrefs.player2Left = kcode;
                        strOutput = JsonUtility.ToJson(customPlayerPrefs);
                        File.WriteAllText(Application.dataPath + "/playerprefs.json", strOutput);
                        btnTextPlayer2Left.text = kcode.ToString();
                        btnConfiguracionesVolverMainMenu.SetActive(true);
                        break;
                    case "player2Right":
                        Debug.Log(buttonPressed);
                        Debug.Log("KeyCode down: " + kcode);
                        isSystemListeningNewPlayerKeyInput = false;
                        customPlayerPrefs.player2Right = kcode;
                        strOutput = JsonUtility.ToJson(customPlayerPrefs);
                        File.WriteAllText(Application.dataPath + "/playerprefs.json", strOutput);
                        btnTextPlayer2Right.text = kcode.ToString();
                        btnConfiguracionesVolverMainMenu.SetActive(true);
                        break;
                    case "player2Down":
                        Debug.Log(buttonPressed);
                        Debug.Log("KeyCode down: " + kcode);
                        isSystemListeningNewPlayerKeyInput = false;
                        customPlayerPrefs.player2Down = kcode;
                        strOutput = JsonUtility.ToJson(customPlayerPrefs);
                        File.WriteAllText(Application.dataPath + "/playerprefs.json", strOutput);
                        btnTextPlayer2Down.text = kcode.ToString();
                        btnConfiguracionesVolverMainMenu.SetActive(true);
                        break;
                    case "player2Up":
                        Debug.Log(buttonPressed);
                        Debug.Log("KeyCode down: " + kcode);
                        isSystemListeningNewPlayerKeyInput = false;
                        customPlayerPrefs.player2Up = kcode;
                        strOutput = JsonUtility.ToJson(customPlayerPrefs);
                        File.WriteAllText(Application.dataPath + "/playerprefs.json", strOutput);
                        btnTextPlayer2Up.text = kcode.ToString();
                        btnConfiguracionesVolverMainMenu.SetActive(true);
                        break;
                    case "player2Kick":
                        Debug.Log(buttonPressed);
                        Debug.Log("KeyCode down: " + kcode);
                        isSystemListeningNewPlayerKeyInput = false;
                        customPlayerPrefs.player2Kick = kcode;
                        strOutput = JsonUtility.ToJson(customPlayerPrefs);
                        File.WriteAllText(Application.dataPath + "/playerprefs.json", strOutput);
                        btnTextPlayer2Kick.text = kcode.ToString();
                        btnConfiguracionesVolverMainMenu.SetActive(true);
                        break;
                }
                
            }
        }
    }

    public void getDefaultKeysAndChangeButtonsText() {
        btnTextPlayer1Left.text = customPlayerPrefs.player1Left.ToString();
        btnTextPlayer1Right.text = customPlayerPrefs.player1Right.ToString();
        btnTextPlayer1Down.text = customPlayerPrefs.player1Down.ToString();
        btnTextPlayer1Up.text = customPlayerPrefs.player1Up.ToString();
        btnTextPlayer1Kick.text = customPlayerPrefs.player1Kick.ToString();

        btnTextPlayer2Left.text = customPlayerPrefs.player2Left.ToString();
        btnTextPlayer2Right.text = customPlayerPrefs.player2Right.ToString();
        btnTextPlayer2Down.text = customPlayerPrefs.player2Down.ToString();
        btnTextPlayer2Up.text = customPlayerPrefs.player2Up.ToString();
        btnTextPlayer2Kick.text = customPlayerPrefs.player2Kick.ToString();
    }




}
