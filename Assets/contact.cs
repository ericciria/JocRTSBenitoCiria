using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contact : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Construccio")
        {
            Debug.Log("NO");
        }
    }
}
