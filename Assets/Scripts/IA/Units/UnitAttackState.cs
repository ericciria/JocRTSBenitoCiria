using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : UnitStates
{
    void UnitStates.OnEnterState(Unit unit)
    {
        //Debug.Log("Enter Attack State");
    }

    void UnitStates.OnExitState(Unit unit)
    {
        //Debug.Log("Exit Attack State");
    }

    UnitStates UnitStates.OnUpdate(Unit unit)
    {
        Vector3 directionToTarget;
        float angle;
        float distanceToPlayer;
        if (unit.target != null)
        {
            distanceToPlayer = Vector3.Distance(unit.target.position, unit.transform.position);
            directionToTarget = unit.transform.position - unit.target.position;
            angle = Vector3.Angle(unit.transform.forward, directionToTarget);
            //Debug.Log(Mathf.Abs(angle));

            if (distanceToPlayer > unit.attackDistance)
            {
                return new UnitPursueState();
            }
            else if (Mathf.Abs(angle) > 175)
            {
                if (unit.timeSinceLastAttack > unit.attackCooldown)
                {
                    unit.GetComponent<ParticleSystem>().Play();
                    Debug.Log("Attack");
                    if (unit.target.GetComponent<Unit>() != null)
                    {
                        if (unit.target.GetComponent<Unit>().typeOfUnit.Equals(unit.damageMultiplierType))
                        {
                            unit.target.gameObject.GetComponent<ObjectLife>().takeDamage(unit.attackDamage * unit.damageMultiplierAmount);
                        }
                        else
                        {
                            unit.target.gameObject.GetComponent<ObjectLife>().takeDamage(unit.attackDamage);
                        }
                    }
                    else if (unit.target.GetComponentInParent<Building>() != null)
                    {
                        if (unit.damageMultiplierType.Equals("Building"))
                        {
                            unit.target.gameObject.GetComponent<ObjectLife>().takeDamage(unit.attackDamage * unit.damageMultiplierAmount);
                        }
                        else
                        {
                            unit.target.gameObject.GetComponent<ObjectLife>().takeDamage(unit.attackDamage);
                        }
                    }
                    else
                    {
                        return new UnitIdleState();
                    }
                    
                    
                    unit.timeSinceLastAttack = 0;
                }
                //Debug.Log("Target in front of unit");
            }
            else
            {
                unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, Quaternion.LookRotation(unit.target.position - unit.transform.position), 5 * Time.deltaTime);
            }
        }
        else if (unit.nearestEnemy != null)
        {
            distanceToPlayer = Vector3.Distance(unit.nearestEnemy.transform.position, unit.transform.position);
            if (distanceToPlayer < unit.attackDistance)
            {
                directionToTarget = unit.transform.position - unit.nearestEnemy.transform.position;
                angle = Vector3.Angle(unit.transform.forward, directionToTarget);
                if (Mathf.Abs(angle) > 175)
                {
                    if (unit.timeSinceLastAttack > unit.attackCooldown)
                    {
                        unit.GetComponent<ParticleSystem>().Play();
                        Debug.Log("Attack");
                        unit.nearestEnemy.GetComponent<ObjectLife>().takeDamage(unit.attackDamage);
                        unit.timeSinceLastAttack = 0;
                    }
                    //Debug.Log("Target in front of unit");
                }
                else
                {
                    unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, Quaternion.LookRotation(unit.nearestEnemy.transform.position - unit.transform.position), 5 * Time.deltaTime);
                }
            }
            else
            {
                return new UnitIdleState();
            }
        }
        else
        {
            return new UnitIdleState();
        }

        return null;
    }
}
