using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class GoalkeeperAITest : MonoBehaviour
{
    public Agent playerAIAgent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BallAI>(out BallAI ballAI))
        {
            playerAIAgent.SetReward(-1f);
            playerAIAgent.EndEpisode();
        }
    }
}
