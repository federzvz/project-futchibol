using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBouncy : MonoBehaviour
{
    public Vector3 startForce;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(startForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
