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
        if (_tag == "Player1Gate" && other.tag == "Player2Enemy")
        {
            GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
            player2.GetComponent<Player>().UpdateScore();
            Destroy(other.gameObject);
        }
        else if (_tag == "Player2Gate" && other.tag == "Player1Enemy")
        {
            GameObject player1 = GameObject.FindGameObjectWithTag("Player");
            player1.GetComponent<Player>().UpdateScore();
            Destroy(other.gameObject);
        }
    }
}
