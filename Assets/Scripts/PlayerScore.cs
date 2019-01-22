using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScore : NetworkBehaviour
{
    [SyncVar]
    public int scorePlayer1 = 0;

    [SyncVar]
    public int scorePlayer2 = 0;

    [SyncVar]
    public bool isAlivePlayer1 = true;

    [SyncVar]
    public bool isAlivePlayer2 = true;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        ResetScore();
        ResetLives();
    }

    public void UpdateScore(int[] score)
    {
        CmdUpdateScore(score);
    }

    public void UpdateLives(bool[] lives)
    {
        CmdUpdateLives(lives);
    }

    public void ResetScore()
    {
        CmdResetScore();
    }

    public void ResetLives()
    {
        CmdResetLives();
    }

    [ClientRpc]
    public void RpcResetScore()
    {
        scorePlayer1 = 0;
        scorePlayer2 = 0;
    }

    [ClientRpc]
    public void RpcResetLives()
    {
        isAlivePlayer1 = true;
        isAlivePlayer2 = true;
    }

    [Command]
    private void CmdResetScore()
    {
        scorePlayer1 = 0;
        scorePlayer2 = 0;
    }

    [Command]
    private void CmdResetLives()
    {
        isAlivePlayer1 = true;
        isAlivePlayer2 = true;
    }

    [Command]
    private void CmdUpdateScore(int[] score)
    {
        scorePlayer1 = score[0];
        scorePlayer2 = score[1];
    }

    [Command]
    private void CmdUpdateLives(bool[] lives)
    {
        isAlivePlayer1 = lives[0];
        isAlivePlayer2 = lives[1];
    }

    public int[] GetScore()
    {
        return new int[2] { scorePlayer1, scorePlayer2 };
    }
}
