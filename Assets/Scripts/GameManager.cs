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
    }

    private void Update()
    {
        
    }

    public void StartHost()
    {
        base.networkAddress = _uiManager.GetIPAddressValue();
        base.StartHost();
        isPlaying = true;
        //_uiManager.SetMenuStatus(false);
    }

    public void ConnectClient()
    {
        base.networkAddress = _uiManager.GetIPAddressValue();
        base.StartClient();
        isPlaying = true;
        CmdOnPlayerConnect()
        //_uiManager.SetMenuStatus(false);
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

    [Command]
    public void CmdOnPlayerConnect()
    {
        _uiManager.SetPlayerConnectedText(_player.tag);
    }
}
