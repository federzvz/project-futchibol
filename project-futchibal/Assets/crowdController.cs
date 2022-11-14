using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crowdController : MonoBehaviour
{
    Rigidbody rigidBody; 
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(rigidBody.position.y);
        if(rigidBody.position.y < 8.9 && rigidBody.position.y >= 6.65){ // Segunda linea de hinchadas
            if(rigidBody.position.y <= 6.8){
                // rigidBody.AddForce(200);
                int rand = Random.Range(30, 60);
                rigidBody.AddForce(transform.up * rand);
            }
        } else if(rigidBody.position.y < 6.45 && rigidBody.position.y >= 4.34){ // Primera linea de hinchadas
            if(rigidBody.position.y <= 4.45){
                // rigidBody.AddForce(200);
                int rand = Random.Range(30, 60);
                rigidBody.AddForce(transform.up * rand);
            }
        } else { // Ultima linea de hinchadas
            if(rigidBody.position.y <= 9.15){
                // rigidBody.AddForce(200);
                int rand = Random.Range(30, 60);
                rigidBody.AddForce(transform.up * rand);
            }
        }
        
    }
}
