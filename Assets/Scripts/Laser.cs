using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 30;

    void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }
}
