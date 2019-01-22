using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScore : NetworkBehaviour
{
    public int scorePlayer1 = 0;
    public int scorePlayer2 = 0;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        ResetScore();
    }

    public void UpdateScore(int[] score)
    {
        CmdUpdateScore(score);
    }

    public void ResetScore()
    {
        CmdResetScore();
    }

    [Command]
    private void CmdResetScore()
    {
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        RpcResetScore();
    }

    [ClientRpc]
    private void RpcResetScore()
    {
        scorePlayer1 = 0;
        scorePlayer2 = 0;
    }

    [Command]
    private void CmdUpdateScore(int[] score)
    {
        scorePlayer1 = score[0];
        scorePlayer2 = score[1];
        RpcUpdateScore(score);
    }

    [ClientRpc]
    private void RpcUpdateScore(int[] score)
    {
        scorePlayer1 = score[0];
        scorePlayer2 = score[1];
    }

    public int[] GetScore()
    {
        return new int[2] { scorePlayer1, scorePlayer2 };
    }
}
