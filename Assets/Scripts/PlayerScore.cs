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

    [SyncVar]
    public int livesPlayer1 = 10;

    [SyncVar]
    public int livesPlayer2 = 10;

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

    public void UpdateLives(int livesPlayer1, int livesPlayer2)
    {
        CmdUpdateLives(livesPlayer1, livesPlayer2);
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
        livesPlayer1 = 10;
        livesPlayer2 = 10;
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
        livesPlayer1 = 10;
        livesPlayer2 = 10;
    }

    [Command]
    private void CmdUpdateScore(int[] score)
    {
        scorePlayer1 = score[0];
        scorePlayer2 = score[1];
    }

    [Command]
    private void CmdUpdateLives(int livesPlayer1, int livesPlayer2)
    {
        this.livesPlayer1 = livesPlayer1;
        this.livesPlayer2 = livesPlayer2;

        if (this.livesPlayer1 == 0)
        {
            this.isAlivePlayer1 = false;
        }
        else if (this.livesPlayer2 == 0)
        {
            this.isAlivePlayer2 = false;
        }
    }

    public int[] GetScore()
    {
        return new int[2] { scorePlayer1, scorePlayer2 };
    }
}
