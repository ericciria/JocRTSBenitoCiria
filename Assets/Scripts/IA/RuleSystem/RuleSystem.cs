using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleSystem : MonoBehaviour
{
    private delegate bool Condition();
    private delegate void Action();

    [SerializeField] float evaluationRate = 0.1f;
    private float timeSinceLastEvaluation = Mathf.Infinity;

    List<Condition> conditions = new List<Condition>();
    List<Action> actions = new List<Action>();
    // Regla: tupla de condició-acció

    private void Awake()
    {
        conditions.Add(Condition1);
        conditions.Add(Condition2);
        conditions.Add(Condition3);
        actions.Add(Action1);
        actions.Add(Action2);
        actions.Add(Action3);


    }

    void Start()
    {
        
    }

    void Update()
    {
        timeSinceLastEvaluation += Time.deltaTime;
        if (timeSinceLastEvaluation > evaluationRate)
        {
            Evaluate();
            timeSinceLastEvaluation = 0;
        }
    }
    private void Evaluate()
    {
        Debug.Assert(conditions.Count == actions.Count); // Assert: Si no se cumple, el codigo peta y te indica donde
        for(int i = 0; i < conditions.Count; i++)
        {
            if (conditions[i]())
            {
                actions[i]();
            }
        }
    }

    // Llista funcions-condicio i llista funcions-accio
    private bool Condition1()
    {
        //Debug.Log("Condition1");
        return Random.Range(0f, 1f) >= 0.5;     //50% - 1/2
    }
    private bool Condition2()
    {
        //Debug.Log("Condition2");
        return Random.Range(0f, 1f) >= 0.66;    // 33% - 1/3
    }
    private bool Condition3()
    {
        //Debug.Log("Condition3");
        return Random.Range(0f, 1f) >= 0.75;    // 25% - 1/4
    }

    private void Action1()
    {
        Debug.Log("Action1");
    }
    private void Action2()
    {
        Debug.Log("Action2");
    }
    private void Action3()
    {
        Debug.Log("Action3");
    }

}
