using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkManager
{
    private GameObject _player;
    public bool isPlaying = false;

    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private PlayerScore _playerScore;

    [SerializeField]
    private GameObject _spawnPointPlayer1;

    [SerializeField]
    private GameObject _spawnPointPlayer2;

    [SerializeField]
    private GameObject _player1Prefab;

    [SerializeField]
    private GameObject _player2Prefab;

    private void Start()
    {
        _uiManager = _uiManager.GetComponent<UIManager>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void ConnectHost()
    {
        base.networkAddress = _uiManager.GetIPAddressValue() != "" ? _uiManager.GetIPAddressValue() : "localhost";
        base.StartHost();
    }

    public void ConnectClient()
    {
        base.networkAddress = _uiManager.GetIPAddressValue() != "" ? _uiManager.GetIPAddressValue() : "localhost";
        base.StartClient();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {   
        if(conn.connectionId == 0)
        {
            _player = Instantiate(_player1Prefab, _spawnPointPlayer1.transform.position, Quaternion.identity);
        } 
        else if(conn.connectionId != 0)
        {
            _player = Instantiate(_player2Prefab, _spawnPointPlayer2.transform.position, new Quaternion(0, 180, 0, 0));
        }

        NetworkServer.AddPlayerForConnection(conn, _player, playerControllerId);
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        isPlaying = true;
        _uiManager.SetLobbyInfoText("Hosting on: " + base.networkAddress + ":" + base.networkPort);
        _uiManager.SetMenuStatus(false);
    }

    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        _uiManager.SetLobbyInfoText("Connecting...");
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        isPlaying = true;
        _uiManager.SetMenuStatus(false);
        _uiManager.SetLobbyInfoText("");
        _playerScore.ResetScore();
        _playerScore.ResetLives();
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        _playerScore.RpcResetScore();
        _playerScore.RpcResetLives();
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);
        _uiManager.SetLobbyInfoText("ERROR: Server is full or host does not exist.");
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        base.OnServerError(conn, errorCode);
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
        isPlaying = false;
        _uiManager.SetMenuStatus(true);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        isPlaying = false;
        _uiManager.SetMenuStatus(true);
    }
}
