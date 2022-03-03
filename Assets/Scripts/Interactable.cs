using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Transform frontPlayer;
    public PlayerActions player;
    private bool grabbed, grabbable;

    private void Start()
    {
        grabbed = false;
        grabbable = true;
        player = GameObject.Find("Player").GetComponent<PlayerActions>();
    }

    private void Update()
    {
        if (grabbed)
        {
            this.transform.position = frontPlayer.position;
        }
    }

    public void OnInteract()
    {
        if (grabbable)
        {
            Debug.Log("AAA");
            if (!grabbed)
            {
                Invoke(nameof(canGrab), 1.0f);
                GetComponent<Rigidbody>().useGravity = false;
                this.transform.position = frontPlayer.position;
                this.transform.parent = GameObject.Find("FrontPlayer").transform;
                grabbed = !grabbed;
                grabbable = false;
            }
            else
            {
                Invoke(nameof(canGrab), 1.0f);
                GetComponent<Rigidbody>().useGravity = true;
                this.transform.parent = null;
                grabbed = !grabbed;
                grabbable = false;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cubepoint")
        {
            player.score += 1;
            Debug.Log("+1");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cubepoint")
        {
            player.score -= 1;
            Debug.Log("-1");
        }
    }

    private void canGrab()
    {
        grabbable = true;
    }
}
