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
    [SerializeField] private Animator animator;

    private int nowPositionIndex;

    private float waitTime;

    public void Start()
    {
        agent.updatePosition = false;
        RandomList();
    }
    public void FixedUpdate()
    {
        if(npc is null)
        {
            return;
        }
        // ������Ȃ��̃A�j���[�V����
        // nextPosition����deltaPosition���Z�o
        var worldDeltaPosition = agent.nextPosition - transform.position;

        // �L�����N�^�[����_�ɂ���xz���ʂɎˉe����deltaPosition
        var dx = Vector3.Dot(transform.right, worldDeltaPosition);
        var dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Time.deltaTime���瑬�x���Z�o
        var velocity = deltaPosition / Time.deltaTime;

        animator.SetFloat("Speed", velocity.y);
        transform.position = agent.nextPosition;

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
            //�҂����Ԃ̃��Z�b�g
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
    }
    private void RandomList()
    {
        if (npc.shouldRandomMove)
        {
            npc.PassPositionList = npc.PassPositionList.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}
