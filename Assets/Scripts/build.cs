using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class build : MonoBehaviour
{
    public GameObject build_blue;

    public void spawn_blue()
    {   
        Instantiate(build_blue);
    }
}
