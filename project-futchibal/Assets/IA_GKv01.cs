using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chunk { 
    public Chunk(double xMin, double xMax, double zMin, double zMax)
    {
        this.zMin = zMin;
        this.xMin = xMin;
        this.zMax = zMax;
        this.xMax = xMax;
    }
    public double xMin { get; set; }
    public double xMax { get; set; }
    public double zMin { get; set; }
    public double zMax { get; set; }
}

public class Coordenadas
{
    public Coordenadas(double x, double z)
    {
        this.x = x;
        this.z = z;
    }
    public double x { get; set; }
    public double z { get; set; }
}

public enum Accion { KeepPosition, Defend, Attack, FreeMovement, Pass };

public enum Estado { GK, Defender, Midfielder, Forward };

public class CPU
{
    public CPU(GameObject yo, List<Chunk> historial, Coordenadas objetivo, Accion accion, Estado estado, Vector3 vectorPlayerPelota)
    {
        this.vectorPlayerPelota = new Vector3();
        this.yo = yo;
        this.historial = historial;
        this.objetivo = objetivo;
        this.estado = estado;
        this.accion = accion;
        this.izquierdaODerecha = 0f;
        this.abajoOArriba = 0f;
        this.isPlayerKicking = 0f;
        this.playerRigidbody = yo.GetComponent<Rigidbody>();
        this.velocidadReaccion = 0.2f;
        this.power = 1;
        this.kickPotencia = 1200;
    }
    public CPU(GameObject? yo, GameObject? pelota)
    {
        this.vectorPlayerPelota = new Vector3();
        this.yo = yo;
        this.playerRigidbody = yo.GetComponent<Rigidbody>();
        this.estado = Estado.GK;
        this.accion = Accion.KeepPosition;
        this.izquierdaODerecha = 0f;
        this.abajoOArriba = 0f;
        this.isPlayerKicking = 0f;
        this.velocidadReaccion = 0.2f;
        this.pelota = pelota;
        this.power = 1;
        this.kickPotencia = 1200;
}
    private float kickPotencia { get; set; }
    private float power { get; set; }
    private Rigidbody playerRigidbody { get; set; }
    private float izquierdaODerecha { get; set; }
    private float abajoOArriba { get; set; }
    private float fuerzaHorizontal { get; set; }
    private float fuerzaVertical { get; set; }
    private float isPlayerKicking { get; set; }
    private float velocidadReaccion { get; set; }
    private Accion accion { get; set; }
    private Estado estado { get; set; }
    public GameObject yo { get; set; }
    public GameObject pelota { get; set; }
    private List<Chunk> historial { get; set; }
    private Coordenadas objetivo { get; set; }
    private Vector3 vectorPlayerPelota { get; set; }
    private void update()
    {

    }
    public void calcVelocidadHorizontal(float izquierdaODerecha)
    {
        if (izquierdaODerecha == 0)
        {
            fuerzaHorizontal -= velocidadReaccion;
        }
        else if (izquierdaODerecha == 1)
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
    public void calcVelocidadVertical(float abajoOArriba)
    {
        if (abajoOArriba == 0)
        {
            fuerzaVertical -= velocidadReaccion;
        }
        else if (abajoOArriba == 1)
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
    public void kickBall(float isPlayerKicking)
    {
        if (isPlayerKicking == 1)
        {
            if ((pelota.transform.localPosition - playerRigidbody.transform.localPosition).magnitude <= 4) // Distancia requerida
            {
                vectorPlayerPelota = pelota.transform.localPosition - (new Vector3(playerRigidbody.transform.localPosition.x, playerRigidbody.transform.localPosition.y - (power - 0.5f), playerRigidbody.transform.localPosition.z));
                vectorPlayerPelota = vectorPlayerPelota.normalized;
                vectorPlayerPelota = vectorPlayerPelota * (power * power);
                pelota.GetComponent<Rigidbody>().AddForce(vectorPlayerPelota * kickPotencia);
            }
            power = 1f;
        }
    }
}

public class IA_GKv01 : MonoBehaviour
{
    private double anchoChunk = 0f;
    private double altoChunk = 0f;
    public double limiteDerecha, limiteIzquierda, limiteArriba, limiteAbajo;
    public GameObject arcoPropio, arcoRival, pelota;
    //public GameObject yo;

    public float potencia = 1200;
    public float power = 1;
    private float fuerzaHorizontal = 0f;
    private float fuerzaVertical = 0f;
    public float velocidadReaccion = 0.2f;

    
    public List<GameObject> equipoPropio;
    public List<GameObject> equipoRival;
    
    private List<CPU> equipoPropioCPU;
    private List<CPU> equipoRivalCPU;

    public List<Chunk> chunks;
    private bool soyLocal;

    void Start()
    {
        //for (int i = 0; i < equipoPropio.Count; i++)
        //{
        //    equipoPropioCPU.Add(new CPU(equipoPropio?[i], pelota));
        //}
        soyLocal = averiguarLocalidad();
        chunks = cargarChunks();
        //Debug.Log(soyLocal);
    }

    void Update()
    {
        //for (int i = 0; i < equipoPropio.Count; i++)
        //{
        //    getChunkActual(equipoPropioCPU[i].yo);
        //}
        //Debug.Log(getChunkActual().xMin);
    }

    bool averiguarLocalidad() // true si es local (izq) y false si es visitante (der)
    {
        return (arcoPropio.transform.position.x < arcoRival.transform.position.x);
    }

    List<Chunk> cargarChunks() // Divide la cancha en 96 Chunks y los retorna en una lista
    {
        List<Chunk> chunks = new List<Chunk>();
        anchoChunk = (Math.Abs(limiteIzquierda - limiteDerecha)) / 12;
        altoChunk = (Math.Abs(limiteAbajo - limiteArriba)) / 8;
        double xMin_ = 0f, zMin_ = 0f;
        if (limiteIzquierda < limiteDerecha)
        {
            xMin_ = limiteIzquierda;
            zMin_ = limiteArriba;
        } else
        {
            xMin_ = limiteDerecha;
            zMin_ = limiteAbajo;
        }
        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                if (limiteIzquierda < limiteDerecha)
                {
                    chunks.Add(new Chunk(xMin_, xMin_ + anchoChunk, zMin_, zMin_ + altoChunk));
                    xMin_ = xMin_ + anchoChunk;
                    if (j == 11)
                        xMin_ = limiteIzquierda;
                } else
                {
                    chunks.Add(new Chunk(xMin_, xMin_ - anchoChunk, zMin_, zMin_ - altoChunk));
                    xMin_ = xMin_ - anchoChunk;
                    if (j == 11)
                        xMin_ = limiteDerecha;
                }
            }
            if (limiteArriba < limiteAbajo)
                zMin_ = zMin_ + altoChunk;
            else
                zMin_ = zMin_ - altoChunk;
        }
        return chunks;
    }

    Chunk getChunkActual(GameObject Obj)
    {
        //test.transform.position.x 
        double xNormalizada, zNormalizada;
        xNormalizada = Obj.transform.position.x + Math.Abs((limiteIzquierda - limiteDerecha) / 2) - 1;
        zNormalizada = Math.Abs((limiteArriba - limiteAbajo) / 2) - Obj.transform.position.z - 1;
        int indice = (int) ((xNormalizada / anchoChunk) + Math.Floor(zNormalizada / altoChunk) * 12);
        //Debug.Log("X:" + xNormalizada);
        //Debug.Log("Z:" + zNormalizada);
        Debug.Log("CHUNK: " + indice);
        return chunks[indice];
    }

    bool quitarPelota(GameObject IA)
    {
        return true;
    }
    bool pasarPelota(GameObject IAFrom, GameObject IATo)
    {
        //IAFrom
        return true;
    }

//    if (Input.GetKey(izquierdaKey))
//    {
//        izquierdaODerechax = 0;
//    }
//    else if (Input.GetKey(derechaKey))
//{
//    izquierdaODerechax = 1;
//}
//if (Input.GetKey(abajoKey))
//{
//    arribaOAbajox = 0;
//}
//else if (Input.GetKey(arribaKey))
//{
//    arribaOAbajox = 1;
//}
//if (Input.GetKey(kick))
//{
//    patearONoPatearx = 1;
//}
//else
//{
//    patearONoPatearx = 0;
//}

    // --- START: FUNCTIONS NEEDS FROM Kick --- //
    
    // --- END: FUNCTIONS NEEDS FROM Kick --- //

}
