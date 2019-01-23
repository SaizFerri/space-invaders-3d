using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    // Global GameObjects
    private UIManager _uiManager;
    private GameManager _gameManager;
    private PlayerScore _playerScore;

    [SerializeField]
    private Vector3 bounds = new Vector3(75, 25, 125);

    private bool _isPlaying = false;

    // Audio
    private AudioSource _audioSource;

    // Prefabs
    [SerializeField]
    private GameObject _laserPlayer1Prefab;

    [SerializeField]
    private GameObject _laserPlayer2Prefab;

    [SerializeField]
    private GameObject _dobleLaserPlayer1Prefab;

    [SerializeField]
    private GameObject _dobleLaserPlayer2Prefab;

    [SerializeField]
    private GameObject _enemy1Prefab;

    [SerializeField]
    private GameObject _enemy2Prefab;

    // UI
    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private GameObject _scorePanel;

    [SerializeField]
    private Text _scorePlayer1;

    [SerializeField]
    private Text _scorePlayer2;

    [SerializeField]
    private Text _winText;

    [SerializeField]
    private Text _livesText;

    [SerializeField]
    private GameObject _backButton;

    // Camera
    public GameObject playerCamera;

    // Tag
    private string _tag;

    // Movement
    private Rigidbody _rigidbody;

    [SerializeField]
    private float _speed = 30;

    private bool isMoving = false;
    private float _moveX;
    private float _moveY;
    private float _moveZ;

    // Life
    [SerializeField]
    private int _lives = 10;

    // Laser and ship
    private bool _canShoot = true;
    private bool _canSpawnShip = true;
    private float _shotCooldown = 0.5f;
    private float _shipSpawnCooldown = 2f;
    private Vector3 _dobleLaserPosition = new Vector3(0.32f, 0.04f, 5.3f);
    private Vector3 _dobleLaserPosition2 = new Vector3(0.32f, 0.04f, -5.3f);

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _gameManager = FindObjectOfType<GameManager>();
        _uiManager = FindObjectOfType<UIManager>();
        _playerScore = FindObjectOfType<PlayerScore>();
        _audioSource = GetComponent<AudioSource>();
        _tag = gameObject.tag;
        _isPlaying = true;

        if (isLocalPlayer)
        {
            playerCamera.SetActive(true);
            _scorePanel.SetActive(true);
        }
        else
        {
            playerCamera.SetActive(false);
            _scorePanel.SetActive(false);
        }
    }

    void Update()
    {
        if(isLocalPlayer)
        {
            _moveX = Input.GetAxis("Horizontal");
            _moveY = Input.GetAxis("Vertical");
            _moveZ = Input.GetAxis("Depth");

            if (_moveX != 0 || _moveY != 0 || _moveZ != 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                OpenPausePanel(true);
            }

            Shoot();
            SpawnSpaceShip();
            UpdateLives();
            UpdateScoreUI();
            UpdateLivesUI();
            
            if(_playerScore.scorePlayer1 == 10 || _playerScore.scorePlayer2 == 10)
            {
                if(_isPlaying)
                {
                    SetWinOrLose();
                    _isPlaying = false;
                }
            }

            if(!_playerScore.isAlivePlayer1 || !_playerScore.isAlivePlayer2)
            {
                if(_isPlaying)
                {
                    SetWinOrLoseByLives();
                    _isPlaying = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(isMoving)
        {
            Move();
        }
    }

    public void DisconnectPlayer()
    {
        if (isServer)
        {
            _gameManager.StopHost();
        }
        else
        {
            _gameManager.StopClient();
        }
    }

    public void OpenPausePanel(bool backButtonStatus)
    {
        if (!backButtonStatus)
        {
            _uiManager.SetGameObjectStatus(_backButton, backButtonStatus);
        }
        _uiManager.SetGameObjectStatus(_scorePanel, false);
        _uiManager.SetGameObjectStatus(_pauseMenu, true);
        _canShoot = false;
    }

    public void ClosePausePanel()
    {
        _uiManager.SetGameObjectStatus(_scorePanel, true);
        _uiManager.SetGameObjectStatus(_pauseMenu, false);
        _canShoot = true;
    }

    void Move()
    {
        Vector3 direction;

        if(_tag == "Player2")
        {
            direction = new Vector3(-(_moveX), _moveY, -(_moveZ));
        }
        else
        {
            direction = new Vector3(_moveX, _moveY, _moveZ);
        }

        _rigidbody.MovePosition(transform.position + (direction * _speed * Time.deltaTime));
    }

    private void UpdateLives()
    {
        CmdUpdateLives();
    }

    private void UpdateScoreUI()
    {
        _scorePlayer1.text = _playerScore.scorePlayer1.ToString();
        _scorePlayer2.text = _playerScore.scorePlayer2.ToString();
    }

    private void UpdateLivesUI()
    {
        _livesText.text = "Lives: " + _lives.ToString();
    }

    private void SetWinOrLose()
    {
        if(_tag == "Player2" && _playerScore.scorePlayer1 == 10 && _playerScore.scorePlayer2 < 10)
        {
            _winText.text = "YOU LOSE!";
        }
        else if (_tag == "Player2" && _playerScore.scorePlayer1 < 10 && _playerScore.scorePlayer2 == 10)
        {
            _winText.text = "YOU WIN!";
        }
        else if (_playerScore.scorePlayer1 == 10 && _playerScore.scorePlayer2 < 10)
        {
            _winText.text = "YOU WIN!";
        }
        else if (_playerScore.scorePlayer1 < 10 && _playerScore.scorePlayer2 == 10)
        {
            _winText.text = "YOU LOSE!";
        }
        else if (_playerScore.scorePlayer1 == 10 && _playerScore.scorePlayer2 == 10)
        {
            _winText.text = "TIE!";
        }

        OpenPausePanel(false);
    }

    private void SetWinOrLoseByLives()
    {
        if (_tag == "Player2" && !_playerScore.isAlivePlayer2 && _playerScore.isAlivePlayer1)
        {
            _winText.text = "YOU WERE KILLED!";
        }
        else if(_tag == "Player2" && _playerScore.isAlivePlayer2 && !_playerScore.isAlivePlayer1)
        {
            _winText.text = "YOU WIN!";
        }
        else if(_tag == "Player" && _playerScore.isAlivePlayer2 && !_playerScore.isAlivePlayer1)
        {
            _winText.text = "YOU WERE KILLED!";
        }
        else if(_tag == "Player" && !_playerScore.isAlivePlayer2 && _playerScore.isAlivePlayer1)
        {
            _winText.text = "YOU WIN!";
        }

        OpenPausePanel(false);
    }

    [Command]
    private void CmdUpdateLives()
    {
        bool[] livesStatus = new bool[2] { true, true };

        if(_tag == "Player2" && _lives == 0)
        {
            livesStatus[1] = false;
            _playerScore.UpdateLives(livesStatus);
        }
        else if (_tag == "Player" && _lives == 0)
        {
            livesStatus[0] = false;
            _playerScore.UpdateLives(livesStatus);
        }
    }

    [Command]
    public void CmdSpawnLaser()
    {
        GameObject laser;

        _audioSource.Play();

        if (_tag == "Player2")
        {
            laser = Instantiate(_dobleLaserPlayer2Prefab, transform.position + _dobleLaserPosition2, new Quaternion(0, 180, 0, 0));
            NetworkServer.Spawn(laser);
        }
        else
        {
            laser = Instantiate(_dobleLaserPlayer1Prefab, transform.position + _dobleLaserPosition, Quaternion.identity);
            NetworkServer.Spawn(laser);
        }
    }

    [Command]
    public void CmdSpawnEnemySpaceShip()
    {
        GameObject spaceShip;

        if (_tag == "Player2")
        {
            spaceShip = Instantiate(_enemy2Prefab, transform.position + new Vector3(0, 0, -5.83f), new Quaternion(0, 180, 0, 0));
            NetworkServer.SpawnWithClientAuthority(spaceShip, connectionToClient);
        }
        else
        {
            spaceShip = Instantiate(_enemy1Prefab, transform.position + new Vector3(0, 0, 5.83f), Quaternion.identity);
            NetworkServer.SpawnWithClientAuthority(spaceShip, connectionToClient);
        }
    }

    void Shoot()
    {
        if(Input.GetButtonDown("Fire1") && _canShoot)
        {
            _canShoot = false;
            CmdSpawnLaser();
            StartCoroutine(ShotCooldown());
        }
    }

    void SpawnSpaceShip()
    {
        if(Input.GetButtonDown("Fire2") && _canSpawnShip)
        {
            _canSpawnShip = false;
            CmdSpawnEnemySpaceShip();
            StartCoroutine(ShipSpawnCooldown());
        }
    }

    public void Damage()
    {
        if (_lives > 0)
        {
            _lives -= 1;
        }
    }

    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(_shotCooldown);
        _canShoot = true;
    }


    IEnumerator ShipSpawnCooldown()
    {
        yield return new WaitForSeconds(_shipSpawnCooldown);
        _canSpawnShip = true;
    }
}
