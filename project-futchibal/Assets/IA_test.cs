using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coords
{
    public Coords(double x, double z)
    {
        X = x;
        Z = z;
    }

    public double X { get; set; }
    public double Z { get; set; }

    public override string ToString() => $"({X}, {Z})";
}
public enum Tipos { Goalkeeper, Defender, Midfielder, Forward };

public class IA_test : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public GameObject me;
    public GameObject myGoal;
    public GameObject ball;
    public float potencia = 300f;
    public float GKdistanciaParaSalir = 15f;
    private Coords currentPosition = new Coords(0,0);
    private Coords goalPosition = new Coords(0, 0);
    private Coords ObjetivoPosition = new Coords(0, 0);
    private Coords ballPosition = new Coords(0, 0);
    private double distanciaX = 0f, distanciaZ = 0f, distanciaPelotaArco = 0f, distanciaIAPelota = 0f;
    public Tipos Tipo;
    private Vector3 vectorIAPelota = new Vector3();
    private bool salir = false;
    private float fuerzaHorizontal = 0f;
    private float fuerzaVertical = 0f;
    private float delayToKick = 0f;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        getCurrentPosition();
        goalPosition.X = myGoal.transform.position.x;
        goalPosition.Z = myGoal.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (delayToKick > 0)
            delayToKick -= 0.02f;
        actualizarDistanciaPelotaArco();
        if (Tipo == Tipos.Goalkeeper)
        {
            Debug.Log("GOLEROOOOOOOOOO");
            Debug.Log("YO: " + currentPosition.ToString());
            Debug.Log("OBJ: " + ObjetivoPosition.ToString());
            Goalkeepear();
        }
        //getCurrentPosition();
        //Debug.Log("Posicion: (" + currentPosition.X + "," + currentPosition.Z + ")");
        //Debug.Log("Posicion GOAL: (" + goalPosition.X + "," + goalPosition.Z + ")");
    }

    void getCurrentPosition()
    {
        currentPosition.X = me.transform.position.x;
        currentPosition.Z = me.transform.position.z;
    }
    void mover()
    {
        if (currentPosition.X < ObjetivoPosition.X) //Moverme a la derecha
        {
            if (currentPosition.Z < ObjetivoPosition.Z) //Moverme a arriba
            {
                fuerzaHorizontal = 0.5f;
                fuerzaVertical = 0.5f;
            } else if (currentPosition.Z > ObjetivoPosition.Z) //Moverme a abajo
            {
                fuerzaHorizontal = 0.5f;
                fuerzaVertical = -0.5f;
            }
            else //No moverme en vertical
            {
                fuerzaHorizontal = 0.5f;
                fuerzaVertical = 0f;
            }
        }
        if (currentPosition.X > ObjetivoPosition.X) //Moverme a la izquierda
        {
            if (currentPosition.Z < ObjetivoPosition.Z) //Moverme a arriba
            {
                fuerzaHorizontal = -0.5f;
                fuerzaVertical = 0.5f;
            }
            else if (currentPosition.Z > ObjetivoPosition.Z) //Moverme a abajo
            {
                fuerzaHorizontal = -0.5f;
                fuerzaVertical = -0.5f;
            }
            else //No moverme en vertical
            {
                fuerzaHorizontal = -0.5f;
                fuerzaVertical = 0f;
            }
        }
        m_Rigidbody.AddForce(transform.right * fuerzaHorizontal * potencia);
        m_Rigidbody.AddForce(transform.forward * fuerzaVertical * potencia);
    
    }
    void Goalkeepear() //Goalkeepear consiste en pararse a la mitad de la trayectoria entre la pelota y el arco (v1)
    {
        actualizarDistanciaPelotaArco();
        if (distanciaPelotaArco < GKdistanciaParaSalir)
        {
            achicar();
        } else
        {
            salir = false;
        }
        getCurrentPosition(); //Actualizo mi posicion actual
        calcPosicionObjetivo(); //Actualizo la posicion a la que quiero ir
        mover(); //Voy a la posicion
    }

    void calcPosicionObjetivo()
    {
        ballPosition.X = ball.transform.position.x;
        ballPosition.Z = ball.transform.position.z;
        if(!salir) // Si es false el objetivo es mantener la posicion de golero
        {
            ObjetivoPosition.X = ((ballPosition.X + goalPosition.X) / 2);
            ObjetivoPosition.Z = ((ballPosition.Z + goalPosition.Z) / 2);
        } else // Si es true hay que achicar por lo que el objetivo es la pelota
        {
            ObjetivoPosition.X = ballPosition.X;
            ObjetivoPosition.Z = ballPosition.Z;
        }
        //ObjetivoPosition.X = ((ballPosition.X + goalPosition.X) / 2);
        //ObjetivoPosition.Z = ((ballPosition.Z + goalPosition.Z) / 2);
    }
    void patear()
    {
        vectorIAPelota = ball.transform.position - (new Vector3(m_Rigidbody.position.x, m_Rigidbody.position.y - 1, m_Rigidbody.position.z));
        //Debug.Log(vectorIAPelota.magnitude);
        vectorIAPelota = vectorIAPelota.normalized;
        //Debug.Log(vectorPlayerPelota.magnitude);
        vectorIAPelota = vectorIAPelota * 3f;
        //Debug.Log(vectorIAPelota.magnitude);
        ball.GetComponent<Rigidbody>().AddForce(vectorIAPelota * 1200);
        delayToKick = 3;
    }

    void actualizarDistanciaPelotaArco()
    {
        distanciaX = Math.Abs(ballPosition.X - goalPosition.X);
        distanciaZ = Math.Abs(ballPosition.Z - goalPosition.Z);
        //Debug.Log("Distania: " + distanciaX);

        distanciaPelotaArco = (float)Math.Sqrt((distanciaX * distanciaX) + (distanciaZ * distanciaZ));
    }
    void actualizarDistanciaIAPelota()
    {
        distanciaX = Math.Abs(ballPosition.X - currentPosition.X);
        distanciaZ = Math.Abs(ballPosition.Z - currentPosition.Z);
        //Debug.Log("Distania: " + distanciaX);

        distanciaIAPelota = (float)Math.Sqrt((distanciaX * distanciaX) + (distanciaZ * distanciaZ));
    }

    void achicar() //Cambia el objetivo a moverse hacia la pelota y pregunta la distancia (si es menor a cierto numero patea)
    { // Para cambiar el objetivo modifica una var (achicar) a true
        salir = true;
        actualizarDistanciaIAPelota();
        if (distanciaIAPelota < 2.5)
        {
            if (ball.transform.position.y < 4.5) {
                if(delayToKick <= 0)
                    patear();
            }
        }
    }
}
