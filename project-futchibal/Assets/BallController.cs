using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject ball, ballShadow;
    public float shadowYAltitude;

    void Update()
    {
        ShadowAlwaysUnderBall();
    }

    public void ShadowAlwaysUnderBall() {
        ballShadow.transform.position = new Vector3(ball.transform.position.x, shadowYAltitude, ball.transform.position.z);
    }
}
