using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIManager : NetworkBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _menu;

    [SerializeField]
    private GameObject _instructionsPanel;

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

    public void SetGameObjectStatus(GameObject panel, bool status)
    {
        panel.SetActive(status);
    }

    public void OpenInstructions()
    {
        if(!_gameManager.isPlaying)
        {
            SetGameObjectStatus(_instructionsPanel, true);
        }
    }

    public void CloseInstructions()
    {
        if (!_gameManager.isPlaying)
        {
            SetGameObjectStatus(_instructionsPanel, false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
