using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLife : MonoBehaviour
{
    [SerializeField] int health = 10;

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
}
