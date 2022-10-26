using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController instance;

    public GameObject multijugadorLocal, multijugadorOnline, estadisticas, opciones, salir;
    public GameObject partidoRapido, volverMainMenu, campeonato;

    public GameObject partidoRapidoPanel;
    public GameObject btnElegirCancha1, btnElegirCancha2;

    public EventSystem eventSystem;
    private GameObject eventSystemlastSelectedGameObject;

    //LocalMultiplayer controls
    //Player 1
    public static KeyCode player1Left = KeyCode.A;
    public static KeyCode player1Right = KeyCode.D;
    public static KeyCode player1Up = KeyCode.W;
    public static KeyCode player1Down = KeyCode.S;
    public static KeyCode player1Kick = KeyCode.Space;
    //Player 2
    public static KeyCode player2Left = KeyCode.LeftArrow;
    public static KeyCode player2Right = KeyCode.RightArrow;
    public static KeyCode player2Up = KeyCode.UpArrow;
    public static KeyCode player2Down = KeyCode.DownArrow;
    public static KeyCode player2Kick = KeyCode.P;
    private bool isSystemListeningNewPlayerKeyInput = false;

    private string stadiumSelected = "";

    public void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }

    public void Update()
    {

        //Si no hay ningún botón seleccionado(Porque clickeó el backround), se guarda y se setea el último botón que estaba seleccionado.
        if (eventSystem.currentSelectedGameObject != null)
        {
            eventSystemlastSelectedGameObject = eventSystem.currentSelectedGameObject;
        }
        else {
            eventSystem.SetSelectedGameObject(eventSystemlastSelectedGameObject);
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
            int rInt = Random.Range(0, 1);
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
    }

    public void listenPlayer1LeftKeyInput() {
        if (isSystemListeningNewPlayerKeyInput && Event.current.isKey && Event.current.type == EventType.KeyDown)
        {
            player1Left = Event.current.keyCode;
        }
        
    }


}
