using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    private PlayerActions player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerActions>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            player.score += 1;
            Debug.Log("Bullethit");
            Debug.Log(player.score);
            Destroy(gameObject);
        }
    }
}
