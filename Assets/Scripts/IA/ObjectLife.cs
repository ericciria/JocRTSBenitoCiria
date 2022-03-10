using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLife : MonoBehaviour
{
    [SerializeField] int health = 1;

    public void takeDamage(int damage)
    {
        health -= damage;
        //Debug.Log(health);
        if (health <= 0)
        {
            // He posat que destrueixi el parent per com he fet la torreta, que requeria d'un transform separat per tornar
            // a la posició original, i he hagut de posar-li parents a totes les unitats
            Destroy(transform.parent.gameObject);
        }
    }
    public void heal(int damage)
    {
        health += damage;
    }

    public void setHealth(int amount)
    {
        health = amount;
    }
    public int getHealth()
    {
        return health;
    }
}
