using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class StelthNPC : MonoBehaviour
{
    [SerializeField] private NPCObject npc;
    [SerializeField] private NavMeshAgent agent;
    private int nowPositionIndex;

    private float waitTime;

    public void Start()
    {
        RandomList();  
    }
    public void FixedUpdate()
    {
        var hasIndexNext = nowPositionIndex < npc.PassPositionList.Count - 1;
        bool canGoing;

        if (hasIndexNext)
        {
            canGoing = npc.PassPositionList[nowPositionIndex + 1].w <= waitTime;
        }
        else
        {
            canGoing = npc.PassPositionList[0].w <= waitTime;
        }

        var isGoal = agent.remainingDistance <= 0.1f;
        if (isGoal && canGoing)
        {
            Debug.Log("目的地の更新");
            //待ち時間のリセット
            waitTime = 0;

            agent.destination = npc.PassPositionList[nowPositionIndex];
            if(hasIndexNext)
            {
                nowPositionIndex++;
            }
            else
            {
                RandomList();
                nowPositionIndex = 0;
            }
        }
        else if(isGoal)
        {
            waitTime += Time.fixedDeltaTime;
        }

        Debug.Log(waitTime);
    }
    private void RandomList()
    {
        if (npc.shouldRandomMove)
        {
            npc.PassPositionList = npc.PassPositionList.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}
