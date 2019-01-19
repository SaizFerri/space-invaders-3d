using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkManager
{

    [SerializeField]
    private GameObject _spawnPointPlayer1;

    [SerializeField]
    private GameObject _spawnPointPlayer2;

    [SerializeField]
    private GameObject _player1Prefab;

    [SerializeField]
    private GameObject _player2Prefab;

    [SerializeField]
    private GameObject _menu;

    private void Start()
    {
        DontDestroyOnLoad(_menu.gameObject);
    }

    public void StartHost()
    {
        base.StartHost();
        _menu.SetActive(false);
    }

    public void ConnectClient()
    {
        base.StartClient();
        _menu.SetActive(false);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player;
        
        if(conn.connectionId == 0)
        {
            player = Instantiate(_player1Prefab, _spawnPointPlayer1.transform.position, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        } 
        else if(conn.connectionId != 0)
        {
            player = Instantiate(_player2Prefab, _spawnPointPlayer2.transform.position, new Quaternion(0, 180, 0, 0));
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }
}
