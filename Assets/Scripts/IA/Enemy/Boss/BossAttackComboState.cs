using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackComboState : BossStates
{
    public void OnEnterState(AIBoss character)
    {
        Debug.Log("Enter Boss Attack Combo attack: " + character.currentCombo);
        character.m_currentWaitTime = 0f;
        character.m_waitTime = 1f;
    }

    public void OnExitState(AIBoss character)
    {
        Debug.Log("Exit Boss Attack Combo attack: " + character.currentCombo);
    }

    public BossStates OnUpdate(AIBoss character)
    {
        if (character.target != null)
        {
            if (character.currentCombo <= character.maxCombo - 1)
            {
                if (character.m_currentWaitTime > character.m_waitTime)
                {
                    character.currentCombo++;
                    character.m_currentWaitTime = 0;
                    character.GetComponent<ParticleSystem>().Play();
                    character.target.gameObject.GetComponent<ObjectLife>().takeDamage(character.attack);
                    return new BossAttackComboState();
                }
                character.m_currentWaitTime += Time.deltaTime;
            }
            else
            {
                character.currentCombo = 0;
                character.timeSinceLastAttack = 0;
                return new BossIdleWarState();
            }
        }
        else
        {
            character.currentCombo = 0;
            character.timeSinceLastAttack = 0;
            return new BossIdleWarState();
        }

        return null;
    }
}
