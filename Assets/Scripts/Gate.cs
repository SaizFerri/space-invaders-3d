using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Gate : NetworkBehaviour
{
    private string _tag;

    private void Start()
    {
        _tag = this.gameObject.tag;    
    }

    private void OnTriggerEnter(Collider other)
    {
        // If enemy ship gets through the gate update score in enemy player
        
    }
}
