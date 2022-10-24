using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class FederAI : Agent
{
    private float fuerzaHorizontal = 0f;
    private float fuerzaVertical = 0f;
    private float velocidadReaccion = 0.2f;
    private Vector3 vectorPlayerPelota = new Vector3();
    public float potencia = 300f;
    public KeyCode izquierdaKey;
    public KeyCode derechaKey;
    public KeyCode arribaKey;
    public KeyCode abajoKey;
    public KeyCode kick;
    public Transform playerInitialPosition, enemyInitialPosition, ballInitialPosition;
    public Rigidbody pelota;
    public float kickPotencia = 900;
    public float power = 1;
    public Transform coordenadaGolArcoPropio, coordenadaGolArcoEnemigo;
    public int setPlayerTeamNumber = 1;
    public List<Collider> wallPlayerLimits;
    public Collider playerCollider;
    public GameObject enemyPlayer;
    public Agent enemyPlayerAgentScript;

    [SerializeField] private Transform targetTransform;
    [SerializeField] private Rigidbody playerRigidbody;

    public override void Initialize()
    {
        for (int i = 0; i < wallPlayerLimits.Count; i++)
        {
            Physics.IgnoreCollision(playerCollider, wallPlayerLimits[i]);
        }
    }
    public override void OnEpisodeBegin()
    {

        pelota.isKinematic = true;
        pelota.isKinematic = false;
        transform.localPosition = playerInitialPosition.localPosition;
        //targetTransform.localPosition = ballInitialPosition.localPosition;
        targetTransform.localPosition = new Vector3(Random.Range(-34f, 34f), 1, Random.Range(-20f, 20f));
        enemyPlayer.transform.localPosition = new Vector3(Random.Range(32f, 38f), 1.5f, Random.Range(-3.49f, 3.49f));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(enemyPlayer.transform.localPosition);
        sensor.AddObservation(coordenadaGolArcoEnemigo.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float izquierdaODerecha = actions.DiscreteActions[0]; // 0=izquierda | 1=derecha
        float abajoOArriba = actions.DiscreteActions[1]; // 0=abajo | 1=arriba
        float patearONoPatear = actions.DiscreteActions[2]; // 0=NoPatear | 1 =patear

        //Debug.Log("DiscreteActions[0]" + actions.DiscreteActions[0]);
        //Debug.Log("DiscreteActions[1]" + actions.DiscreteActions[1]);
        //Debug.Log("DiscreteActions[2]" + actions.DiscreteActions[2]);

        // Player Movement
        calcVelocidadHorizontal(izquierdaODerecha);
        calcVelocidadVertical(abajoOArriba);
        playerRigidbody.AddForce(transform.right * fuerzaHorizontal * potencia);
        playerRigidbody.AddForce(transform.forward * fuerzaVertical * potencia);

        if ((pelota.transform.localPosition - playerRigidbody.transform.localPosition).magnitude <= 4)
        {
            //AddReward(+1f);
            //isPlayerOnPositionToKick();
            kickBall(patearONoPatear);
            if (targetTransform.localPosition.z <= 6 && targetTransform.localPosition.z >= -6)
            {
                kickBall(patearONoPatear);
            }
            else {
                kickBall(patearONoPatear);
            }
            /*else if (targetTransform.localPosition.z >= 10 && targetTransform.localPosition.z <= 20 || targetTransform.localPosition.z <= -10 && targetTransform.localPosition.z >= -20) {
                kickBall(patearONoPatear);
            }*/
            /*if (patearONoPatear == 1)
            {
                AddReward(+1f);
                kickBall(1);
            }
            else
            {
                kickBall(0);
            }*/
        }

        //isBallOnEnemyField();
        //isBallNearGoalEnemy();
        CheckGoal();
        AddReward(-1f / MaxStep);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        /*ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");*/

        int arribaOAbajox = 3, izquierdaODerechax = 3, patearONoPatearx = 3;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        if (Input.GetKey(izquierdaKey))
        {
            izquierdaODerechax = 0;
        }
        else if (Input.GetKey(derechaKey))
        {
            izquierdaODerechax = 1;
        }
        if (Input.GetKey(abajoKey))
        {
            arribaOAbajox = 0;
        }
        else if (Input.GetKey(arribaKey))
        {
            arribaOAbajox = 1;
        }
        if (Input.GetKey(kick))
        {
            patearONoPatearx = 1;
        }
        else
        {
            patearONoPatearx = 0;
        }

        discreteActions[0] = izquierdaODerechax;
        discreteActions[1] = arribaOAbajox;
        discreteActions[2] = patearONoPatearx;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("HUBO COLISIÓN");
        if (setPlayerTeamNumber == 1)
        {
            if (other.TryGetComponent<WallAI>(out WallAI wallAI))
            {
                //Debug.Log("HUBO COLISIÓN: SE DETECTO QUE CHOCÓ CON PARED");
                SetReward(-1f);
                //ResetMatchGame();
                EndEpisode();
                /*if (enemyPlayerAgentScript != null) {
                    enemyPlayerAgentScript.EndEpisode();
                }*/
            }
        }
        else {
            if (other.TryGetComponent<WallAI>(out WallAI wallAI))
            {
                transform.localPosition = playerInitialPosition.localPosition;
            }
        }
    }

    // --- START: FUNCTIONS NEEDS FROM PlayerMovementController --- //
    void calcVelocidadHorizontal(float izquierdaODerecha)
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
    void calcVelocidadVertical(float abajoOArriba)
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
    // --- END: FUNCTIONS NEEDS FROM PlayerMovementController --- //

    // --- START: FUNCTIONS NEEDS FROM Kick --- //
    public void kickBall(float isPlayerKicking)
    {
        /*if (isPlayerKicking == 1)
        {
            if (power < 2)
                power += 0.02f;
        }*/
        if (isPlayerKicking == 1)
        {
            if ((pelota.transform.localPosition - playerRigidbody.transform.localPosition).magnitude <= 4) // Distancia requerida
            {
                //AddReward(+1f);
                vectorPlayerPelota = pelota.transform.localPosition - (new Vector3(playerRigidbody.transform.localPosition.x, playerRigidbody.transform.localPosition.y - (power - 0.5f), playerRigidbody.transform.localPosition.z));
                vectorPlayerPelota = vectorPlayerPelota.normalized;
                vectorPlayerPelota = vectorPlayerPelota * (power * power);
                //Debug.Log(vectorPlayerPelota.magnitude);
                pelota.AddForce(vectorPlayerPelota * kickPotencia);
            }
            else
            {
                //AddReward(-1f);
            }
            power = 1f;
        }
    }
    // --- END: FUNCTIONS NEEDS FROM Kick --- //

    // --- START: CHECK IF PLAYER SCORED OR HAS BEEN SCORED --- //
    public void CheckGoal()
    {
        if (setPlayerTeamNumber == 1)
        {
            if (pelota.transform.localPosition.x >= coordenadaGolArcoEnemigo.localPosition.x)
            {
                //Debug.Log("ROJO MARCA: GOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOL");
                AddReward(+100f);
                //ResetMatchGame();
                EndEpisode();
            }
            if (pelota.transform.localPosition.x <= coordenadaGolArcoPropio.localPosition.x)
            {
                //Debug.Log("ROJO CAGADA: NONONOONONONONONONONONO GOL EN PROPIA NONO NO NO NO NO NO NO N O");
                SetReward(-1f);
                //ResetMatchGame();
                EndEpisode();
            }
        }
        if (setPlayerTeamNumber == 2)
        {
            if (pelota.transform.localPosition.x <= coordenadaGolArcoEnemigo.localPosition.x)
            {
                //Debug.Log("AMARILLO MARCA: GOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOL");
                AddReward(+1f);
                //ResetMatchGame();
                EndEpisode();
            }
            if (pelota.transform.localPosition.x >= coordenadaGolArcoPropio.localPosition.x)
            {
                //Debug.Log("AMARILLO CAGADA: NONONOONONONONONONONONO GOL EN PROPIA NONO NO NO NO NO NO NO N O");
                SetReward(-1f);
                //ResetMatchGame();
                EndEpisode();

            }
        }
    }
    // --- END: CHECK IF PLAYER SCORED OR HAS BEEN SCORED --- //

    // --- START: RESET GAME --- //
    public void ResetMatchGame()
    {
        pelota.isKinematic = true;
        pelota.isKinematic = false;
        //transform.localPosition = playerInitialPosition.localPosition;
        //targetTransform.localPosition = ballInitialPosition.localPosition; x34, z20
        targetTransform.localPosition = new Vector3(Random.Range(-34f, 34f), 1, Random.Range(-20f, 20f));

        //enemyPlayer.transform.localPosition = enemyInitialPosition.localPosition;
    }
    // --- END: RESET GAME --- //

    public void isBallOnEnemyField() {
        if (setPlayerTeamNumber == 1) {
            if (targetTransform.localPosition.x > 0)
            {
                AddReward(+0.1f);
            }
            else {
                AddReward(-0.1f);
            }
        }
        if (setPlayerTeamNumber == 2)
        {
            if (targetTransform.localPosition.x < 0)
            {
                AddReward(+0.1f);
            }
            else
            {
                AddReward(-0.1f);
            }
        }
    }

    public void isBallNearGoalEnemy() {
        float ballToMyOwnGoalDistance;
        float ballToEnemyGoalDistance;
        if (setPlayerTeamNumber == 1) {
            ballToMyOwnGoalDistance = Vector3.Distance(targetTransform.localPosition, coordenadaGolArcoPropio.localPosition);
            ballToEnemyGoalDistance = Vector3.Distance(targetTransform.localPosition, coordenadaGolArcoEnemigo.localPosition);
            if (ballToMyOwnGoalDistance > ballToEnemyGoalDistance) {
                if (ballToEnemyGoalDistance < 5)
                {
                    AddReward(+1f);
                }
            }

        }
        if (setPlayerTeamNumber == 2)
        {
            ballToMyOwnGoalDistance = Vector3.Distance(targetTransform.localPosition, coordenadaGolArcoEnemigo.localPosition);
            ballToEnemyGoalDistance = Vector3.Distance(targetTransform.localPosition, coordenadaGolArcoPropio.localPosition);
            if (ballToMyOwnGoalDistance < ballToEnemyGoalDistance)
            {
                AddReward(-1f);
            }
            else
            {
                AddReward(+1f);
            }
            if (ballToEnemyGoalDistance < 15)
            {
                AddReward(+1f);
            }
            else
            {
                AddReward(-1f);
            }
        }
    }

    public void isPlayerOnPositionToKick() {
        if (setPlayerTeamNumber == 1) {
            if (transform.localPosition.x <= targetTransform.localPosition.x)
            {
                AddReward(+1f);
            }
            else {
                AddReward(-1f);
            }
        }
        if (setPlayerTeamNumber == 2)
        {
            if (transform.localPosition.x >= targetTransform.localPosition.x)
            {
                AddReward(+1f);
            }
            else
            {
                AddReward(-1f);
            }

        }
    }
}
