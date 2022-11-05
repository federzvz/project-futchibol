using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public float potencia = 300f;
    public int playerTeamId;
    public KeyCode izquierda;
    public KeyCode derecha;
    public KeyCode arriba;
    public KeyCode abajo ;
    private float fuerzaHorizontal = 0f;
    private float fuerzaVertical = 0f;
    private float velocidadReaccion = 0.2f;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();
        //m_Rigidbody.drag = 3;

        //setPlayerControlPrefs
        setUpPlayerControlPrefs();
    }
    void FixedUpdate()
    {
        calcVelocidadHorizontal();
        calcVelocidadVertical();

        Debug.Log(fuerzaHorizontal);

        //float xDirection = Input.GetAxis("Horizontal");
        //float zDirection = Input.GetAxis("Vertical");
        //Debug.Log(xDirection);
        //Debug.Log(zDirection);
        //m_Rigidbody.AddForce(transform.right * xDirection * potencia);
        m_Rigidbody.AddForce(transform.right * fuerzaHorizontal * potencia);
        m_Rigidbody.AddForce(transform.forward * fuerzaVertical * potencia);

        //if (Input.GetButton("Jump"))
        //{
        //    //Apply a force to this Rigidbody in direction of this GameObjects up axis
        //    m_Rigidbody.AddForce(transform.up * m_Thrust);
        //}

    }
    void calcVelocidadHorizontal()
    {
        if (Input.GetKey(izquierda))
        {
            fuerzaHorizontal -= velocidadReaccion;
        }
        else if (Input.GetKey(derecha))
        {
            fuerzaHorizontal += velocidadReaccion;
        }
        else
        {
            if ((-velocidadReaccion < fuerzaHorizontal) && (fuerzaHorizontal < velocidadReaccion))
            {
                fuerzaHorizontal = 0;
            }
            else
            {
                if (fuerzaHorizontal > 0)
                {
                    fuerzaHorizontal -= velocidadReaccion;
                }
                else if (fuerzaHorizontal < 0)
                {
                    fuerzaHorizontal += velocidadReaccion;
                }
            }
        }
        if (fuerzaHorizontal > 1)
        {
            fuerzaHorizontal = 1f;
        }
        else if (fuerzaHorizontal < -1)
        {
            fuerzaHorizontal = -1f;
        }
    }
    void calcVelocidadVertical()
    {
        if (Input.GetKey(abajo))
        {
            fuerzaVertical -= velocidadReaccion;
        }
        else if (Input.GetKey(arriba))
        {
            fuerzaVertical += velocidadReaccion;
        }
        else
        {
            if ((-velocidadReaccion < fuerzaVertical) && (fuerzaVertical < velocidadReaccion))
            {
                fuerzaVertical = 0;
            }
            else
            {
                if (fuerzaVertical > 0)
                {
                    fuerzaVertical -= velocidadReaccion;
                }
                else if (fuerzaVertical < 0)
                {
                    fuerzaVertical += velocidadReaccion;
                }
            }
        }
        if (fuerzaVertical > 1)
        {
            fuerzaVertical = 1f;
        }
        else if (fuerzaVertical < -1)
        {
            fuerzaVertical = -1f;
        }
    }

    public void setUpPlayerControlPrefs() {
        string json = File.ReadAllText(Application.dataPath + "/playerprefs.json");
        CustomPlayerPrefs customPlayerPrefs = JsonUtility.FromJson<CustomPlayerPrefs>(json);
        if (playerTeamId == 1) {
            Debug.Log(Application.dataPath + "/playerprefs.json");
            this.izquierda = customPlayerPrefs.player1Left;
            this.derecha = customPlayerPrefs.player1Right;
            this.arriba = customPlayerPrefs.player1Up;
            this.abajo = customPlayerPrefs.player1Down;
        }
        if (playerTeamId == 2) {
            this.izquierda = customPlayerPrefs.player2Left;
            this.derecha = customPlayerPrefs.player2Right;
            this.arriba = customPlayerPrefs.player2Up;
            this.abajo = customPlayerPrefs.player2Down;
        }
    }
}
