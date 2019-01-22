using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour
{
    private Rigidbody _rigidbody;
    private string _tag;
    private string[] _enemyTags = new string[2] {"Player1Enemy", "Player2Enemy"};
    private string[] _playerTags = new string[2] { "Player", "Player2" };
    private string[] _laserTags = new string[2] { "Laser1", "Laser2" };

    [SerializeField]
    private float _speed = 50;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _tag = this.gameObject.tag;
    }

    void FixedUpdate()
    {
        _rigidbody.MovePosition(transform.position + (transform.forward * _speed * Time.deltaTime));
    }

    private void OnTriggerStay(Collider other)
    {
        // If enemy ship collides with player, destroy ship and damage player
        if (_tag == _enemyTags[0] && other.tag == _playerTags[1])
        {
            other.GetComponent<Player>().Damage();
            Destroy(this.gameObject);
        }
        // If enemy ship collides with player, destroy ship and damage player
        else if(_tag == _enemyTags[1] && other.tag == _playerTags[0])
        {
            other.GetComponent<Player>().Damage();
            Destroy(this.gameObject);
        }
        // If enemy ship collides with enemy laser, destroy ship and laser
        else if (_tag == _enemyTags[0] && other.tag == _laserTags[1])
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        // If enemy ship collides with enemy laser, destroy ship and laser
        else if(_tag == _enemyTags[1] && other.tag == _laserTags[0])
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        // If 2 enemy ships collide destroy both
        else if (_tag == _enemyTags[0] && other.tag == _enemyTags[1])
        {
            Destroy(this.gameObject);
        }
        // If 2 enemy ships collide destroy both
        else if (_tag == _enemyTags[1] && other.tag == _enemyTags[0])
        {
            Destroy(this.gameObject);
        }
    }
}
