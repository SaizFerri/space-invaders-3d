using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UIManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject _menu;

    [SerializeField]
    private GameObject _pauseMenu;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(_menu.gameObject);
    }

    public void SetMenuStatus(bool status)
    {
        _menu.SetActive(status);
    }
}
