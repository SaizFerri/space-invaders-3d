using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    private UIManager _uiManager;
    private GameManager _gameManager;
    private PlayerScore _playerScore;

    [SerializeField]
    private Vector3 bounds = new Vector3(75, 25, 125);

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

    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private GameObject _scorePanel;

    [SerializeField]
    private Text _scorePlayer1;

    [SerializeField]
    private Text _scorePlayer2;

    public GameObject playerCamera;

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
    private int _lifes = 5;
    private int _hitCount = 0;

    // Laser
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
        _tag = gameObject.tag;

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
                _uiManager.SetGameObjectStatus(_scorePanel, false);
                _uiManager.SetGameObjectStatus(_pauseMenu, true);
            }

            Shoot();
            SpawnSpaceShip();
            UpdateScoreUI();
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

    private void UpdateScoreUI()
    {
        _scorePlayer1.text = _playerScore.scorePlayer1.ToString();
        _scorePlayer2.text = _playerScore.scorePlayer2.ToString();
    }

    [Command]
    public void CmdSpawnLaser()
    {
        GameObject laser;

        if (_tag == "Player2")
        {
            laser = Instantiate(_dobleLaserPlayer2Prefab, transform.position + _dobleLaserPosition2, Quaternion.identity);
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

    public void LaserDamage()
    {
        if(_lifes > 0 && _hitCount == 2)
        {
            _lifes--;
            _hitCount = 0;
        }
        else if (_lifes > 0 && _hitCount != 2)
        {
            _hitCount++;
        }
        else
        {
            Debug.Log("");
            //Destroy(this.gameObject);
        }
    }

    public void EnemyDamage()
    {
        if (_lifes > 0)
        {
            _lifes--;
        }
        else
        {
            //Destroy(this.gameObject);
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
