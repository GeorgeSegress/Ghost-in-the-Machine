using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum AIState
{
    Looping,
    Following
}
public class Enemy : MonoBehaviour
{
    public GameObject loopObj;
    public bool chaser;
    public Transform[] loops;
    private int loopSpot = 0;

    private NavMeshAgent myAgent;

    private Transform target;
    private AIState curState = AIState.Looping;

    float waitBar = 0;

    private bool iterating;

    public AudioSource beep;
    public AudioSource unbeep;

    void Start()
    {
        loops = loopObj.GetComponent<EnemyPathTool>().GetLoop();
        myAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(IterateLoop());
    }

    public void FixedUpdate()
    {
        switch(curState)
        {
            case AIState.Following:
                myAgent.SetDestination(target.position);
                if (target.GetComponent<FirstPersonMovement>().curState == RunState.Idle)
                {
                    myAgent.isStopped = true;
                    waitBar += Time.deltaTime;
                    if(waitBar >= 3)
                    {
                        curState = AIState.Looping;
                        myAgent.isStopped = false;
                        StartCoroutine(IterateLoop());
                    }
                }
                else
                    myAgent.isStopped = false;
                break;
            case AIState.Looping:
                if (target && target.GetComponent<FirstPersonMovement>().curState != RunState.Idle)
                    SawPlayer(target);
                if (myAgent.remainingDistance < 2)
                    StartCoroutine(IterateLoop());
                break;
        }
    }

    private IEnumerator IterateLoop()
    {
        if (!iterating)
        {
            iterating = true;
            myAgent.destination = loops[loopSpot].position;
            loopSpot++;
            if (loopSpot >= loops.Length)
                loopSpot = 0;
            yield return new WaitForSeconds(.5f);
            iterating = false;
        }
    }

    public void SawPlayer(Transform trans)
    {
        waitBar = 0;
        target = trans;
        beep.Play();
        myAgent.SetDestination(trans.position);
        curState = AIState.Following;
    }

    public void UnsawPlayer()
    {
        target = null;
        unbeep.Play();
        if(curState == AIState.Following)
        {
            curState = AIState.Looping;
            StartCoroutine(IterateLoop());
        }
    }
}
