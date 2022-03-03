using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacter : MonoBehaviour
{

    public Transform target;
    public float pursueDistance = 10.0f;
    public float atackDistance = 10.0f;
    public float atackCooldown = 10.0f;
    public NavMeshAgent agent;
    IState currentState = new IdleState();

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        target = GameObject.Find("AI").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        IState nextState = currentState.OnUpdate(this);

        if (nextState != null)
        {
            currentState.OnExitState(this);
            currentState = nextState;
            currentState.OnEnterState(this);
        }
        
    }
}
