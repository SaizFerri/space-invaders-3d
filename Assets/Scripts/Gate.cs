using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Gate : NetworkBehaviour
{
    [SerializeField]
    private PlayerScore _scoreManager;

    private string _tag;
    private string[] _enemyTags = new string[2] { "Player1Enemy", "Player2Enemy" };
    private string[] _gateTags = new string[2] { "Player1Gate", "Player2Gate" };

    private void Start()
    {
        _tag = this.gameObject.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        int[] score = _scoreManager.GetScore();

        // If enemy ship gets through the gate update score in enemy player
        if (_tag == _gateTags[1] && other.tag == "Player1Enemy")
        {
            int scorePlayer1 = score[0] + 1;
           _scoreManager.UpdateScore(new int[2] { scorePlayer1, score[1] });
        }
        else if (_tag == _gateTags[0] && other.tag == "Player2Enemy")
        {
            int scorePlayer2 = score[1] + 1;
            _scoreManager.UpdateScore(new int[2] { score[0], scorePlayer2 });
        }
    }
}
