using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Laser : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField]
    private Vector3 _bounds = new Vector3(75, 25, 125);
    private string[] _playerTags = new string[2] { "Player", "Player2" };
    private string[] _laserTags = new string[2] { "Laser1", "Laser2" };

    [SerializeField]
    private float _speed = 150;

    private string _tag;

    void Start()
    {
        _tag = this.gameObject.tag;
    }

    void FixedUpdate()
    {
        _rigidbody.MovePosition(transform.position + (transform.forward * _speed * Time.deltaTime));

        if (transform.position.z > _bounds.z || transform.position.z < -_bounds.z)
        {
            DestroyLaser();
        }
    }

    /*void OnTriggerEnter(Collider other)
    {
        // If Laser collides with player, damage player and destroy laser
        if (other.tag == _playerTags[0] && _tag == _laserTags[1])
        {
            other.GetComponent<Player>().LaserDamage();
            DestroyLaser();
        }
        else if (other.tag == _playerTags[1] && _tag == _laserTags[0])
        {
            other.GetComponent<Player>().LaserDamage();
            DestroyLaser();
        }
    }*/

    // If the laser is fast or the connection too bad, OnTriggerStay is better to detect the collision
    void OnTriggerStay(Collider other)
    {
        // If Laser collides with player, damage player and destroy laser
        if (other.tag == _playerTags[0] && _tag == _laserTags[1])
        {
            other.GetComponent<Player>().Damage();
            DestroyLaser();
        }
        else if (other.tag == _playerTags[1] && _tag == _laserTags[0])
        {
            other.GetComponent<Player>().Damage();
            DestroyLaser();
        }
    }

    void DestroyLaser()
    {
        // Destroy dobleLaser GameObject
        if (this.gameObject.transform.parent)
        {
            Destroy(this.gameObject.transform.parent.gameObject);
        }
        Destroy(this.gameObject);
    }
}
