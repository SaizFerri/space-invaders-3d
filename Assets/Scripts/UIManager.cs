using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject _menu;

    [SerializeField]
    private InputField _addressInput;
    private string _ipAddress = "";

    [SerializeField]
    private Text _playerConnectedText;

    [SerializeField]
    private GameObject _pauseMenu;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(_menu.gameObject);
    }

    public void GetAddressInput()
    {
        _ipAddress = _addressInput.text;
    }

    public void SetMenuStatus(bool status)
    {
        _menu.SetActive(status);
    }

    public string GetIPAddressValue()
    {
        return _ipAddress;
    }

    public void SetPlayerConnectedText(string text)
    {
        _playerConnectedText.text = text;
    }
}
