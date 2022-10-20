using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float xOriginal = 0f;
    public float yOriginal = 35f;
    public float zOriginal = -35f;
    private float x = 0f, y = 0f, z = 0f;
    private float xMayor = 0f , xMenor = 0f, zMayor = 0f, zMenor = 0f;
    public GameObject camera;
    public List<GameObject> team1;
    public List<GameObject> team2;
    public GameObject pelota;
    // Start is called before the first frame update
    void Start()
    {
        //x = 0f;
        //y = 35f;
        //z = 35f;
    }

    // Update is called once per frame
    void Update()
    {
        actualizarPosition();
    }
    void actualizarPosition()
    {
        //Me quedo con la 'x' y 'z' mas grande y mas chica
        if (team1.Count != 0)
        {
            xMayor = team1[0].transform.position.x;
            xMenor = team1[0].transform.position.x;
            zMayor = team1[0].transform.position.z;
            zMenor = team1[0].transform.position.z;
            for (int j = 0; j < team1.Count; j++)
            {
                if (team1[j].transform.position.x > xMayor)
                    xMayor = team1[j].transform.position.x;
                if (team1[j].transform.position.z > zMayor)
                    zMayor = team1[j].transform.position.z;
                if (team1[j].transform.position.x < xMenor)
                    xMenor = team1[j].transform.position.x;
                if (team1[j].transform.position.z < zMenor)
                    zMenor = team1[j].transform.position.z;
            }
        }

        if (team2.Count != 0)
        {
            for (int j = 0; j < team2.Count; j++)
            {
                if (team2[j].transform.position.x > xMayor)
                    xMayor = team2[j].transform.position.x;
                if (team2[j].transform.position.z > zMayor)
                    zMayor = team2[j].transform.position.z;
                if (team2[j].transform.position.x < xMenor)
                    xMenor = team2[j].transform.position.x;
                if (team2[j].transform.position.z < zMenor)
                    zMenor = team2[j].transform.position.z;
            }
        }

        if (pelota.transform.position.x > xMayor)
            xMayor = pelota.transform.position.x;
        if (pelota.transform.position.z > zMayor)
            zMayor = pelota.transform.position.z;
        if (pelota.transform.position.x < xMenor)
            xMenor = pelota.transform.position.x;
        if (pelota.transform.position.z < zMenor)
            zMenor = pelota.transform.position.z;

        //Ahora se toman los mayores y menores de esas coordenadas y se dividen a la mitad y se actualiza la posicion de la camara 

        //Debug.Log("xMayor: " + xMayor);
        //Debug.Log("xMenor: " + xMenor);
        //Debug.Log("zMayor: " + zMayor);
        //Debug.Log("zMenor: " + zMenor);

        //Para hallar el medio de dos numeros basta con sumarlos y al resultado lo dividimos entre 2

        x = ((xMenor + xMayor) / 2) + xOriginal;
        z = ((zMenor + zMayor) / 2) + zOriginal;
        Debug.Log("X: " + x);
        Debug.Log("Z: " + z);

        //camera.transform.Translate(new Vector3(x, y, z));
        camera.transform.position = new Vector3(x, yOriginal, z);
    }
}
