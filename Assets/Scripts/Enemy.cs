using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private string _tag;
    private string[] _enemyTags = new string[2] {"Player1Enemy", "Player2Enemy"};

    [SerializeField]
    private float _speed = 40;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _tag = this.gameObject.tag;
    }

    void Update()
    {
        _rigidbody.MovePosition(transform.position + (transform.forward * _speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        // If enemy ship collides with player, destroy ship and damage player
        if (_tag == _enemyTags[0] && other.tag == "Player2")
        {
            other.GetComponent<Player>().EnemyDamage();
            Destroy(this.gameObject);
        }
        // If enemy ship collides with player, destroy ship and damage player
        else if(_tag == _enemyTags[1] && other.tag == "Player")
        {
            other.GetComponent<Player>().EnemyDamage();
            Destroy(this.gameObject);
        }
        // If enemy ship collides with enemy laser, destroy ship and laser
        else if (_tag == _enemyTags[0] && other.tag == "Laser2")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        // If enemy ship collides with enemy laser, destroy ship and laser
        else if(_tag == _enemyTags[1] && other.tag == "Laser1")
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
