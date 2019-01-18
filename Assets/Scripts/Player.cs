using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Vector3 bounds = new Vector3(75, 25, 125);

    [SerializeField]
    private GameObject _laserPrefab;

    private Rigidbody _rigidbody;

    private float _speed = 30;
    private bool isMoving = false;
    private float _moveX;
    private float _moveY;
    private float _moveZ;
    private Vector3 _laserLeftPosition = new Vector3(-0.31f, 0.1f, 5.24f);
    private Vector3 _laserRightPosition = new Vector3(0.31f, 0.1f, 5.24f);

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
    }

    void Update()
    {
        _moveX = Input.GetAxis("Horizontal");
        _moveY = Input.GetAxis("Vertical");
        _moveZ = Input.GetAxis("Depth");

        if (_moveX != 0 || _moveY != 0 || _moveZ != 0)
        {
            isMoving = true;
        } else
        {
            isMoving = false;
        }

        Shoot();
    }

    void FixedUpdate()
    {
        if(isMoving)
        {
            Move();
        }
    }

    void Move()
    {
        Vector3 direction = new Vector3(_moveX, _moveY, _moveZ);

        _rigidbody.MovePosition(transform.position + (direction * _speed * Time.deltaTime));
    }

    void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(_laserPrefab, transform.position + _laserLeftPosition, Quaternion.identity);
            Instantiate(_laserPrefab, transform.position + _laserRightPosition, Quaternion.identity);
        }
    }
}
