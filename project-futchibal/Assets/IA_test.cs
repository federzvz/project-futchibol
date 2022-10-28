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
    private double zToSumm = 0f, xToSumm = 0f;
    Rigidbody m_Rigidbody;
    public GameObject me;
    public GameObject myGoal, opponentGoal;
    public GameObject ball;
    public float potencia = 300f;
    public float GKdistanciaParaSalir = 15f; // Distancia a la cual achica el golero
    public float distanciaParaSerDelantero = 40f; // Distancia de la pelota al arco propio a partir de la cual se convierte en delantero
    public double distanciaTest = 0f;
    private Coords currentPosition = new Coords(0,0);
    private Coords goalPosition = new Coords(0, 0);
    private Coords ObjetivoPosition = new Coords(0, 0);
    private Coords ballPosition = new Coords(0, 0);
    private double distanciaXIAPelota = 0f, distanciaZIAPelota = 0f, distanciaPelotaArco = 0f, distanciaIAPelota = 0f, distanciaXPelotaArco = 0f, distanciaZPelotaArco = 0f, angulo = 0f;
    private double distanciaXPelotaArcoOp = 0f, distanciaZPelotaArcoOp = 0f;
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
        m_Rigidbody.centerOfMass = new Vector3(0f, -0.80f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        me.transform.eulerAngles = new Vector3(me.transform.eulerAngles.x, 0, me.transform.eulerAngles.z);
        if (delayToKick > 0)
            delayToKick -= 0.02f;
        actualizarDistanciaPelotaArco();
        actualizarDistanciaPelotaArcoOpponent();
        if (Tipo == Tipos.Goalkeeper)
        {
            Debug.Log("GOLEROOOOOOOOOO");
            //Debug.Log("YO: " + currentPosition.ToString());
            //Debug.Log("OBJ: " + ObjetivoPosition.ToString());
            Goalkeepear();
        } else if (Tipo == Tipos.Forward)
        {
            Debug.Log("DELANTEROOOOOO");
            Forwardear();
        }
        Debug.Log("DISTANCIA IA - PELOTA: " + distanciaIAPelota);
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
        if (distanciaPelotaArco > distanciaParaSerDelantero)
        {
            Debug.Log("ME TRANSFORMO EN DELANTEROOO");
            Tipo = Tipos.Forward;
        } else
        {
            getCurrentPosition(); //Actualizo mi posicion actual
            calcPosicionObjetivo(Tipos.Goalkeeper); //Actualizo la posicion a la que quiero ir
            mover(); //Voy a la posicion
        }
    }
    void Forwardear() //Forwardear consiste en pararse atras de la pelota y el arco rival y patear (v1)
    {
        actualizarDistanciaPelotaArco();
        Debug.Log("DISTANCIA PELOTA - ARCO: " + distanciaPelotaArco);
        Debug.Log("DISTANCIA MINIMA PARA SER DELANTERO: " + distanciaParaSerDelantero);
        if (distanciaPelotaArco < distanciaParaSerDelantero)
        {
            Debug.Log("ME TRANSFORMO EN GOLERO");
            Tipo = Tipos.Goalkeeper;
        }
        else
        {
            Tipo = Tipos.Forward;
        }
        getCurrentPosition(); //Actualizo mi posicion actual
        calcPosicionObjetivo(Tipos.Forward); //Actualizo la posicion a la que quiero ir
        mover(); //Voy a la posicion
        definir();
    }
    void calcPosicionObjetivo(Tipos tipo)
    {
        if(tipo == Tipos.Goalkeeper)
        {
            ballPosition.X = ball.transform.position.x;
            ballPosition.Z = ball.transform.position.z;
            if (!salir) // Si es false el objetivo es mantener la posicion de golero
            {
                ObjetivoPosition.X = ((ballPosition.X + goalPosition.X) / 2);
                ObjetivoPosition.Z = ((ballPosition.Z + goalPosition.Z) / 2);
            }
            else // Si es true hay que achicar por lo que el objetivo es la pelota
            {
                ObjetivoPosition.X = ballPosition.X;
                ObjetivoPosition.Z = ballPosition.Z;
            }
        } else if (tipo == Tipos.Forward)
        {   // Objetivo es estar atras de la pelota apuntando al arco.
            ballPosition.X = ball.transform.position.x; //Actualizamos la x de la pelota
            ballPosition.Z = ball.transform.position.z; //Actualizamos la z de la pelota
            //CORREGIRRRRRRRRRRRR HACE CUALQUIER COSA
            //################################################################################################################################
            angulo = (Math.Atan2(distanciaZPelotaArcoOp, distanciaXPelotaArcoOp) * (180 / Math.PI));
            Debug.Log("ANGULO::::" + angulo);

            zToSumm = Math.Sin((Math.PI / 180) * angulo) * distanciaTest;
            xToSumm = Math.Cos((Math.PI / 180) * angulo) * distanciaTest;

            if (opponentGoal.transform.position.x < 0 && ballPosition.Z > 0)
            {
                ObjetivoPosition.X = ballPosition.X + xToSumm;
                ObjetivoPosition.Z = ballPosition.Z + zToSumm;
            } else if (opponentGoal.transform.position.x < 0 && ballPosition.Z < 0)
            {
                ObjetivoPosition.X = ballPosition.X + xToSumm;
                ObjetivoPosition.Z = ballPosition.Z - zToSumm;
            } else if (opponentGoal.transform.position.x > 0 && ballPosition.Z > 0)
            {
                ObjetivoPosition.X = ballPosition.X - xToSumm;
                ObjetivoPosition.Z = ballPosition.Z + zToSumm;
            } else if (opponentGoal.transform.position.x > 0 && ballPosition.Z < 0)
            {
                ObjetivoPosition.X = ballPosition.X - xToSumm;
                ObjetivoPosition.Z = ballPosition.Z - zToSumm;
            }
            
            // el angulo de Atan2 me devuelve en radianes o sea de 0 a PI por lo que lo convierto a grados
            
            


            //ObjetivoPosition.X = distanciaXPelotaArcoOp + opponentGoal.transform.position.x + xToSumm;
            //ObjetivoPosition.Z = distanciaZPelotaArcoOp + opponentGoal.transform.position.z + zToSumm;

            //ObjetivoPosition.X = distanciaXPelotaArcoOp + xToSumm;
            //ObjetivoPosition.Z = distanciaZPelotaArcoOp + zToSumm;
            //################################################################################################################################
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
        distanciaXPelotaArco = Math.Abs(ballPosition.X - goalPosition.X);
        distanciaZPelotaArco = Math.Abs(ballPosition.Z - goalPosition.Z);
        //Debug.Log("Distania: " + distanciaX);

        distanciaPelotaArco = (float)Math.Sqrt((distanciaXPelotaArco * distanciaXPelotaArco) + (distanciaZPelotaArco * distanciaZPelotaArco));
    }
    void actualizarDistanciaPelotaArcoOpponent()
    {
        distanciaXPelotaArcoOp = Math.Abs(ballPosition.X - opponentGoal.transform.position.x);
        distanciaZPelotaArcoOp = Math.Abs(ballPosition.Z - opponentGoal.transform.position.z);
        //Debug.Log("Distania: " + distanciaX);

        //distanciaPelotaArco = (float)Math.Sqrt((distanciaXPelotaArco * distanciaXPelotaArco) + (distanciaZPelotaArco * distanciaZPelotaArco));
    }
    void actualizarDistanciaIAPelota()
    {
        distanciaXIAPelota = Math.Abs(ballPosition.X - currentPosition.X);
        distanciaZIAPelota = Math.Abs(ballPosition.Z - currentPosition.Z);
        //Debug.Log("Distania: " + distanciaX);

        distanciaIAPelota = (float)Math.Sqrt((distanciaXIAPelota * distanciaXIAPelota) + (distanciaZIAPelota * distanciaZIAPelota));
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

    void definir() //Cambia el objetivo a moverse hacia la pelota y pregunta la distancia (si es menor a cierto numero patea)
    {
        actualizarDistanciaIAPelota();
        if (distanciaIAPelota < 2.5)
        {
            if (Tipo == Tipos.Forward)
            {
                if (ball.transform.position.y < 3)
                {

                    if (delayToKick <= 0)
                        patear();

                }
            } else
            {
                if (ball.transform.position.y < 4.5)
                {

                    if (delayToKick <= 0)
                        patear();

                }
            }
        }
    }
}
