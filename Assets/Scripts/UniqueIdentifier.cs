using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueIdentifier : MonoBehaviour
{
    public string id;
    void Start()
    {
        id = Guid.NewGuid().ToString();
    }

}
